using HighThroughputQueue;
using QueueClient;

Console.WriteLine("Processing  Queues");

QueueProcessor1 processor1 = new QueueProcessor1();
processor1.Process();

