using DomainWatchDog.Models;

namespace DomainWatchDog;

internal static class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0 || args[0] == "--help") {
            Console.WriteLine("Usage: DomainWatchDog --dns <DNS server> --domain <domain> --file <file location> --interval <refresh interval> [--rsyslog <ip address>]");
            return;
        }

        string dnsServer = GetArgumentValue(args, "--dns")!;
        string domain = GetArgumentValue(args, "--domain")!;
        string fileLocation = GetArgumentValue(args, "--file")!;
        int refreshInterval = int.Parse(GetArgumentValue(args, "--interval")!);

        string rsyslogServer = GetArgumentValue(args, "--rsyslog")!;

        AppData appData = new(dnsServer, domain, rsyslogServer, fileLocation, refreshInterval);

        var app = new DwdApp(appData);
        app.Start();
    }

    private static string? GetArgumentValue(string[] args, string argumentName)
    {
        for (int i = 0; i < args.Length - 1; i++) {
            if (args[i] == argumentName) {
                return args[i + 1];
            }
        }
        return null;
    }
}
