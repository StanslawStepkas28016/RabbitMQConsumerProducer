## RabbitMQConsumerProducer

This is a simple solution presenting the usage of
RabbitMQ for sending messages from one application to another application.

- **ProducerApi** - sends messages to the "release-car" queue on a car release which is triggered on the `api/car-factory/release` endpoint. 
- **ConsumerApi** - receives and logs the message send from the producer, while waiting for other messages.

Communication diagram:
![200](Assets/Communication%20Diagram.png)