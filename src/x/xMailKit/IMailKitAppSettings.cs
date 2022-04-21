using MailKit.Net.Smtp;
using MimeKit;

namespace MailKit;

public interface IHaveMailKitAppSettings {
  IMailKitAppSettings MailKit { get; set; }
}

public interface IMailKitAppSettings : IHaveSmtpAppSettings {
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

  public static void SmtpClientConnectionInfo(this IMailKitAppSettings settings) {
    using (var client = new SmtpClient()) {
      client.Connect(settings.Smtp.Host, settings.Smtp.Port, settings.Smtp.UseSsl);

      Console.WriteLine($"Negotiated the following SSL options with {settings.Smtp.Host}:");
      Console.WriteLine($"        Protocol Version: {client.SslProtocol}");
      Console.WriteLine($"        Cipher Algorithm: {client.SslCipherAlgorithm}");
      Console.WriteLine($"         Cipher Strength: {client.SslCipherStrength}");
      Console.WriteLine($"          Hash Algorithm: {client.SslHashAlgorithm}");
      Console.WriteLine($"           Hash Strength: {client.SslHashStrength}");
      Console.WriteLine($"  Key-Exchange Algorithm: {client.SslKeyExchangeAlgorithm}");
      Console.WriteLine($"   Key-Exchange Strength: {client.SslKeyExchangeStrength}");

      // Example Log:
      //
      // Negotiated the following SSL options with smtp.gmail.com:
      //         Protocol Version: Tls12
      //         Cipher Algorithm: Aes128
      //          Cipher Strength: 128
      //           Hash Algorithm: Sha256
      //            Hash Strength: 0
      //   Key-Exchange Algorithm: 44550
      //    Key-Exchange Strength: 255

      client.Disconnect(true);
    }
  }

  public static void SendSmptClient(this IMailKitAppSettings settings, params MimeMessage[] messages) {
    //using (var client = new SmtpClient(new ProtocolLogger("smtp.log"))) {
    using (var client = new SmtpClient()) {
      try {
        client.Connect(settings.Smtp.Host, settings.Smtp.Port, settings.Smtp.UseSsl);
        //client.AuthenticationMechanisms.Remove("XOAUTH2");
        if (settings.Smtp.UserName != null) {
          client.Authenticate(settings.Smtp.UserName, settings.Smtp.Password);
        }
        foreach (var message in messages) {
          message.ApplySettings(settings);
          client.Send(message);
        }
      } catch (Exception ex) {
        System.Diagnostics.Debug.WriteLine(ex.Message);
        throw ex;
      } finally {
        client.Disconnect(true);
        client.Dispose();
      }
    }
  }

  public async static Task SendSmptClientAsync(this IMailKitAppSettings settings, params MimeMessage[] messages) {
    using (var client = new SmtpClient()) {
      try {
        await client.ConnectAsync(settings.Smtp.Host, settings.Smtp.Port, settings.Smtp.UseSsl);
        if (settings.Smtp.UserName != null) {
          //client.AuthenticationMechanisms.Remove("XOAUTH2");
          await client.AuthenticateAsync(settings.Smtp.UserName, settings.Smtp.Password);
        }
        foreach (var message in messages) {
          message.ApplySettings(settings);
          await client.SendAsync(message);
        }
      } catch (Exception ex) {
        System.Diagnostics.Debug.WriteLine(ex.Message);
        throw ex;
      } finally {
        await client.DisconnectAsync(true);
        client.Dispose();
      }
    }
  }

}