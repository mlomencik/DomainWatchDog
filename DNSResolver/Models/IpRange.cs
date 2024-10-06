using System.Net;

namespace DNSResolver.Models;

public class IPRange
{
    required public IPAddress Start { get; set; }
    required public IPAddress End { get; set; }
}
