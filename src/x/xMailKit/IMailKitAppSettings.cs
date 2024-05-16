using MailKit.Net.Smtp;
using MimeKit;

namespace MailKit;

public interface IMailKitAppSettings {
  MailboxAddressAppSettings DefaultFrom { get; set; }
  MailboxAddressAppSettings DefaultTo { get; set; }
  //[Required]
  string Host { get; set; }
  int Port { get; set; }
  bool UseSsl { get; set; }
  string? UserName { get; set; }
  string? Password { get; set; }
}

public static class IMailKitAppSettingsExtensions {

  public static MimeMessage ApplySettings(this IMailKitAppSettings settings, MimeMessage message ) {
    if (message.From.Count < 1) {
      message.From.Add(settings.DefaultFrom);
    }
    if ((message.To.Count + message.Cc.Count + message.Bcc.Count) < 1) {
      message.To.Add(settings.DefaultTo);
    }
    return message;
  }

  public static MimeMessage GetMimeMessage(this IMailKitAppSettings settings
    , string subject
    , string body
    , List<MailboxAddress>? to = null
    , MailboxAddress? from = null
    , MimeKit.Text.TextFormat textFormat = MimeKit.Text.TextFormat.Html) {
    var message = settings.ApplySettings(new MimeMessage());
    if (from != null) {
      message.From.Add(from);
    }
    if (to?.Count > 0) {
      message.To.AddRange(to);
    }
    settings.ApplySettings(message);
    message.Subject = subject;
    message.Body = new TextPart(textFormat) { Text = body };
    return message;
  }

  public static void SmtpClientConnectionInfo(this IMailKitAppSettings settings) {
    using (var client = new SmtpClient()) {
      client.Connect(settings.Host, settings.Port, settings.UseSsl);

      Console.WriteLine($"Negotiated the following SSL options with {settings.Host}:");
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

  public static void SendUsingSmptClient(this IMailKitAppSettings settings, params MimeMessage[] messages) {
    //using (var client = new SmtpClient(new ProtocolLogger("smtp.log"))) {
    using (var client = new SmtpClient()) {
      try {
        client.Connect(settings.Host, settings.Port, settings.UseSsl);
        //client.AuthenticationMechanisms.Remove("XOAUTH2");
        if (settings.UserName is not null) {
          client.Authenticate(settings.UserName, settings.Password);
        }
        foreach (var message in messages) {
          settings.ApplySettings(message);
          client.Send(message);
        }
      } catch (Exception ex) {
        System.Diagnostics.Debug.WriteLine(ex.Message);
        throw;
      } finally {
        client.Disconnect(true);
        client.Dispose();
      }
    }
  }

  public async static Task SendUsingSmptClientAsync(this IMailKitAppSettings settings, params MimeMessage[] messages) {
    using (var client = new SmtpClient()) {
      try {
        await client.ConnectAsync(settings.Host, settings.Port, settings.UseSsl);
        if (settings.UserName is not null) {
          //client.AuthenticationMechanisms.Remove("XOAUTH2");
          await client.AuthenticateAsync(settings.UserName, settings.Password);
        }
        foreach (var message in messages) {
          settings.ApplySettings(message);
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

  public static void SendUsingSmptClient(this IMailKitAppSettings settings , MimeMessage message ) => settings.SendUsingSmptClient(message);
  public static void SendUsingSmptClient(this IMailKitAppSettings settings, IEnumerable<MimeMessage> messages ) => settings.SendUsingSmptClient(messages.ToArray());
  public async static Task SendUsingSmptClientAsync(this IMailKitAppSettings settings, MimeMessage message) => await settings.SendUsingSmptClientAsync(message);
  public async static Task SendUsingSmptClientAsync(this IMailKitAppSettings settings, IEnumerable<MimeMessage> messages) => await settings.SendUsingSmptClientAsync(messages.ToArray());


}
