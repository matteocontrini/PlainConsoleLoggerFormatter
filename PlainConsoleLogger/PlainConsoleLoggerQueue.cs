using System;
using System.Collections.Concurrent;
using System.Threading;

namespace PlainConsoleLogger
{
    internal class PlainConsoleLoggerQueue : IDisposable
    {
        private readonly Thread thread;
        private readonly BlockingCollection<string> collection;
        private const int QUEUE_CAPACITY = 1024;

        public PlainConsoleLoggerQueue()
        {
            this.collection = new BlockingCollection<string>(QUEUE_CAPACITY);

            this.thread = new Thread(ProcessQueue)
            {
                IsBackground = true,
                Name = "Console logger queue processing thread"
            };

            this.thread.Start();
        }

        public void EnqueueMessage(string message)
        {
            try
            {
                this.collection.Add(message);
            }
            // Object is disposed or adding is complete
            catch
            {
                WriteMessage(message);
            }
        }

        private void WriteMessage(string message)
        {
            Console.Write(message);
        }

        private void ProcessQueue()
        {
            foreach (var message in this.collection.GetConsumingEnumerable())
            {
                WriteMessage(message);
            }
        }

        public void Dispose()
        {
            this.collection.CompleteAdding();

            try
            {
                this.thread.Join(TimeSpan.FromMilliseconds(1500));
            }
            catch (ThreadStateException)
            {
            }
        }
    }
}
