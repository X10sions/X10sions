namespace Microsoft.Office.Interop.Outlook;

public static class _Extensions {

  public static string GetEmailAddress(this MailItem oMailItem) => oMailItem.SenderEmailType == "EX" ? oMailItem.Sender.GetExchangeUser().PrimarySmtpAddress : oMailItem.SenderEmailAddress;

  public static MAPIFolder GetFolder(this Application outlookApp, OlDefaultFolders folderType, string? sharedEmailAddress = null) {
    if (sharedEmailAddress == null) {
      return outlookApp.Session.GetDefaultFolder(folderType);
    } else {
      var recip = GetOutlookRecipient(outlookApp, sharedEmailAddress);
      return outlookApp.Session.GetSharedDefaultFolder(recip, folderType);
    }
  }

  public static Recipient GetOutlookRecipient(this Application outlookApp, string sharedEmailAddress) {
    var ns = outlookApp.GetNamespace("MAPI");
    var recip = ns.CreateRecipient(sharedEmailAddress);
    recip.Resolve();
    return recip;
  }

}
