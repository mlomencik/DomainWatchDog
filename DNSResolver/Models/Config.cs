using System.Net;

namespace DNSResolver.Models;

public class Config
{
    public DetectionConfig? DetectionConfig { get; set; }

    public List<PrivateIpRange>? PrivateIpRanges { get; set; }
}

public class DetectionConfig
{
    public int TtlLimit { get; set; }
}

public class PrivateIpRange
{
    public IPAddress? Start { get; set; }
    public IPAddress? End { get; set; }
}
