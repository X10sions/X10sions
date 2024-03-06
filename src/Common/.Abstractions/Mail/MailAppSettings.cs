namespace Common.Mail;

public interface IMailAppSettings {
  MailAddress DefaultFrom { get; }
  MailAddress DefaultTo { get; }
  MailHost Host { get; }
}

public class MailAppSettings : IMailAppSettings {
  public MailAddress DefaultFrom { get; set; } = new();
  public MailAddress DefaultTo { get; set; } = new();
  public MailHost Host { get; set; } = new();
}
