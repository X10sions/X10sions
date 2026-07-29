using MailKit.Security;

namespace MailKit.Net.Smtp;

public static class SmtpClientExtensions {
  public const string SmtpHost = "smtp.office365.com";
  public const int SmtpPort = 587;

  public static void Connect(this SmtpClient client) => client.Connect(SmtpHost, SmtpPort, SecureSocketOptions.StartTls);

}
