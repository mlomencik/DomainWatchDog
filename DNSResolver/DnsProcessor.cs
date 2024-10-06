using DnsClient;
using DnsClient.Protocol;
using System.Net;

namespace DNSResolver;
public class DnsProcessor(string dnsServer)
{
    private readonly LookupClient LookupClient = new(IPAddress.Parse(dnsServer));

    public List<IPAddress> ResolveDomain(string domain)
    {
        List<IPAddress> ipAddresses = [];

        IDnsQueryResponse result = LookupClient.Query(domain, QueryType.A);
        foreach (DnsResourceRecord? answer in result.Answers) {
            if (answer is ARecord aRecord) {
                ipAddresses.Add(aRecord.Address);
            }
        }
        return ipAddresses;
    }
}
