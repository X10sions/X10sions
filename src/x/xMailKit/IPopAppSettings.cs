namespace MailKit;

public interface IHavePopAppSettings {
  IPopAppSettings Pop  { get; set; }
}

public interface IPopAppSettings {
  //https://github.com/jstedfast/MailKit/blob/master/Documentation/Examples/Pop3Examples.cs
  string? Password { get; set; }
  int Port { get; set; }
  string? Host { get; set; }
  string? Username { get; set; }
}