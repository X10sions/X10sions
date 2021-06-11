using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace System.Net {
  public static class DnsExtensions {

    public static IPAddress? GetIPAddress(string hostNameOrAddress, AddressFamily addressFamily = AddressFamily.InterNetwork) {
      try {
        return NetworkInterface.GetIsNetworkAvailable() ? Dns.GetHostEntry(hostNameOrAddress)?.AddressList.FirstOrDefault(ip => ip.AddressFamily == addressFamily) : null;
      } catch {
        return null;
      }
    }

    public static IPAddress? GetIPAddressV4(string hostNameOrAddress) => GetIPAddress(hostNameOrAddress, AddressFamily.InterNetwork);
    public static IPAddress? GetIPAddressV6(string hostNameOrAddress) => GetIPAddress(hostNameOrAddress, AddressFamily.InterNetworkV6);

    public static IPAddress[]? GetIPAddressList(string hostNameOrAddress) {
      try {
        return NetworkInterface.GetIsNetworkAvailable() ? Dns.GetHostAddresses(hostNameOrAddress) : null;
      } catch {
        return null;
      }
    }

    [Obsolete("Use System.Net.Dns.GetHostEntry")] public static IPHostEntry LocalHostEntry() => Dns.GetHostEntry(string.Empty);

  }
}