using MailKit.Security;

namespace MailKit.Net.Imap;

public static class ImapClientExtensions {
  public const string ImapHost = "outlook.office365.com";
  public const int ImapPort = 993;

  public static void Connect(this ImapClient client) => client.Connect(ImapHost, ImapPort, SecureSocketOptions.Auto);

  public static IMailFolder GetFolder(this ImapClient client, SpecialFolder specialFolder, string folderName) => client.Capabilities.HasFlag(ImapCapabilities.SpecialUse) ? client.GetFolder(specialFolder) : client.GetFolder(client.PersonalNamespaces[0]).GetSubfolder(folderName);
  public static IMailFolder GetFolderDeletedItems(this ImapClient client) => client.GetFolder(SpecialFolder.Trash, "Deleted Items");
  public static IMailFolder GetFolderSentItems(this ImapClient client) => client.GetFolder(SpecialFolder.Sent, "Sent Items");


}
