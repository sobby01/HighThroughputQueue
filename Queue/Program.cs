using HighThroughputQueue;
using QueueClient;

Console.WriteLine("Processing  Queues");

QueueProcessor2 processor1 = new QueueProcessor2();
processor1.Process();

