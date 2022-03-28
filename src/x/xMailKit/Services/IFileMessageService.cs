using System.IO;
using System.Threading.Tasks;

namespace MailKit.Services {

  public interface IFileMessageService {
    string FilePath { get; }
    string FormatMessage(string to, string subject, string message);
  }

  public static class IFileMessageServiceExtensions {

    public static Task AppendFileAsync(this IFileMessageService service, string to, string subject, string message) {
      var emailMessage = service.FormatMessage(to, subject, message);
      File.AppendAllText(service.FilePath, emailMessage);
      return Task.FromResult(0);
    }

  }
}