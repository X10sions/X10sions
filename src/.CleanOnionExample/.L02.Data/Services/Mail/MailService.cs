using Common.Exceptions;
using Common.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CleanOnionExample.Services.Mail;

public class MailService : IMailService {
  public MailAppSettings _mailSettings { get; }
  public ILogger<MailService> _logger { get; }

  public MailService(IOptionsMonitor<MailAppSettings> mailSettings, ILogger<MailService> logger) {
    _mailSettings = mailSettings.CurrentValue;
    _logger = logger;
  }
  public async Task SendAsync(MailRequest mailRequest) {
    try {
      // create message
      var email = new MimeMessage();
      email.Sender = MailboxAddress.Parse(mailRequest.From.Address ?? _mailSettings.DefaultFrom.Address);
      email.To.Add(MailboxAddress.Parse(mailRequest.To.ToString()));
      email.Subject = mailRequest.Subject;
      var builder = new BodyBuilder();
      builder.HtmlBody = mailRequest.Body;
      email.Body = builder.ToMessageBody();
      using var smtp = new SmtpClient();
      smtp.Connect(_mailSettings.Host.NameOrIpAddress, _mailSettings.Host.Port, SecureSocketOptions.StartTls);
      smtp.Authenticate(_mailSettings.Host.UserName, _mailSettings.Host.Password);
      await smtp.SendAsync(email);
      smtp.Disconnect(true);
    } catch (System.Exception ex) {
      _logger.LogError(ex.Message, ex);
      throw new ApiException(ex.Message);
    }
  }

}
