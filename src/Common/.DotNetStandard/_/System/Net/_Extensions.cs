using Common.Mail;

namespace System.Net.Mail;
public static class _Extensions {

  public static MailAddress AsMailAddress(this IMailAddress value) => value.DisplayName is null ? new MailAddress(value.Address) : new MailAddress(value.Address, value.DisplayName);

  public static MailMessage ApplySettings(this MailMessage message, IMailAppSettings settings) {
    if (string.IsNullOrWhiteSpace(message.From?.Address)) {
      message.From = settings.DefaultFrom.AsMailAddress();
    }
    if ((message.To.Count + message.CC.Count + message.Bcc.Count) < 1) {
      message.To.Add(settings.DefaultTo.AsMailAddress());
    }
    return message;
  }

  public static void SendSmptClient(this IMailAppSettings settings, MailMessage[] messages) {
    //using (var client = new SmtpClient(new ProtocolLogger("smtp.log"))) {
    using (var client = new SmtpClient(settings.Host.NameOrIpAddress, settings.Host.Port)) {
      client.EnableSsl = settings.Host.UseSsl;
      if (settings.Host.UserName is not null)
        client.Credentials = new NetworkCredential(settings.Host.UserName, settings.Host.Password);
      try {
        foreach (var message in messages) {
          client.Send(message.ApplySettings(settings));
        }
      } catch (Exception ex) {
        Diagnostics.Debug.WriteLine(ex.Message);
        throw ex;
      } finally {
        client.Dispose();
      }
    }
  }

}
