using DNSResolver.Models;
using DomainWatchDog.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DomainWatchDog;

internal static class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0 || args[0] == "--help") {
            Console.WriteLine("Usage: DomainWatchDog --dns <DNS server> --domain <domain> --file <file location> --interval <refresh interval> [--rsyslog <ip address>]");
            return;
        }

        // Parse the command line arguments
        string dnsServer = GetArgumentValue(args, "--dns")!;
        string domain = GetArgumentValue(args, "--domain")!;
        string fileLocation = GetArgumentValue(args, "--file")!;
        int refreshInterval = int.Parse(GetArgumentValue(args, "--interval")!);

        string rsyslogServer = GetArgumentValue(args, "--rsyslog")!;

        // Create an instance of the AppData class
        AppData appData = new(dnsServer, domain, rsyslogServer, fileLocation, refreshInterval);

        // Load detection configuration from the specified file and set the private IP ranges
        Config config = LoadDetectionConfiguration(appData.AddressFilePath);
        appData.PrivateIPv4Ranges = config.PrivateIpRanges;
        appData.A_TTL_LIMIT = config.DetectionConfig!.TtlLimit;

        // Start the DomainWatchDog application
        var app = new DwdApp(appData, new(appData.RSyslogServer));
        app.Start();
    }

    /// <summary>
    /// Retrieves the value of the specified argument from the command line arguments.
    /// </summary>
    /// <param name="args"></param>
    /// <param name="argumentName"></param>
    /// <returns></returns>
    private static string? GetArgumentValue(string[] args, string argumentName)
    {
        for (int i = 0; i < args.Length - 1; i++) {
            if (args[i] == argumentName) {
                return args[i + 1];
            }
        }
        return null;
    }

    /// <summary>
    /// Loads the detection configuration from the specified file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private static Config LoadDetectionConfiguration(string filePath)
    {
        IDeserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance)
                .Build();

        using (StreamReader reader = new(filePath)) {
            return deserializer.Deserialize<Config>(reader);
        }
    }
}
