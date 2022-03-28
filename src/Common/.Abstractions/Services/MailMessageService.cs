namespace Common.Services {
  public class MailMessageService : IMailMessageService {
    public MailMessageService(string fromAddress, string fromDisplayName)
      : this(new MailAddress(fromAddress, fromDisplayName)) { }

    public MailMessageService(MailAddress fromMailAddress) {
      FromMailAddress = fromMailAddress;
    }

    public MailAddress FromMailAddress { get; }

  }
}