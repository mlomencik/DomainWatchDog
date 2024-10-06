using Serilog;
using Serilog.Core;

namespace RSyslogHandler;
public class RSyslog
{
    private readonly Logger SyslogLogger;
    private readonly bool IsEnabled = true;

    public RSyslog(string rsyslogServer)
    {
        if (!string.IsNullOrEmpty(rsyslogServer)) {
            SyslogLogger = new LoggerConfiguration().WriteTo.UdpSyslog(rsyslogServer).CreateLogger();
        }
        IsEnabled = false;
    }

    public void Send(string detectedIp, string domain)
    {
        if (IsEnabled) {
            SyslogLogger.Information($"Private IP detected for domain {domain}: {detectedIp}");
        }
    }
}
