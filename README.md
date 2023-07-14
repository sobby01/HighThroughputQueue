# HighThroughputQueue

A simple concurrent message queue implementation in C#.

Here we are designing a queue on a single machine, Following is the use case
1. High Throughput
2. Persistent

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Design](#design)
- [Installation](#installation)
- [Usage](#usage)

## Introduction

We implemented 4 versions of this message queue

1. In version 1, I implemented a basic queue with the number of workers as input and process the messages, but in this version, we can not achieve high throughput.
2. In version 2, I implemented multiple queues, each queue was associated with a separate worker which helped us to manage high throughput as it helps to process more items. But can we also introduce multiple threads to process more data
3. In Version 3, and the final version, I introduced multi-threading to process Enqueue and Dequeue on separate threads.

The Concurrent Message Queue is a C# library that provides a concurrent message queue implementation, allowing for the enqueueing and processing of messages in a multi-threaded environment. It offers the flexibility to attach multiple workers to different queues and process messages concurrently.

## Features

- Enqueue messages into different queues
- Attach multiple workers to process messages concurrently
- Thread-safe message processing using concurrent data structures

## Design

![image](https://github.com/sobby01/HighThroughputQueue/assets/3829498/3d11da67-7235-4e33-b8f3-e9b2591fe740)


## Installation

To use the Concurrent Message Queue library in your project, follow these steps:

1. Clone the repository: `git clone https://github.com/sobby01/HighThroughputQueue`
2. Build the solution in Visual Studio or using the .NET CLI: `dotnet build`
3. Reference the generated assembly in your project

## Usage

To use the Message Queue in your application, follow these steps:
 
1. Create an instance of the `MessageQueue<T>` class:
```csharp
   IMessageQueue<string> multiQueue = new MessageQueue<string>();
   ```

3. Create worker instances and attach them to specific queues:
```csharp
   IWorker<string> worker1 = new Worker<string>();
   IWorker<string> worker2 = new Worker<string>();
   multiQueue.AttachWorker("Queue1", worker1);
   multiQueue.AttachWorker("Queue2", worker2);
```

4. Start the workers to begin message processing:
```csharp
   worker1.StartWorking();
   worker2.StartWorking();
```

5. Enqueue messages into the respective queues:
```csharp
   multiQueue.Enqueue("Queue1", "Message 1");
   multiQueue.Enqueue("Queue2", "Message 2");
   multiQueue.Enqueue("Queue1", "Message 3");
```

6. Simulate processing time or wait for the workers to complete their processing.
7. Stop the workers:
```csharp
   worker1.StopWorking();
   worker2.StopWorking();
```

## Persistent
  Coming soon...

## Contact
For any inquiries or questions, feel free to contact me at programmingwithsobby@gmail.com

