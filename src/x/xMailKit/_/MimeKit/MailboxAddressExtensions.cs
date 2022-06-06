using Common.Mail;

namespace MimeKit;
public static class MailboxAddressExtensions {
  public static MailboxAddress AsMailboxAddress(this IMailAddress value) => value.DisplayName is null ? MailboxAddress.Parse(value.Address) : new MailboxAddress(value.DisplayName, value.Address);
}
