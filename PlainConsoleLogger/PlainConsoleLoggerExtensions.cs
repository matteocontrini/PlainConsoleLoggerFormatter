using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace PlainConsoleLogger
{
    public static class PlainConsoleLoggerExtensions
    {
        public static ILoggingBuilder AddPlainConsole(this ILoggingBuilder builder)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, PlainConsoleLoggerProvider>());

            return builder;
        }
    }
}
