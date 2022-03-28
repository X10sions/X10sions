using MimeKit;

namespace MailKit.Services {
  public class MailMessageService : IMailMessageService {
    public MailMessageService(string fromAddress, string fromDisplayName)
      : this(new MailboxAddress(fromAddress, fromDisplayName)) { }

    public MailMessageService(MailboxAddress from) : this(new MailKitAppSettings { From = from }) { }
    public MailMessageService(IOptions<IMailKitAppSettings> mailSettingsOptions) {
      Settings = mailSettingsOptions.Value;
      FromMailAddress = fromMailAddress;
    }

    public MailMessageService(IMailKitAppSettings settings) {
      Settings = settings;
    }

    public MailboxAddress FromMailAddress { get; }

    public IMailKitAppSettings Settings { get; }
  }
}