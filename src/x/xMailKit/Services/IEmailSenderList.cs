namespace MailKit.Services {
  public interface IEmailSenderList {
    IList<MailKitMessage> SentEmails { get; }
    Task AddEmail(MailKitMessage mailMessage);
  }
}