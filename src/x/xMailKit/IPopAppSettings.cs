namespace MailKit {
  public interface IPopAppSettings {
    string? Password { get; set; }
    int Port { get; set; }
    string? Host { get; set; }
    string? Username { get; set; }
  }
}