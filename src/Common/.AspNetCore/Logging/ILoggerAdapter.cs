using Microsoft.Extensions.Logging;

namespace Common.Logging;

public class LoggerAdapter<T> : ILoggerAdapter<T> {
  public LoggerAdapter(ILogger<T> logger) {
    Logger = logger;
  }
  public ILogger<T> Logger { get; }

  //public void LogInformation(string message) {
  //  if (logger.IsEnabled(LogLevel.Information)) {
  //    logger.LogInformation(message);
  //  }
  //}
  //public void LogInformation<T0>(string message, T0 arg0) {
  //  if (logger.IsEnabled(LogLevel.Information)) {
  //    logger.LogInformation(message, arg0);
  //  }
  //}
  //public void LogInformation<T0, T1>(string message, T0 arg0, T1 arg1) {
  //  if (logger.IsEnabled(LogLevel.Information)) {
  //    logger.LogInformation(message,  arg0, arg1);
  //  }
  //}
  //public void LogInformation<T0, T1, T2>(string message, T0 arg0, T1 arg1, T1 arg2) {
  //  if (logger.IsEnabled(LogLevel.Information)) {
  //    logger.LogInformation(message, arg0, arg1, arg2);
  //  }
  //}
}


public interface ILoggerAdapter<T> {
  ILogger<T> Logger { get; }
  //void LogInformation(string message);
  //void LogInformation<T0>(string message, T0 arg0);
  //void LogInformation<T0, T1>(string message, T0 arg0, T1 ar1);
  //void LogInformation<T0, T1, T2>(string message, T0 arg0, T1 ar1, T1 arg2);
}

public static class ILoggerAdapterExtensions {

  public static void LogBenchmarkMessage<T>(ILogger<T> logger) {
    logger.LogBenchmarkMessageGen(69, 420);

  }
  //public static void LogInformation<T>(this ILoggerAdapter<T> la, string message) => la.Logger.LogInformation( message);
  //public static void LogInformation<T, T0>(this ILoggerAdapter<T> la, string message, T0 arg0) => la.Logger.LogWhenEnabled(LogLevel.Information, message, arg0);
  //public static void LogInformation<T, T0, T1>(this ILoggerAdapter<T> la, string message, T0 arg0, T1 arg1) => la.Logger.LogWhenEnabled(LogLevel.Information, message, arg0, arg1);
  //public static void LogInformation<T, T0, T1, T2>(this ILoggerAdapter<T> la, string message, T0 arg0, T1 arg1, T2 arg2) => la.Logger.LogWhenEnabled(LogLevel.Information, message, arg0, arg1, arg2);

}

public static partial class LoggerMessageDefinitionsGen {

  [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "This is a log message with paramters {first}, {second}", SkipEnabledCheck = true)]
  public static partial void LogBenchmarkMessageGen(this ILogger logger, int first, int second);
}