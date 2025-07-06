namespace Common.AspNetCore.Configuration.Options {

  public class LoggingOptions : ILoggingOptions {
    public ILogLevelOptions LogLevel { get; set; } = new LogLevelOptions();

    public bool IncludeScopes { get; set; }

  }
}