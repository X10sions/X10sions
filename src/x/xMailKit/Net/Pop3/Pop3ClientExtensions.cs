using MailKit.Net.Imap;
using MailKit.Security;

namespace MailKit.Net.Pop3;

public static class Pop3ClientExtensions {
  public const string Pop3Host = ImapClientExtensions.ImapHost;
  public const int Pop3Port = 995;

  public static void Connect(this Pop3Client client) => client.Connect(Pop3Host, Pop3Port, SecureSocketOptions.Auto);

}
