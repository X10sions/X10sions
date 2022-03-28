namespace MailKit.Services {
  public interface IMessageService : IFileMessageService, IMailMessageService {
  }

  public static class IMessageServiceExtensions {

    public static Task SendAndAppendFileAsync(this IMessageService service, string to, string subject, string message) {
      return service.AppendFileAsync(to, subject, message);
      //return service.SendAsync(to, subject, message);
    }

  }
}