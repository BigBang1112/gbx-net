using System.Net;
using System.Net.Sockets;

namespace GbxExplorerOld.Client.Extensions;

internal static class IPAddressExtensions
{
    // Special-purpose ranges from the IANA IPv4 and IPv6 registries:
    // https://www.iana.org/assignments/iana-ipv4-special-registry/
    // https://www.iana.org/assignments/iana-ipv6-special-registry/
    private static readonly IPNetwork[] nonPublicIPv4Networks =
    [
        IPNetwork.Parse("0.0.0.0/8"),
        IPNetwork.Parse("10.0.0.0/8"),
        IPNetwork.Parse("100.64.0.0/10"),
        IPNetwork.Parse("127.0.0.0/8"),
        IPNetwork.Parse("169.254.0.0/16"),
        IPNetwork.Parse("172.16.0.0/12"),
        IPNetwork.Parse("192.0.0.0/24"),
        IPNetwork.Parse("192.0.2.0/24"),
        IPNetwork.Parse("192.88.99.0/24"),
        IPNetwork.Parse("192.168.0.0/16"),
        IPNetwork.Parse("198.18.0.0/15"),
        IPNetwork.Parse("198.51.100.0/24"),
        IPNetwork.Parse("203.0.113.0/24"),
        IPNetwork.Parse("224.0.0.0/4"),
        IPNetwork.Parse("240.0.0.0/4")
    ];

    private static readonly IPNetwork globalIPv6Network = IPNetwork.Parse("2000::/3");
    private static readonly IPNetwork translatedIPv4Network = IPNetwork.Parse("64:ff9b::/96");

    private static readonly IPNetwork[] nonPublicIPv6Networks =
    [
        IPNetwork.Parse("2001::/23"),
        IPNetwork.Parse("2001:db8::/32"),
        IPNetwork.Parse("3fff::/20")
    ];

    private static readonly IPNetwork[] publicIPv6SpecialNetworks =
    [
        IPNetwork.Parse("2001::/32"), // Teredo can contain a public IPv4 address.
        IPNetwork.Parse("2001:1::1/128"),
        IPNetwork.Parse("2001:1::2/128"),
        IPNetwork.Parse("2001:1::3/128"),
        IPNetwork.Parse("2001:3::/32"),
        IPNetwork.Parse("2001:4:112::/48"),
        IPNetwork.Parse("2001:20::/28"),
        IPNetwork.Parse("2001:30::/28")
    ];

    public static bool IsPublic(this IPAddress address)
    {
        if (address.IsIPv4MappedToIPv6 || translatedIPv4Network.Contains(address))
        {
            address = address.MapToIPv4();
        }

        if (address.AddressFamily == AddressFamily.InterNetwork)
        {
            var bytes = address.GetAddressBytes();

            // Public anycast exceptions within the otherwise reserved 192.0.0.0/24.
            if (bytes is [192, 0, 0, 9 or 10])
            {
                return true;
            }

            return !nonPublicIPv4Networks.Any(network => network.Contains(address));
        }

        return address.AddressFamily == AddressFamily.InterNetworkV6
            && globalIPv6Network.Contains(address)
            && (publicIPv6SpecialNetworks.Any(network => network.Contains(address))
                || !nonPublicIPv6Networks.Any(network => network.Contains(address)));
    }
}
