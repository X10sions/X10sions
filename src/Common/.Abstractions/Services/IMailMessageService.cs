using System;
using System.Threading.Tasks;

namespace Common.Services {
  public interface IMailMessageService {
    MailAddress FromMailAddress { get; }
  }

  public static class IMailMessageServiceExtensions {

    public static async Task xSendAsync(this IMailMessageService service, string to, string subject, string message) {
      var emailMessage = new MailMessage();
      emailMessage.To.Add(to);
      emailMessage.Subject = subject;
      emailMessage.From = service.FromMailAddress;
      emailMessage.Body = message;
      emailMessage.IsBodyHtml = true;
      using (var client = new SmtpClient()) {
        try {
          await client.SendMailAsync(emailMessage);
          //} catch (InvalidApiRequestException ex) {
          //  System.Diagnostics.Debug.WriteLine(ex.Errors.ToList().Aggregate((allErrors, error) => allErrors += ", " + error));
        } catch (Exception ex) {
          System.Diagnostics.Debug.WriteLine(ex.Message);
        }
      }
    }

  }
}