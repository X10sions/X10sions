using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanOnionExample.Services.Mail;

public class MailService : IEmailService {
  public MailSettings _mailSettings { get; }
  public ILogger<MailService> _logger { get; }

  public MailService(IOptionsMonitor<MailSettings> mailSettings, ILogger<MailService> logger) {
    _mailSettings = mailSettings.CurrentValue;
    _logger = logger;
  }
  public async Task SendEmailAsync(MailRequest mailRequest) {
    try {
      // create message
      var email = new MimeMessage();
      email.Sender = MailboxAddress.Parse(mailRequest.From ?? _mailSettings.DefaultFrom);
      email.To.Add(MailboxAddress.Parse(mailRequest.To));
      email.Subject = mailRequest.Subject;
      var builder = new BodyBuilder();
      builder.HtmlBody = mailRequest.Body;
      email.Body = builder.ToMessageBody();
      using var smtp = new SmtpClient();
      smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
      smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
      await smtp.SendAsync(email);
      smtp.Disconnect(true);
    } catch (System.Exception ex) {
      _logger.LogError(ex.Message, ex);
      throw new ApiException(ex.Message);
    }
  }

}
