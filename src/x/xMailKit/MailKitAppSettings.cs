namespace MailKit;

public class MailKitAppSettings : IMailKitAppSettings {
  public MailboxAddressAppSettings DefaultFrom { get; set; } = default!;
  public MailboxAddressAppSettings DefaultTo { get; set; } = default!;
  public string Host { get; set; } = default!;
  public int Port { get; set; }
  public bool UseSsl { get; set; }
  public string? UserName { get; set; }
  public string? Password { get; set; }

}
