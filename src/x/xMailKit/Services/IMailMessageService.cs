using MailKit.Net.Smtp;
using MimeKit;

namespace MailKit.Services {
  public interface IMailMessageService {
    MailboxAddress FromMailAddress { get; }
    IMailKitAppSettings Settings { get; }
  }



  public static class IMailMessageServiceExtensions {

    public static async Task SendAsync(this IMailMessageService service, string to, string subject, string message) {
      var emailMessage = new MimeMessage();
      emailMessage.To.Add(MailboxAddress.Parse(to));
      emailMessage.Subject = subject;
      emailMessage.From.Add(service.FromMailAddress);
      emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };
      using (var client = new SmtpClient()) {
        try {
          await client.ConnectAsync(service.Settings.Host, service.Settings.Port);
          await client.AuthenticateAsync(service.Settings.UserName, service.Settings.Password);
          await client.SendAsync(emailMessage);
          //} catch (InvalidApiRequestException ex) {
          //  System.Diagnostics.Debug.WriteLine(ex.Errors.ToList().Aggregate((allErrors, error) => allErrors += ", " + error));
        } catch (Exception ex) {
          System.Diagnostics.Debug.WriteLine(ex.Message);
        }
      }
    }

  }
}