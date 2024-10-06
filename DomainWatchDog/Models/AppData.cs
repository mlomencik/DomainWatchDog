namespace DomainWatchDog.Models;
public class AppData(string dnsServer, string domain, string rsyslogServer, string fileLocation, int refreshInterval)
{
    public string DNSServer { get; set; } = dnsServer;
    public string Domain { get; set; } = domain;
    public string RSyslogServer { get; set; } = rsyslogServer;
    public string AddressFilePath { get; set; } = fileLocation;
    public int RefreshInterval { get; set; } = refreshInterval;
}
