using System;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;

namespace PlainConsoleLogger
{
    internal class PlainConsoleLogger : ILogger
    {
        private readonly string categoryName;
        private readonly PlainConsoleLoggerQueue messageQueue;

        internal PlainConsoleLogger(string categoryName, PlainConsoleLoggerQueue messageQueue)
        {
            // Keep only the class name (after the last dot)
            this.categoryName = categoryName.Substring(categoryName.LastIndexOf('.') + 1);

            this.messageQueue = messageQueue;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string message = formatter(state, exception);

            if (string.IsNullOrEmpty(message) && exception == null)
            {
                return;
            }

            string line = FormatMessageDefault(logLevel, message, exception);

            this.messageQueue.EnqueueMessage(line);
        }

        private string FormatMessageDefault(LogLevel logLevel, string message, Exception exception)
        {
            // Example:
            // 2018-07-30T22:29:32 INFO [Program] Request received

            StringBuilder builder = new StringBuilder();
            string logLevelString = GetLogLevelString(logLevel);

            // Add UTC DateTime in ISO 8601 format
            builder.Append(DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture));

            // Add the log level
            builder.Append(" ");
            builder.Append(logLevelString);

            // Add the class name
            builder.Append(" [");
            builder.Append(this.categoryName);
            builder.Append("] ");

            // Add the message
            if (!string.IsNullOrEmpty(message))
            {
                builder.AppendLine(message);
            }

            // Add the exception with stack trace
            // Example:
            // System.InvalidOperationException
            //    at Namespace.Class.Method() in File:line X
            if (exception != null)
            {
                // Indentation
                string[] exceptionLines = exception.ToString().Split('\n');

                foreach (string line in exceptionLines)
                {
                    builder.AppendLine(new string(' ', 4) + line);
                }
            }

            return builder.ToString();
        }

        private string GetLogLevelString(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return "TRACE";
                case LogLevel.Debug:
                    return "DEBUG";
                case LogLevel.Information:
                    return "INFO";
                case LogLevel.Warning:
                    return "WARN";
                case LogLevel.Error:
                    return "ERROR";
                case LogLevel.Critical:
                    return "CRIT";
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
