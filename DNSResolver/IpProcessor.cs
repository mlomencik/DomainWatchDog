using DNSResolver.Models;
using System.Net;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DNSResolver;
public static class IpProcessor
{
    public static List<IPRange> LoadPrivateIpRanges(string filePath)
    {
        IDeserializer deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        using (StreamReader reader = new StreamReader(filePath)) {
            return deserializer.Deserialize<List<IPRange>>(reader);
        }
    }

    public static bool IsPrivateIp(IPAddress ip, List<IPRange> privateRanges)
    {
        foreach (IPRange range in privateRanges) {
            if (IsInRange(ip, range))
                return true;
        }
        return false;
    }

    public static bool IsInRange(IPAddress ip, IPRange range)
    {
        byte[] ipBytes = ip.GetAddressBytes();
        byte[] lowerBytes = range.Start.GetAddressBytes();
        byte[] upperBytes = range.End.GetAddressBytes();

        bool lowerBoundary = true, upperBoundary = true;

        for (int i = 0; i < ipBytes.Length && (lowerBoundary || upperBoundary); i++) {
            if (lowerBoundary && ipBytes[i] < lowerBytes[i]) return false;
            if (upperBoundary && ipBytes[i] > upperBytes[i]) return false;

            lowerBoundary &= ipBytes[i] == lowerBytes[i];
            upperBoundary &= ipBytes[i] == upperBytes[i];
        }
        return true;
    }
}
