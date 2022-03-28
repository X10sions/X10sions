using MailKit.Net.Smtp;
using MimeKit;

namespace MailKit {
  public interface IMailKitAppSettings {
    // https://code-maze.com/aspnetcore-send-email/
    // https://lukelowrey.com/dotnet-email-guide-2021/


    //IPopAppSettings Pop { get; set; }
    ISmtpAppSettings Smtp { get; set; }

    //string? ServerDomainPart { get; set; }

    //public static string UserNameEmailAddress(this IMailKitAppSettings settings, string userName) => userName + "@" + settings.ServerDomainPart;

    MailboxAddress DefaultFrom { get; set; }
    MailboxAddress DefaultTo { get; set; }

    //    string FromAddress { get; set; }
    //    string FromDisplayName { get; set; }
  }
  public static class IMailKitAppSettingsExtensions {

    public static MimeMessage GetMimeMessage(this IMailKitAppSettings settings
      , string subject
      , string body
      , List<MailboxAddress>? to = null
      , MailboxAddress? from = null
      , MimeKit.Text.TextFormat textFormat = MimeKit.Text.TextFormat.Html) {
      var message = new MimeMessage().ApplySettings(settings);
      if (from != null) {
        message.From.Add(from);
      }
      if (to?.Count > 0) {
        message.To.AddRange(to);
      }
      message.ApplySettings(settings);
      message.Subject = subject;
      message.Body = new TextPart(textFormat) { Text = body };
      return message;
    }

    public static void SendSmptClient(this IMailKitAppSettings settings, MimeMessage message) {
      message.ApplySettings(settings);
      using (var client = new SmtpClient()) {
        try {
          client.Connect(settings.Smtp.Host, settings.Smtp.Port, settings.Smtp.UseSsl);
          //client.AuthenticationMechanisms.Remove("XOAUTH2");
          if (settings.Smtp.UserName != null) {
            client.Authenticate(settings.Smtp.UserName, settings.Smtp.Password);
          }
          client.Send(message);
        } catch {
          throw;
        } finally {
          client.Disconnect(true);
          client.Dispose();
        }
      }
    }

    public async static Task SendSmptClientAsync(this IMailKitAppSettings settings, MimeMessage message) {
      message.ApplySettings(settings);
      using (var client = new SmtpClient()) {
        try {
          await client.ConnectAsync(settings.Smtp.Host, settings.Smtp.Port, settings.Smtp.UseSsl);
          if (settings.Smtp.UserName != null) {
            //client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(settings.Smtp.UserName, settings.Smtp.Password);
          }
          await client.SendAsync(message);
        } catch {
          throw;
        } finally {
          await client.DisconnectAsync(true);
          client.Dispose();
        }
      }
    }

  }
}