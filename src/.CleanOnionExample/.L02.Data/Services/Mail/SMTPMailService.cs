using Common.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CleanOnionExample.Services.Mail;

public class SMTPMailService : IMailService {
  public MailAppSettings _mailSettings { get; }
  public ILogger<SMTPMailService> _logger { get; }

  public SMTPMailService(IOptions<MailAppSettings> mailSettings, ILogger<SMTPMailService> logger) {
    _mailSettings = mailSettings.Value;
    _logger = logger;
  }

  public async Task SendAsync(MailRequest request) {
    try {
      var email = new MimeMessage();
      email.Sender = MailboxAddress.Parse(request?.From?.Address ?? _mailSettings.DefaultFrom.Address);
      email.To.Add(MailboxAddress.Parse(request.To.ToString()));
      email.Subject = request.Subject;
      var builder = new BodyBuilder();
      builder.HtmlBody = request.Body;
      email.Body = builder.ToMessageBody();
      using var smtp = new SmtpClient();
      smtp.Connect(_mailSettings.Host.NameOrIpAddress, _mailSettings.Host.Port, SecureSocketOptions.StartTls);
      smtp.Authenticate(_mailSettings.Host.UserName, _mailSettings.Host.Password);
      await smtp.SendAsync(email);
      smtp.Disconnect(true);
    } catch (Exception ex) {
      _logger.LogError(ex.Message, ex);
    }
  }
}
