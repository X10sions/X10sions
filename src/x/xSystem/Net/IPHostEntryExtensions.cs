namespace System.Net {
  public static class IPHostEntryExtensions {

    public static string ComputerDomainName(this IPHostEntry ipHostEntry) => ipHostEntry.HostName.Substring(ipHostEntry.HostName.IndexOf(".", StringComparison.Ordinal) + 1);
    public static string ComputerName(this IPHostEntry ipHostEntry) => ipHostEntry.HostName.Substring(0, ipHostEntry.HostName.IndexOf(".", StringComparison.Ordinal));
    public static string ComputerPhysicalPath(this IPHostEntry ipHostEntry) => $@"\\{ipHostEntry.ComputerName()}\";

    public static bool IsLocal(this IPHostEntry ipHostEntry) {
      var localIps = Dns.GetHostEntry(string.Empty).AddressList;
      return (from ip in ipHostEntry.AddressList
              where IPAddress.IsLoopback(ip) || localIps.Any(localIp => localIp.Equals(ip))
              select ip).FirstOrDefault() != null;
    }

    public static IPAddress IPAddressV4(this IPHostEntry ipHostEntry) => ipHostEntry.AddressList.FirstOrDefault(x => x.AddressFamily == Sockets.AddressFamily.InterNetwork);
    public static IPAddress IPAddressV6(this IPHostEntry ipHostEntry) => ipHostEntry.AddressList.FirstOrDefault(x => x.AddressFamily == Sockets.AddressFamily.InterNetworkV6);

    public static string IPAddressPhysicalPath(this IPHostEntry ipHostEntry) => $@"\\{ipHostEntry.IPAddressV4()}\";
    public static string PhysicalPath(this IPHostEntry ipHostEntry) => $@"\\{ipHostEntry.HostName}\";

  }
}
