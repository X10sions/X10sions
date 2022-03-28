using MimeKit;

namespace MailKit.Services {
  public class MessageService : IMessageService {

    public MessageService(string filePath, string fromAddress, string fromDisplayName)
      : this(filePath, new MailboxAddress(fromAddress, fromDisplayName)) { }

    public MessageService(string filePath, MailboxAddress fromMailAddress) {
      FilePath = filePath;
      FromMailAddress = fromMailAddress;
    }

    public string FilePath { get; } = "\\Logs\\emails.log";
    public MailboxAddress FromMailAddress { get; }

    public string FormatMessage(string to, string subject, string message)
      => $"Date:{DateTime.Now}\nTo: {to}\nSubject: {subject}\nMessage: {message}\n\n";

  }
}