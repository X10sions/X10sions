namespace MailKit;

public interface IHaveSmtpAppSettings {
  ISmtpAppSettings Smtp { get; set; }
}

public interface ISmtpAppSettings {
  string Host { get; set; }
  int Port { get; set; }
  string? UserName { get; set; }
  string? Password { get; set; }
  bool UseSsl { get; set; }
}

