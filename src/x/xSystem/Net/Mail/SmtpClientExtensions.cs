namespace System.Net.Mail {
  public static class SmtpClientExtensions {

    public static SmtpClient SetCredentials(this SmtpClient client, NetworkCredential value) {
      client.Credentials = value;
      return client;
    }

    public static SmtpClient SetDeliveryFormat(this SmtpClient client, SmtpDeliveryFormat value) {
      client.DeliveryFormat = value;
      return client;
    }

    public static SmtpClient SetDeliveryMethod(this SmtpClient client, SmtpDeliveryMethod value) {
      client.DeliveryMethod = value;
      return client;
    }

    public static SmtpClient SetEnableSsl(this SmtpClient client, bool value) {
      client.EnableSsl = value;
      return client;
    }

    public static SmtpClient SetHost(this SmtpClient client, string value) {
      client.Host = value;
      return client;
    }

    public static SmtpClient SetPort(this SmtpClient client, int value) {
      client.Port = value;
      return client;
    }

  }
}
