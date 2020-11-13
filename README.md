# PlainConsoleLoggerFormatter [![NuGet](https://img.shields.io/nuget/v/PlainConsoleLoggerFormatter?color=success)](https://www.nuget.org/packages/PlainConsoleLoggerFormatter) [![License](https://img.shields.io/github/license/matteocontrini/PlainConsoleLogger?color=success)](https://github.com/matteocontrini/PlainConsoleLogger/blob/master/LICENSE)

A basic alternative console logger formatter for ASP.NET 5.0.

The main differences from the default Microsoft's console formatter are:

- no support for colors
- better log levels mapping (TRACE, DEBUG, INFO, WARN, ERROR and CRIT), instead of weird abbreviations
- log messages are by default on a single line
- exceptions start on a new line and are indented with 4 characters
- the class name is simplified by removing the namespace

**Note**: custom console *formatters* were introduced with ASP.NET 5.0. For ASP.NET Core 2/3 refer to the previous versions of this package, which were published as [PlainConsoleLogger](https://www.nuget.org/packages/PlainConsoleLogger/).

## Example message

```
2018-07-30T22:29:32 INFO [Program] Text message here
```

The date is in UTC ISO 8601 format.

## Usage

- Install with [NuGet](https://www.nuget.org/packages/PlainConsoleLogger/)
- Register the logger through the `ILoggingBuilder`:

```csharp
private static void ConfigureLogging(HostBuilderContext hostContext, ILoggingBuilder logging)
{
    logging.AddConsoleFormatter<PlainConsoleFormatter, PlainConsoleFormatterOptions>();
    logging.AddConsole(options => options.FormatterName = nameof(PlainConsoleFormatter));
}
```
