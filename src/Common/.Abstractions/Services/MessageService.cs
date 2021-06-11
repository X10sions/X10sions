using System;
using System.Net.Mail;

namespace Common.Services {
  public class MessageService : IMessageService {

    public MessageService(string filePath, string fromAddress, string fromDisplayName)
      : this(filePath, new MailAddress(fromAddress, fromDisplayName)) { }

    public MessageService(string filePath, MailAddress fromMailAddress) {
      FilePath = filePath;
      FromMailAddress = fromMailAddress;
    }

    public string FilePath { get; } = "\\Logs\\emails.log";
    public MailAddress FromMailAddress { get; }

    public string FormatMessage(string to, string subject, string message)
      => $"Date:{DateTime.Now}\nTo: {to}\nSubject: {subject}\nMessage: {message}\n\n";

  }
}