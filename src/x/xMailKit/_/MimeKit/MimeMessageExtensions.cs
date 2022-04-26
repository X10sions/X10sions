using Common.Mail;
using MailKit;
using MimeKit;

namespace MimeKit;
public static class MimeMessageExtensions {

  public static MimeMessage From(this MimeMessage message, string address) {
    message.From.Add(MailboxAddress.Parse(address));
    return message;
  }

  public static MimeMessage From(this MimeMessage message, string name, string address) {
    message.From.Add(new MailboxAddress(name, address));
    return message;
  }

  public static MimeMessage To(this MimeMessage message, string address) {
    message.To.Add(MailboxAddress.Parse(address));
    return message;
  }

  public static MimeMessage To(this MimeMessage message, string name, string address) {
    message.To.Add(new MailboxAddress(name, address));
    return message;
  }

  public static MimeMessage Cc(this MimeMessage message, string address) {
    message.Cc.Add(MailboxAddress.Parse(address));
    return message;
  }

  public static MimeMessage Cc(this MimeMessage message, string name, string address) {
    message.Cc.Add(new MailboxAddress(name, address));
    return message;
  }

  public static MimeMessage Bcc(this MimeMessage message, string address) {
    message.Bcc.Add(MailboxAddress.Parse(address));
    return message;
  }

  public static MimeMessage Bcc(this MimeMessage message, string name, string address) {
    message.Bcc.Add(new MailboxAddress(name, address));
    return message;
  }
  public static MimeMessage Subject(this MimeMessage message, string subject) {
    message.Subject = subject;
    return message;
  }

  public static MimeMessage Body(this MimeMessage message, string body, Text.TextFormat textFormat) {
    message.Body = new TextPart(textFormat) { Text = body };
    return message;
  }

  public static MimeMessage HtmlBody(this MimeMessage message, string body) => message.Body(body, Text.TextFormat.Html);
  public static MimeMessage PlainBody(this MimeMessage message, string body) => message.Body(body, Text.TextFormat.Plain);

  public static void SendSmptClient(this MimeMessage message, IMailAppSettings settings) => settings.SendSmptClient(message);
  public static void SendSmptClient(this IEnumerable<MimeMessage> messages, IMailAppSettings settings) => settings.SendSmptClient(messages.ToArray());
  public async static Task SendSmptClientAsync(this MimeMessage message, IMailAppSettings settings) => await settings.SendSmptClientAsync(message);
  public async static Task SendSmptClientAsync(this IEnumerable<MimeMessage> messages, IMailAppSettings settings) => await settings.SendSmptClientAsync(messages.ToArray());
    
  public static MimeMessage ApplySettings(this MimeMessage message, IMailAppSettings settings) {
    if (message.From.Count < 1) {
      message.From.Add(settings.DefaultFrom.AsMailboxAddress());
    }
    if ((message.To.Count + message.Cc.Count + message.Bcc.Count) < 1) {
      message.To.Add(settings.DefaultTo.AsMailboxAddress());
    }
    return message;
  }

  //public string FilePath { get; } = "\\Logs\\emails.log";

  //public string FormatMessage(string to, string subject, string message) => $"Date:{DateTime.Now}\nTo: {to}\nSubject: {subject}\nMessage: {message}\n\n";

  //public static Task AppendFileAsync(this IFileMessageService service, string to, string subject, string message) {
  //  var emailMessage = service.FormatMessage(to, subject, message);
  //  File.AppendAllText(service.FilePath, emailMessage);
  //  return Task.FromResult(0);
  //}

  //public static void SaveToPickupDirectory(this MimeMessage message, string pickupDirectory) {
  //  // Generate a random file name to save the message to.
  //  var path = Path.Combine(pickupDirectory, Guid.NewGuid().ToString() + ".eml");
  //  Stream stream;

  //  try {
  //    // Attempt to create the new file.
  //    stream = File.Open(path, FileMode.CreateNew);
  //  } catch (IOException) {
  //    // If the file already exists, try again with a new Guid.
  //    if (File.Exists(path))
  //    // Otherwise, fail immediately since it probably means that there is
  //    // no graceful way to recover from this error.
  //    throw;
  //  }

  //  try {
  //    using (stream) {
  //      using (var filtered = new FilteredStream(stream)) {
  //        filtered.Add(new SmtpDataFilter());
  //        // Make sure to write the message in DOS (<CR><LF>) format.
  //        var options = FormatOptions.Default.Clone();
  //        options.NewLineFormat = NewLineFormat.Dos;
  //        message.WriteTo(options, filtered);
  //        filtered.Flush();
  //        return;
  //      }
  //    }
  //  } catch {
  //    File.Delete(path);
  //    throw;
  //  }
  //}

}