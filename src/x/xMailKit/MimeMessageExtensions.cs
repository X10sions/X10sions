using MailKit;
using MimeKit;

namespace MimeKit {
  public static class MimeMessageExtensions {

    public static void SendSmptClient(this MimeMessage message, IMailKitAppSettings settings) => settings.SendSmptClient(message);
    public async static Task SendSmptClientAsync(this MimeMessage message, IMailKitAppSettings settings) => await settings.SendSmptClientAsync(message);

    public static MimeMessage ApplySettings(this MimeMessage message, IMailKitAppSettings settings) {
      if (message.From.Count < 1) {
        message.From.Add(settings.DefaultFrom);
      }
      if ((message.To.Count + message.Cc.Count + message.Bcc.Count) < 1) {
        message.To.Add(settings.DefaultTo);
      }
      return message;
    }

  }
}
