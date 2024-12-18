# DomainWatchDog

DomainWatchDog is a .NET 8 application designed to monitor domain names and DNS servers. 
It allows users to specify a DNS server, domain, configuration file, refresh interval, and optionally an rsyslog server for logging.

# Usage

```console
DomainWatchDog --dns <DNS server> --domain <domain> --file <file location> --interval <refresh interval> [--rsyslog <ip address>]
```

## Command Line Arguments
-	**--dns** \<DNS server\>: Specifies the DNS server to use.
-	**--domain** \<domain>\: Specifies the domain to monitor.
-	**--file** \<file location\>: Specifies the location of the configuration file.
-	**--interval** \<refresh interval\>: Specifies the refresh interval in seconds.
-	**--rsyslog** \<ip address\> (optional): Specifies the IP address of the rsyslog server for logging.

### Example

```console
DomainWatchDog --dns 8.8.8.8 --domain www.example.com --file config.yaml --interval 60 --rsyslog 192.168.1.100
```

## Build and run as Docker container

Build image from main project directory:

```console
docker build -t dwd .
```

Run container with arguments:

```console
docker run dwd --dns <DNS server> --domain <domain> --file "/app/detection_config.yaml" --interval <refresh interval> [--rsyslog <ip address>]
```
### Example

```console
docker run dwd --dns 1.1.1.1 --domain www.example.com --file "/app/detection_config.yaml" --interval 50 --rsyslog 192.168.1.100
```

# Configuration File

The configuration file should be in YAML format and include the following structure:

```yaml
DetectionConfig: 
  # Add detection configuration properties here
PrivateIpRanges: 
  - # Add private IP ranges here
```

### Example
```yaml
---
DetectionConfig:
  TtlLimit: 5
PrivateIpRanges:
  - Start: 10.0.0.0
    End: 10.255.255.255
  - Start: 172.16.0.0
    End: 172.31.255.255
  - Start: 192.168.0.0
    End: 192.168.255.255
```

# Dependencies

•	[YamlDotNet](https://github.com/aaubry/YamlDotNet): Used for parsing YAML configuration files.

# Building and Running

1.	Ensure you have .NET 8 installed.
2.	Clone the repository.
3.	Build the project using Visual Studio or the .NET CLI.
4.	Run the application with the appropriate command line arguments.

# License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
