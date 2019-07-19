# Faulty-Network

A example of using Hamming codes for forward error correction in a UDP based chat app. The FaultyNetwork acts as the "Red Layer", the clients communicate through it and it will flip random bits and drop random messages. Using Hamming codes and redundant messaging patterns, the chat clients maintain a seamless chat experience.

## Instructions

Build and start the FaultyNetwork app first, then follow instructions in the Faulty Network Client to start two instances of it. Select a different send and bind port for each client, then start chatting!

## Message Structure

#### Connect
1. Message type: 0 (uint8, 1 byte)
2. Message ID (uint16, 2 bytes)
3. Name length (uint8, 1 byte)
4. Name (UTF-8, max length 15 bytes) - The name of the connecting party

#### Chat
1. Message type: 2 (uint8, 1 byte)
2. Message ID (uint16, 2 bytes) 
3. Message count (uint8, 1 byte) - number of messages for the entire chat
4. Message sequence (uint8, 1 byte) - sequence within the count, starting at 1  
5. Text length: (uint16, 2 bytes) - implying a max text length below 65535
6. Text (UTF-8)

#### Ack - To send in response to Chat
1. Message type: 4 (uint8, 1 byte)
2. Ack ID (uint16, 2 bytes) - The Message ID of the message being acknowledged

## Additional Info

Find more information on how Hamming codes work at https://en.wikipedia.org/wiki/Hamming_code