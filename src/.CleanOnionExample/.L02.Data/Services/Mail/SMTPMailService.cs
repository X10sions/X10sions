using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanOnionExample.Services.Mail;

public class SMTPMailService : IMailService {
  public MailSettings _mailSettings { get; }
  public ILogger<SMTPMailService> _logger { get; }

  public SMTPMailService(IOptions<MailSettings> mailSettings, ILogger<SMTPMailService> logger) {
    _mailSettings = mailSettings.Value;
    _logger = logger;
  }

  public async Task SendAsync(MailRequest request) {
    try {
      var email = new MimeMessage();
      email.Sender = MailboxAddress.Parse(request.From ?? _mailSettings.DefaultFrom);
      email.To.Add(MailboxAddress.Parse(request.To));
      email.Subject = request.Subject;
      var builder = new BodyBuilder();
      builder.HtmlBody = request.Body;
      email.Body = builder.ToMessageBody();
      using var smtp = new SmtpClient();
      smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
      smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
      await smtp.SendAsync(email);
      smtp.Disconnect(true);
    } catch (System.Exception ex) {
      _logger.LogError(ex.Message, ex);
    }
  }
}
