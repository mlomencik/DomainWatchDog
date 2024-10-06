using DNSResolver;
using DomainWatchDog.Models;
using RSyslogHandler;

namespace DomainWatchDog;

public class DwdApp(AppData appData)
{
    private readonly AppData AppData = appData;
    private readonly DnsProcessor DnsProcessor = new(appData.DNSServer);
    private readonly RSyslog RSyslog = new(appData.RSyslogServer);
    
    private List<DNSResolver.Models.IPRange>? PrivateRanges;
    private bool IsRunning = false;

    public void Start()
    {
        // Load private IP ranges from the YAML file
        PrivateRanges = IpProcessor.LoadPrivateIpRanges(AppData.AddressFilePath);

        try {
            // Resolve the domain using the specified DNS server
            var resolvedIps = DnsProcessor.ResolveDomain(AppData.Domain);

            // Check if any of the resolved IPs belong to private ranges
            foreach (var ip in resolvedIps) {
                if (IpProcessor.IsPrivateIp(ip, PrivateRanges)) {
                    Console.WriteLine($"Private IP detected: {ip}");
                    RSyslog.Send(ip.ToString(), AppData.Domain);
                }
            }
        }
        catch (Exception ex) {
            // Log any exceptions
            Console.WriteLine($"Error in DNS check: {ex.Message}");
        }

        // Start a new thread for periodic domain resolution and IP checks
        //IsRunning = true;
        //Thread dnsCheckThread = new(ThreadCallback) {
        //    IsBackground = true
        //};
        //dnsCheckThread.Start();
    }

    public void Stop()
    {
        // Stop the thread gracefully
        IsRunning = false;
    }

    private void ThreadCallback()
    {
        while (IsRunning) {
            try {
                // Resolve the domain using the specified DNS server
                List<System.Net.IPAddress> resolvedIps = DnsProcessor.ResolveDomain(AppData.Domain);

                // Check if any of the resolved IPs belong to private ranges
                foreach (System.Net.IPAddress ip in resolvedIps) {
                    if (IpProcessor.IsPrivateIp(ip, PrivateRanges)) {
                        Console.WriteLine($"Private IP detected: {ip}");
                        RSyslog?.Send(ip.ToString(), AppData.Domain);
                    }
                }
            }
            catch (Exception ex) {
                // Log any exceptions
                Console.WriteLine($"Error in DNS check: {ex.Message}");
            }
            // Sleep for the specified interval before running the next check
            Thread.Sleep(TimeSpan.FromSeconds(AppData.RefreshInterval));
        }
    }
}
