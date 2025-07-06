namespace Common.Mail;

public interface IMailAddress {
  string Address { get; }
  string DisplayName { get; }
}

public class MailAddress : IMailAddress {
  public MailAddress() : this(string.Empty) { }
  public MailAddress(string address) {
    Address = address;
  }
  public MailAddress(string displayName, string address) : this(address) {
    DisplayName = displayName;
  }
  public string Address { get; set; }
  public string? DisplayName { get; set; }
}
