using DnsClient.Protocol;
using DNSResolver.Models;
using System.Net;

namespace DNSResolver;
public static class IpProcessor
{
    /// <summary>
    /// Checks if the resolved IP address is suspicious (private IP or low TTL).
    /// </summary>
    /// <param name="record"></param>
    /// <param name="privateRanges"></param>
    /// <returns></returns>
    public static string CheckSuspiciousRecord(DnsResourceRecord record, List<PrivateIpRange> privateRanges)
    {
        // Check if the resolved IP belongs to any of the private ranges
        if (record is ARecord aRecord) {
            IPAddress ip = aRecord.Address;
            foreach (PrivateIpRange range in privateRanges) {
                if (IsInRange(ip, range))
                    return "Private IP detected";
            }
            if (aRecord.TimeToLive < 25) {
                return "Suspicious TTL detected";
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// Checks if the specified IP address is within the specified range.
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public static bool IsInRange(IPAddress ip, PrivateIpRange range)
    {
        byte[] ipBytes = ip.GetAddressBytes();
        byte[] lowerBytes = range.Start!.GetAddressBytes();
        byte[] upperBytes = range.End!.GetAddressBytes();

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
