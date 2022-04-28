namespace Common.Mail;

public interface IMailAppSettings {
  MailAddress DefaultFrom { get; }
  MailAddress DefaultTo { get; }
  MailHost Host { get; }
}

public class MailAppSettings : IMailAppSettings {
  public MailAddress DefaultFrom { get; set; }
  public MailAddress DefaultTo { get; set; }
  public MailHost Host { get; set; }
}
