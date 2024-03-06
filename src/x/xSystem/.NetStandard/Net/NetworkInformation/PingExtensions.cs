namespace System.Net.NetworkInformation;
public static class PingExtensions {

  public static PingReply? TryGetPingReply(this Ping ping, string hostNameOrAddress, int timeout = 100) {
    try {
      return ping.Send(hostNameOrAddress, timeout);
    } catch {
      return null;
    }
  }

  public static IPAddress? GetIPAddressUsingPing(this Ping ping, string hostNameOrAddress, int timeout = 100) {
    var replay = ping.TryGetPingReply(hostNameOrAddress, timeout);
    return replay?.Status == IPStatus.Success ? replay.Address : null;
  }

}