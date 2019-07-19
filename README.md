# Faulty-Network

A example of using hamming codes for forward error correction in a UDP based chat app. The FaultyNetwork acts as the "Red Layer", the clients communicate through it and it will flip random bits and drop random messages. Using Hamming Codes and redundant messaging patterns, the chat clients maintain a seamless chat experience.

## Instructions

Build and start the FaultyNetwork app first, then follow instructions in the Faulty Network Client to start two instances of it. Select a different send and bind port for each client, then start chatting!