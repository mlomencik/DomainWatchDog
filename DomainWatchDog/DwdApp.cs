using DnsClient.Protocol;
using DNSResolver;
using DomainWatchDog.Models;
using LoggingHandler;

namespace DomainWatchDog;

public class DwdApp(AppData appData, MultiLogger logger)
{
    private readonly AppData AppData = appData;
    private readonly DnsProcessor DnsProcessor = new(appData.DNSServer, logger);
    private readonly MultiLogger Logger = logger;
    private readonly ManualResetEventSlim exitEvent = new(false);
    private bool IsRunning = false;

    public void Start()
    {
        // Start a new thread for periodic domain resolution and IP checks
        IsRunning = true;
        Thread dnsCheckThread = new(DwdResolverThread) {
            IsBackground = true
        };
        dnsCheckThread.Start();

        // Wait for the exit event to be signaled (Ctrl+C)
        exitEvent.Wait();
    }

    public void Stop()
    {
        // Stop the thread gracefully
        IsRunning = false;
        // Signal the exit event to allow the application to close
        exitEvent.Set();
    }

    private void DwdResolverThread()
    {
        while (IsRunning) {
            try {
                // Resolve the domain using the specified DNS server
                List<DnsResourceRecord> resolvedRecords = DnsProcessor.ResolveDomain(AppData.Domain);

                // Check if any of the resolved IPs belong to private ranges
                foreach (DnsResourceRecord resolvedRecord in resolvedRecords) {
                    string? resultIp = IpProcessor.CheckSuspiciousIp(resolvedRecord, AppData.PrivateIPv4Ranges!);
                    if (resultIp != string.Empty) {
                        Logger.ConsoleLogger?.Information($"Alert - {resultIp} | Domain: {resolvedRecord.DomainName} | DNS server: {AppData.DNSServer}");
                        Logger.SendRsyslogMessage(resultIp, AppData.Domain);
                    }
                    string? resultTtl = IpProcessor.CheckSuspiciousTtl(resolvedRecord, AppData.A_TTL_LIMIT);
                    if (resultTtl != string.Empty) {
                        Logger.ConsoleLogger?.Information($"Alert - {resultTtl} | Domain: {resolvedRecord.DomainName} | DNS server: {AppData.DNSServer}");
                        Logger.SendRsyslogMessage(resultTtl, AppData.Domain);
                    }
                }
            }
            catch (Exception ex) {
                Logger.ConsoleLogger?.Error($"Error in DNS resolve: {ex.Message}");
            }
            // Sleep for the specified interval before running the next check
            Thread.Sleep(TimeSpan.FromSeconds(AppData.RefreshInterval));
        }
    }
}
