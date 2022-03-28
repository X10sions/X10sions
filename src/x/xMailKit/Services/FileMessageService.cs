using System;

namespace MailKit.Services {
  public class FileMessageService : IFileMessageService {
    public FileMessageService(string filePath) {
      FilePath = filePath;
    }

    public string FilePath { get; } = "\\Logs\\emails.log";

    public string FormatMessage(string to, string subject, string message)
      => $"Date:{DateTime.Now}\nTo: {to}\nSubject: {subject}\nMessage: {message}\n\n";

  }
}