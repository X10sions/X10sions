using System.Linq;
using System.Net.Sockets;

namespace System.Net {
  public static class IpAddressExtensions {

    public static IPHostEntry GetIPHostEntry(this IPAddress ipAddress) => Dns.GetHostEntry(ipAddress);

    public static bool IsLocal(this IPAddress ipAddress) {
      var addressList = Dns.GetHostEntry(string.Empty).AddressList;
      return IPAddress.IsLoopback(ipAddress) || addressList.Any(localIp => localIp.Equals(ipAddress));
    }

    public static string PhysicalPath(this IPAddress ipAddress) => $@"\\{ipAddress}\";
    public static string UncPath(this IPAddress ipAddress) => ipAddress.PhysicalPath();

    public static IPAddress ToV4(this IPAddress ipAddress) => ipAddress.AddressFamily switch {
      AddressFamily.InterNetwork => ipAddress,
      AddressFamily.InterNetworkV6 => ipAddress.MapToIPv4(),
      _ => throw new NotImplementedException(ipAddress.AddressFamily.ToString())
    };

    public static IPAddress ToV6(this IPAddress ipAddress) => ipAddress.AddressFamily switch {
      AddressFamily.InterNetwork => ipAddress.MapToIPv6(),
      AddressFamily.InterNetworkV6 => ipAddress,
      _ => throw new NotImplementedException(ipAddress.AddressFamily.ToString())
    };


  }
}