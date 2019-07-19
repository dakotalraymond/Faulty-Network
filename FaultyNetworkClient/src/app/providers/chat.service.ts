import { Injectable, NgZone } from '@angular/core';
import { Subject } from 'rxjs';
import { Message } from '../models/message.model';
import bitwise from 'bitwise';
import { Bit } from 'bitwise/types';
import { MessageType } from '../models/message-type.enum';
const dgram = require('dgram');

@Injectable()
export class ChatService {
  private server;
  private sendPort = 1982;
  private sendAddress = '127.0.0.1';
  private receivedMessages = new Subject<Message>();
  private messageQueue: Buffer[] = [];
  private receivedMessageIdCounter = 0;
  private sentMessageIdCounter = 0;
  private resendWorker;
  private resendInterval = 50; // Milliseconds

  // Is chat server initialized
  public isInitialized = false;

  // Received messages observable stream
  public receivedMessages$ = this.receivedMessages.asObservable();

  constructor(private zone: NgZone) {
    this.server = dgram.createSocket('udp4');
    this.server.on('message', this.onServerMessage);
    this.server.on('listening', this.onServerConnect);
  }

  /**
   * Initialize UDP socket for messaging
   * @param boundPort Port to bind to
   * @param sendPort Port to send to
   */
  initializeSocket(boundPort: number, sendPort: number) {
    this.server.bind(boundPort, '127.0.0.1', true);
    this.sendPort = sendPort;
    let connectMessage = Buffer.alloc(48);
    this.server.send(connectMessage, 0, connectMessage.length, this.sendPort, this.sendAddress, (err) => {
      if (err) {
        console.log(`There was an error sending udp message: ${err}`);
      }
      this.zone.run(() => {
        this.isInitialized = true;
      });
      this.resendWorker = setInterval(this.sendMessageQueue, this.resendInterval);
    });
  }

  /**
   * Unitialize UDP socket
   */
  unitializeSocket() {
    this.server.close();
    clearInterval(this.resendWorker);
  }

  /**
   * Send a message over bus
   * @param message Message to send
   */
  sendMessage(message: string) {
    let messageBuffer = this.createSendMessage(message);
    this.messageQueue.push(messageBuffer);

    // Save message to stored messages and push to subscribers
    let storedMessage = new Message(false, message);
    this.receivedMessages.next(storedMessage);
  }

  /**
   * Construct a message to send over bus
   * @param message Message body
   */
  private createSendMessage(message: string): Buffer {
    let length = 7 + message.length;
    let messageBuffer = Buffer.alloc(length);
    messageBuffer.writeUInt8(MessageType.Chat, 0);
    messageBuffer.writeUInt16LE(++this.sentMessageIdCounter, 1);
    messageBuffer.writeUInt16LE(message.length, 5);
    messageBuffer.write(message, 7, message.length);
    return messageBuffer;
  }

  /**
   * Handle incoming message
   */
  private onServerMessage = (msg: Buffer) => {
    let decodedMsg = this.decodeHammingBuffer(msg);
    let messageType = decodedMsg.readUInt8(0);

    switch (messageType) {
      case MessageType.Chat:
        this.handleReceiveChatMessage(decodedMsg);
        break;
      case MessageType.Ack:
        this.handleReceiveAck(decodedMsg);
        break;
      default:
        this.handleUnknownMessage(messageType);
        break;
    }
  }

  /**
   * Read chat message, display to user if not previously received
   * @param msg Message to handle
   */
  private handleReceiveChatMessage(msg: Buffer) {
    let receivedMessage = msg.toString('utf8', 7);
    let messageId = msg.readUInt16LE(1);
    this.sendAck(messageId);
    if (messageId === this.receivedMessageIdCounter + 1) {
      this.receivedMessageIdCounter++;

      // Run angular change detection
      this.zone.run(() => {
        this.receivedMessages.next(new Message(true, receivedMessage));
      });
    }
  }

  /**
   * Handle receiving unknown message type
   * @param messageType Message type
   */
  private handleUnknownMessage(messageType: number) {
    console.error(`An unknown message type was received: ${messageType}`);
  }

  /**
   * Send ack message after message receipt
   * @param messageId ID of message received
   */
  private sendAck(messageId: number) {
    let messageBuffer = Buffer.alloc(3);
    messageBuffer.writeUInt8(MessageType.Ack, 0);
    messageBuffer.writeUInt16LE(messageId, 1);
    this.encodeAndSend(messageBuffer);
  }

  /**
   * Remove acknowledged message from send queue
   * @param msg Received message
   */
  private handleReceiveAck(msg: Buffer) {
    let messageId = msg.readUInt16LE(1);
    this.messageQueue = this.messageQueue.filter((m) => {
      return (m.readUInt16LE(1) !== messageId) && (m.readUInt8(0) !== 2);
    });
  }

  /**
   * Send all messages in queue
   */
  private sendMessageQueue = () => {
    for (let buffer of this.messageQueue) {
      this.encodeAndSend(buffer);
    }
  }

  /**
   * Called on UDP server connect
   */
  private onServerConnect = () => {
    const address = this.server.address();
    console.log(`server listening ${address.address}:${address.port}`);
  }

  /**
   * Use hamming codes to add parity bits to buffer
   * @param buf Buffer to encode
   */
  private encodeHammingBuffer(buf: Buffer): Buffer {
    let outputBitArray: Bit[] = [];
    for (let i = 0; i < buf.length; i++) {
      let bitArray = bitwise.buffer.read(buf, i * 8, 8);
      // Calculate and add parity bits and return the new array ([7,4] Hamming code)
      let hamBit1: Bit = ((bitArray[0] + bitArray[1] + bitArray[3]) % 2) ? 1 : 0;
      let hamBit2: Bit = ((bitArray[0] + bitArray[2] + bitArray[3]) % 2) ? 1 : 0;
      let hamBit3: Bit = ((bitArray[1] + bitArray[2] + bitArray[3]) % 2) ? 1 : 0;
      let hamBit4: Bit = ((bitArray[4] + bitArray[5] + bitArray[7]) % 2) ? 1 : 0;
      let hamBit5: Bit = ((bitArray[4] + bitArray[6] + bitArray[7]) % 2) ? 1 : 0;
      let hamBit6: Bit = ((bitArray[5] + bitArray[6] + bitArray[7]) % 2) ? 1 : 0;
      let pushArray: Bit[] = [
        hamBit1, hamBit2, bitArray[0], hamBit3, bitArray[1], bitArray[2], bitArray[3], 0,
        hamBit4, hamBit5, bitArray[4], hamBit6, bitArray[5], bitArray[6], bitArray[7], 0
      ];
      outputBitArray.push(...pushArray);
    }
    return bitwise.buffer.create(outputBitArray);
  }

  /**
   * Use Hamming code to correct flipped bits in buffer and remove parity bits
   * @param buf Buffer to decode
   */
  private decodeHammingBuffer(buf: Buffer): Buffer {
    let outputBitArray: Bit[] = [];
    for (let i = 0; i < buf.length; i++) {
      let bitArray = bitwise.buffer.read(buf, i * 8, 8);
      this.fixBitArray(bitArray);

      // Create new array from original bits
      let cleanedArray: Bit[] = [
        bitArray[2], bitArray[4], bitArray[5], bitArray[6]
      ];
      outputBitArray.push(...cleanedArray);
    }
    return bitwise.buffer.create(outputBitArray);
  }

  /**
   * Encode a message and send over UDP socket
   * @param buf Message to send
   */
  private encodeAndSend(buf: Buffer): void {
    let encodedBuf = this.encodeHammingBuffer(buf);
    this.server.send(encodedBuf, 0, encodedBuf.length, this.sendPort, this.sendAddress, (err) => {
      if (err) {
        console.log(`There was an error sending udp message: ${err}`);
      }
    });
  }

  /**
   * Use Hamming codes to fix array of bits in place
   * @param array Array to fix
   */
  private fixBitArray(array: Bit[]): void {
    let hamBit1: Bit = ((array[2] + array[4] + array[6]) % 2) ? 1 : 0;
    let hamBit2: Bit = ((array[2] + array[5] + array[6]) % 2) ? 1 : 0;
    let hamBit3: Bit = ((array[4] + array[5] + array[6]) % 2) ? 1 : 0;
    let correctPosition = -1;
    if (hamBit1 !== array[0]) {
      correctPosition += 1;
    }
    if (hamBit2 !== array[1]) {
      correctPosition += 2;
    }
    if (hamBit3 !== array[3]) {
      correctPosition += 4;
    }
    if (correctPosition >= 0) {
      array[correctPosition] = array[correctPosition] ? 0 : 1;
    }
  }
}
