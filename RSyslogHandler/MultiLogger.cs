using Serilog;
using Serilog.Core;
using Serilog.Sinks.Syslog;

namespace LoggingHandler;
public class MultiLogger
{
    public readonly Logger? ConsoleLogger;

    private readonly Logger? SyslogLogger;
    private readonly bool SyslogEnabled = true;

    public MultiLogger(string rsyslogServer)
    {
        if (!string.IsNullOrEmpty(rsyslogServer)) {
            SyslogLogger = new LoggerConfiguration().WriteTo
                .UdpSyslog(host: rsyslogServer, port: 514, format: SyslogFormat.RFC5424, appName: "DomainWatchDog")
                .CreateLogger();
        } else {
            SyslogEnabled = false;
        }
        ConsoleLogger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
    }

    public void SendRsyslogMessage(string message, string domain)
    {
        if (SyslogEnabled && SyslogLogger is not null) {
            SyslogLogger.Information($"Detected: {message} for domain - {domain}");
        }
    }
}
