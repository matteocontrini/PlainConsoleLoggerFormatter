using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.IO;

namespace PlainConsoleLoggerFormatter
{
    public class PlainConsoleFormatter : ConsoleFormatter
    {
        private static readonly string padding = new string(' ', 4);
        private static readonly string newLineWithPadding = Environment.NewLine + padding;

        public PlainConsoleFormatter(PlainConsoleFormatterOptions _)
            : base(nameof(PlainConsoleFormatter))
        {
        }

        public PlainConsoleFormatter(IOptionsMonitor<PlainConsoleFormatterOptions> _)
            : base(nameof(PlainConsoleFormatter))
        {
        }

        public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter writer)
        {
            string message = logEntry.Formatter(logEntry.State, logEntry.Exception);

            if (string.IsNullOrEmpty(message) && logEntry.Exception == null)
            {
                return;
            }

            // Example:
            // 2018-07-30T22:29:32 INFO [Program] Request received

            string logLevelString = GetLogLevelString(logEntry.LogLevel);
            string className = logEntry.Category.Substring(logEntry.Category.LastIndexOf('.') + 1);

            // Add UTC DateTime in ISO 8601 format
            writer.Write(DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture));

            // Add the log level and class name
            writer.Write($" {logLevelString} [{className}] ");

            // Add the message
            if (!string.IsNullOrEmpty(message))
            {
                writer.WriteLine(message);
            }

            // Add the exception with stack trace
            if (logEntry.Exception != null)
            {
                writer.Write(padding);
                writer.WriteLine(FormatException(logEntry.Exception));
            }
        }

        /// <summary>
        /// Formats the exception so that stack trace lines are indented
        /// </summary>
        /// <param name="ex">Exception to be formatted</param>
        /// <returns>Formatted exception</returns>
        private string FormatException(Exception ex)
        {
            return ex
                .ToString()
                .Replace(Environment.NewLine, newLineWithPadding);
        }

        /// <summary>
        /// Maps a <see cref="LogLevel"/> to a string
        /// </summary>
        /// <param name="logLevel">The log level</param>
        /// <returns>String representation of the log level</returns>
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
    }
}
