using DnsClient;
using DnsClient.Protocol;
using LoggingHandler;
using System.Net;

namespace DNSResolver;
public class DnsProcessor(string dnsServer, MultiLogger logger)
{
    private readonly LookupClient LookupClient = new(IPAddress.Parse(dnsServer));
    private readonly MultiLogger Logger = logger;

    /// <summary>
    /// Resolves the specified domain using the configured DNS server.
    /// </summary>
    /// <param name="domain"></param>
    /// <returns></returns>
    public List<DnsResourceRecord> ResolveDomain(string domain)
    {
        Logger.ConsoleLogger?.Debug($"Resolving domain: {domain} from DNS server: {dnsServer}");
        List<DnsResourceRecord> resolvedAdresses = [];

        IDnsQueryResponse result = LookupClient.Query(domain, QueryType.A);
        foreach (DnsResourceRecord? answer in result.Answers) {
            resolvedAdresses.Add(answer);

        }
        return resolvedAdresses;
    }
}
