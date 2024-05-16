namespace MimeKit;
public static class InternetAddressExtensions {
  public static IList<InternetAddress> AddMailboxAddress(this IList<InternetAddress> list, string address) => list.AddMailboxAddress(string.Empty, address);

  public static IList<InternetAddress> AddMailboxAddress(this IList<InternetAddress> list, string displayName, string address) {
    list.Add(new MailboxAddress(displayName, address));
    return list;
  }

}
