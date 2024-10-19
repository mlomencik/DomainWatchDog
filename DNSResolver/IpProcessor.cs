using DnsClient.Protocol;
using DNSResolver.Models;
using System.Net;

namespace DNSResolver;
public static class IpProcessor
{
    /// <summary>
    /// Checks if the resolved record IP is from private IP range
    /// </summary>
    /// <param name="record"></param>
    /// <param name="privateRange"></param>
    /// <returns></returns>
    public static string CheckSuspiciousIp(DnsResourceRecord record, List<PrivateIpRange> privateRange)
    {
        // Check if the resolved IP belongs to any of the private ranges
        if (record is ARecord aRecord) {
            IPAddress ip = aRecord.Address;
            foreach (PrivateIpRange range in privateRange) {
                if (IsInRange(ip, range))
                    return $"Detected IP from private range: {ip}";
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// Checks if the resolved record TTL is less than the specified limit
    /// </summary>
    /// <param name="record"></param>
    /// <param name="ttlLimit"></param>
    /// <returns></returns>
    public static string CheckSuspiciousTtl(DnsResourceRecord record, int ttlLimit)
    {
        if (record.TimeToLive < ttlLimit) {
            return $"Detected suspicious TTL: {record.TimeToLive}";
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
