using DNSResolver.Models;

namespace DomainWatchDog.Models;
public class AppData(string dnsServer, string domain, string rsyslogServer, string fileLocation, int refreshInterval)
{
    public string DNSServer { get; set; } = dnsServer;
    public string Domain { get; set; } = domain;
    public string RSyslogServer { get; set; } = rsyslogServer;
    public string AddressFilePath { get; set; } = fileLocation;
    public int RefreshInterval { get; set; } = refreshInterval;
    public int A_TTL_LIMIT { get; set; } = 25;
    public List<PrivateIpRange>? PrivateIPv4Ranges { get; set; }
}
