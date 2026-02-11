using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace QRCodeExtension.Helpers;

internal static class PlaceholderExpander
{
    private static readonly HttpClient HttpClient = new();
    private static readonly Lazy<string> LocalIp = new(ResolveLocalIp);
    private static readonly Lazy<string> PublicIp = new(ResolvePublicIp);

    public static string Expand(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var now = DateTime.Now;
        var replacements = new (string token, string value)[]
        {
            ("{ip_public}", PublicIp.Value),
            ("{ip_local}", LocalIp.Value),
            ("{ip}", LocalIp.Value),
            ("{date}", now.ToString("d")),
            ("{time}", now.ToString("T")),
        };

        var result = input;
        foreach (var replacement in replacements)
        {
            result = result.Replace(replacement.token, replacement.value, StringComparison.Ordinal);
        }

        return result;
    }

    private static string ResolveLocalIp()
    {
        try
        {
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.OperationalStatus != OperationalStatus.Up)
                {
                    continue;
                }

                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                {
                    continue;
                }

                foreach (var addressInfo in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    if (addressInfo.Address.AddressFamily != AddressFamily.InterNetwork)
                    {
                        continue;
                    }

                    var address = addressInfo.Address;
                    if (IPAddress.IsLoopback(address))
                    {
                        continue;
                    }

                    var bytes = address.GetAddressBytes();
                    if (bytes.Length == 4 && bytes[0] == 169 && bytes[1] == 254)
                    {
                        continue;
                    }

                    return address.ToString();
                }
            }
        }
        catch
        {
        }

        return string.Empty;
    }

    private static string ResolvePublicIp()
    {
        try
        {
            using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var response = HttpClient.GetStringAsync("https://api.ipify.org", cancellation.Token).GetAwaiter().GetResult();
            return response.Trim();
        }
        catch
        {
            return string.Empty;
        }
    }
}
