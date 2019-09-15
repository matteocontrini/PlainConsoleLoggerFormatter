using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace PlainConsoleLogger
{
    public class PlainConsoleLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, ILogger> loggers;
        private readonly PlainConsoleLoggerQueue messageQueue;

        public PlainConsoleLoggerProvider()
        {
            this.loggers = new ConcurrentDictionary<string, ILogger>();
            this.messageQueue = new PlainConsoleLoggerQueue();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return this.loggers.GetOrAdd(categoryName, c => new PlainConsoleLogger(c, this.messageQueue));
        }

        public void Dispose()
        {
            this.messageQueue.Dispose();
        }
    }
}
