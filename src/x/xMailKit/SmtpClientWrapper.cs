using MailKit.Net.Smtp;
using MimeKit;

namespace MailKit;
[Obsolete("DO NOT USE: JSUT FOR TESTING")]
public class SmtpClientWrapper : IDisposable {
  public SmtpClientWrapper(IMailKitAppSettings mailSettings, string logFileName = "SmtpClient.log") {
    this.mailSettings = mailSettings;
    try {
      client = new SmtpClient(new ProtocolLogger(logFileName));
      client.Connect(mailSettings.Host, mailSettings.Port, mailSettings.UseSsl);
      //client.AuthenticationMechanisms.Remove("XOAUTH2");
      if (mailSettings.UserName is not null) {
        client.Authenticate(mailSettings.UserName, mailSettings.Password);
      }
    } catch (Exception ex) {
      System.Diagnostics.Debug.WriteLine(ex.Message);
      throw;
    } finally {
      Dispose();
    }
  }

  IMailKitAppSettings mailSettings;
  ISmtpClient client = new SmtpClient();

  public string Send(params MimeMessage[] messages) {
    var result = string.Empty;
    foreach (var message in messages) {
      try {
        mailSettings.ApplySettings(message);
        result = client.Send(message);
      } catch (Exception ex) {
        System.Diagnostics.Debug.WriteLine(ex.Message);
        throw;
      } finally {
        Dispose();
      }
    }
    return result;
  }

  public async Task<string> SendAsync(params MimeMessage[] messages) {
    var result = string.Empty;
    foreach (var message in messages) {
      try {
        mailSettings.ApplySettings(message);
        result = await client.SendAsync(message);
      } catch (Exception ex) {
        System.Diagnostics.Debug.WriteLine(ex.Message);
        throw;
      } finally {
        Dispose();
      }
    }
    return result;
  }

  public void Dispose() {
    client.Disconnect(true);
    client.Dispose();
  }
}
