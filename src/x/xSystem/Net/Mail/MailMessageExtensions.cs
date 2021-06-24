using System.Text;
using System.Threading.Tasks;

namespace System.Net.Mail {
  public static class MailMessageExtensions {

    public static MailMessage AddBcc(this MailMessage msg, MailAddress emailBCC) => msg.AddToCcBcc(null, null, emailBCC);
    public static MailMessage AddBcc(this MailMessage msg, string emailBCC) => msg.AddToCcBcc(null, null, emailBCC);
    public static MailMessage AddCC(this MailMessage msg, MailAddress emailCC) => msg.AddToCcBcc(null, emailCC, null);
    public static MailMessage AddCC(this MailMessage msg, string emailCC) => msg.AddToCcBcc(null, emailCC, null);
    public static MailMessage AddTo(this MailMessage msg, string emailTo) => msg.AddToCcBcc(emailTo, null, null);
    public static MailMessage AddTo(this MailMessage msg, MailAddress emailTo) => msg.AddToCcBcc(emailTo, null, null);

    public static MailMessage AddToCcBcc(this MailMessage msg, MailAddress emailTo = null, MailAddress emailCC = null, MailAddress emailBCC = null) {
      if(emailTo != null) {
        msg.To.Add(emailTo);
      }
      if(emailCC != null) {
        msg.CC.Add(emailCC);
      }
      if(emailBCC != null) {
        msg.Bcc.Add(emailBCC);
      }
      return msg;
    }

    public static MailMessage AddToCcBcc(this MailMessage msg, string emailTo = null, string emailCC = null, string emailBCC = null) {
      if(!string.IsNullOrWhiteSpace(emailTo)) {
        msg.To.Add(emailTo);
      }
      if(!string.IsNullOrWhiteSpace(emailCC)) {
        msg.CC.Add(emailCC);
      }
      if(!string.IsNullOrWhiteSpace(emailBCC)) {
        msg.Bcc.Add(emailBCC);
      }
      return msg;
    }

    public static MailMessage SetBody(this MailMessage mailMessage, string value) {
      mailMessage.Body = value;
      return mailMessage;
    }

    public static MailMessage SetBodyEncoding(this MailMessage mailMessage, Encoding value) {
      //= Encoding.UTF8
      mailMessage.BodyEncoding = value;
      return mailMessage;
    }

    public static MailMessage SetFrom(this MailMessage mailMessage, MailAddress value) {
      mailMessage.From = value;
      return mailMessage;
    }

    public static MailMessage SetFrom(this MailMessage mailMessage, string address, string displayName = null) {
      mailMessage.From = new MailAddress(address, displayName);
      return mailMessage;
    }

    public static MailMessage SetIsBodyHtml(this MailMessage mailMessage, bool value) {
      mailMessage.IsBodyHtml = value;
      return mailMessage;
    }

    public static MailMessage SetSubject(this MailMessage mailMessage, string value) {
      mailMessage.Subject = value;
      return mailMessage;
    }

    public static MailMessage SetSubjectEncoding(this MailMessage mailMessage, Encoding value) {
      //= Encoding.UTF8
      mailMessage.SubjectEncoding = value;
      return mailMessage;
    }

    //public static void Send(this MailMessage mailMessage) => mailMessage.Send(new SmtpClient());
    public static void Send(this MailMessage mailMessage, SmtpClient client) => client.Send(mailMessage);

    public static async Task SendMailAsync(this MailMessage mailMessage) => await mailMessage.SendMailAsync(new SmtpClient());
    public static async Task SendMailAsync(this MailMessage mailMessage, SmtpClient client) => await client.SendMailAsync(mailMessage);

  }
}