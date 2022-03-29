namespace MailKit;
public interface ISmtpAppSettings {
  string Host { get; set; }
  int Port { get; set; }
  string? UserName { get; set; }
  string? Password { get; set; }
  bool UseSsl { get; set; }
}