using MimeKit;

namespace MailKit.Services {

  public interface IEmailSender {
    Task SendEmail(MimeMessage message);
    Task SendEmail(string to, string subject, string htmlMessage);
    Task SendEmailAsync(MimeMessage message);
    Task SendEmailAsync(string to, string subject, string htmlMessage);
  }

  public class EmailSender : IEmailSender {
    public EmailSender(IMailKitAppSettings settings) {
      Settings = settings;
    }

    public IMailKitAppSettings Settings { get; }

    public Task SendEmail(MimeMessage message) => Execute(subject, message, emailtoAddress);
    public Task SendEmail(string emailtoAddress, string subject, string message) => Execute(subject, message, emailtoAddress);
    public Task SendEmailAsync(MimeMessage message) => Execute(subject, message, emailtoAddress);
    public Task SendEmailAsync(string emailtoAddress, string subject, string message) => Execute(subject, message, emailtoAddress);

    public async Task ExecuteASync(string subject, string body, string? emailToAddress = null, string? emailFromAddress = null) {
      var emailTo = string.IsNullOrWhiteSpace(emailToAddress) ? null : new List<MailboxAddress>() { MailboxAddress.Parse(emailToAddress) };
      var emailFrom = string.IsNullOrWhiteSpace(emailFromAddress) ? null : MailboxAddress.Parse(emailFromAddress);
      await ExecuteAsync(subject, body, emailTo, emailFrom);
    }

    public async Task ExecuteAsync(string subject, string body, List<MailboxAddress>? emailTo = null, MailboxAddress? emailFrom = null) {
      var message = Settings.GetMimeMessage(subject, body, emailTo, emailFrom);
      await message.SendSmptClientAsync(Settings);
    }


  }
}