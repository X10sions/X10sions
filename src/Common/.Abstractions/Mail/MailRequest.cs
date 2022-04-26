namespace Common.Mail;

public interface IMailRequest {
  string Body { get; }
  MailAddress? From { get; }
  bool IsBodyHtml { get; }
  string Subject { get; }
  List<MailAddress> To { get; }
  List<MailAddress> Cc { get; }
  List<MailAddress> Bcc { get; }
}

public class MailRequest : IMailRequest {
  public MailRequest(string subject, string body
    , string? from = null
    , IEnumerable<string>? to = null
    , IEnumerable<string>? cc = null
    , IEnumerable<string>? bcc = null
    , bool isBodyHtml = true) {
    if (from is not null) From = new MailAddress(from);
    if (to is not null) To.AddRange(to.Select(x => new MailAddress(x)));
    if (cc is not null) Cc.AddRange(cc.Select(x => new MailAddress(x)));
    if (bcc is not null) Bcc.AddRange(bcc.Select(x => new MailAddress(x)));
    Subject = subject;
    Body = body;
    IsBodyHtml = isBodyHtml;
  }
  public MailRequest(string subject, string body, string to, string? from = null
    , IEnumerable<string>? cc = null
    , IEnumerable<string>? bcc = null
    , bool isBodyHtml = true) : this(subject, body, from, new[] { to }, cc, bcc, isBodyHtml) { }

  public string Body { get; set; }
  public MailAddress? From { get; set; }
  public bool IsBodyHtml { get; set; }
  public string Subject { get; set; }
  public List<MailAddress> To { get; } = new List<MailAddress>();
  public List<MailAddress> Cc { get; } = new List<MailAddress>();
  public List<MailAddress> Bcc { get; } = new List<MailAddress>();
}