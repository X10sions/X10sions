namespace Common.Mail;

public interface IMailHost {
  string NameOrIpAddress { get; }
  string? Password { get; }
  int Port { get; }
  string? UserName { get; }
  bool UseSsl { get; }
}

public class MailHost : IMailHost {
  public string NameOrIpAddress { get; set; } = string.Empty;
  public string? Password { get; set; }
  public int Port { get; set; }
  public string? UserName { get; set; }
  public bool UseSsl { get; set; }
}

//public interface ISmtpMailHost : IMailHost {
//  bool UseSsl { get; }
//}

//public class SmtpMailHost : MailHost, ISmtpMailHost {
//  public bool UseSsl { get; set; }
//}

//public interface IPopMailHost : IMailHost {}

//public class PopMailHost : MailHost, IPopMailHost {}