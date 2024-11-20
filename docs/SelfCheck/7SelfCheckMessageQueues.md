### 1. What is a message queue? What do message queues store and transfer?

> Message queue is a form of asynchronous communication between services. It stores and transfer messages which contain metadata and actual message data.<br>
> Allows cross server (or serverless) communication and some degree of reliability in case client is temporarly unavailable. 


### 2. Describe the publisher/subscriber pattern. The difference between Pub/Sub and Observable patterns.

> Pub/Sub pattern describes communication between publishers and subscribers through a broker which preserves messages by topic(channel) from which multiple subscribers can read messages (in case of channel single read is expected, I guess).<br>
> Observable pattern specifies that observable object contains a list of observers which are notified about state changes of observable object. It notifies observers directly and can be simply implemented if both observers and observable are parts of a single application. <br>
> In Pub/Sub pattern publisher and subscriber are decoupled and usually are different applicatons, used in cross-application communication.<br>
> Observable and observers are tightly coupled and aware of each other, are parts of a single application and used for internal event-handling.

### 3. What is a Message Bus? How does it work?

> Message bus is a communication system for application components. <br>
> It consists of multiple parts: message broker, message channel, message sender and message receiver. Message sender sends a message to a message broker which directs it to appropriate message channel on which message receiver(s) is listening.<br>
> One of many advantages in this case would be fault tolerance in case network or receiver has failed - message bus should preserve message until it is acknowledged by a receiver.<br>

### 4. What is the difference between message queue and web services?

> Web services use synchronous communication which affect latency of requests. <br>
> Real-time communication might be required and that's a typical use case. <br>
> Tightly coupled to common interface. <br>

> Message queues use asynchronous communication which allows clients to send request without waiting for response. <br>
> Easy horizontal-scalable by simply providing more producers and consumers.<br>
> Publisher and subscribers are decoupled. <br>

### 5. Describe the difference between RabbitMQ and Kafka. Provide some use cases for each of them: in which scenarios you'll use RabbitMQ, Kafka?

> RabbitMQ is a message broker based on queue data type.<br>
> Many consumers can be assigned to a single queue. Message is consumed only once.<br>
> Supports advanced routing mechanism to deliver message to a specific consumer using conditions.<br> 
> Push model.<br>
> Deletes message as soon as it is acknowledged by a consumer. What if during processing it fails? - can't replay, should be handled on consumer's side<br>

> Kafka is a message bus based on log data type for stream processing.<br>
> Consumers are distributed by topic partitions and message can be consumed by many consumers.<br>
> Pull model.<br>
> Deletes message after specified amount of time and it can be replayed by a consumer.<br>

> RabbitMQ use cases include asynchronouse communication between application services, complex routing and background jobs (sometimes no workload at all) <br>
> Kafka use cases would be logging, monitoring and other high throughput tasks (like big data processing and etc.). Also event sourcing to monitor changes over time.<br>