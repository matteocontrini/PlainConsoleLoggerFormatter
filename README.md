# PlainConsoleLogger [![NuGet](https://img.shields.io/nuget/v/PlainConsoleLogger?color=success)](https://www.nuget.org/packages/PlainConsoleLogger) [![License](https://img.shields.io/github/license/matteocontrini/PlainConsoleLogger?color=success)](https://github.com/matteocontrini/PlainConsoleLogger/blob/master/LICENSE)

A basic alternative console logger for ASP.NET Core 2.x.
 
The main difference from the Microsoft's console logger is the log format:

- no support for colors
- better log levels mapping (TRACE, DEBUG, INFO, WARN, ERROR and CRIT), instead of weird abbreviations
- log messages are by default on a single line
- exceptions start on a new line and are indented with 4 characters
- the class name is simplified by removing the namespace

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
    logging.ClearProviders();
    logging.AddPlainConsole();
}
```
