namespace Microsoft.Extensions.Logging;
public static class ILoggerExtensions {

  /// <summary>
  /// Avoid the array allocation and any boxing allocations when the level isn't enabled
  /// </summary>
  public static void LogWhenEnabled<T>(this ILogger<T> logger, LogLevel logLevel, string? messageTemplate, params object?[] args) {
    if (logger.IsEnabled(logLevel)) {
      logger.Log(logLevel, messageTemplate, args);
    }
  }

  public static void LogCriticalWhenEnabled<T>(this ILogger<T> logger, string messageTemplate, params object?[] args) => logger.LogWhenEnabled(LogLevel.Critical, messageTemplate, args);
  public static void LogDebugWhenEnabled<T>(this ILogger<T> logger, string messageTemplate, params object?[] args) => logger.LogWhenEnabled(LogLevel.Debug, messageTemplate, args);
  public static void LogErrorWhenEnabled<T>(this ILogger<T> logger, string messageTemplate, params object?[] args) => logger.LogWhenEnabled(LogLevel.Error, messageTemplate, args);
  public static void LogInformationWhenEnabled<T>(this ILogger<T> logger, string messageTemplate, params object?[] args) => logger.LogWhenEnabled(LogLevel.Information, messageTemplate, args);
  public static void LogTraceWhenEnabled<T>(this ILogger<T> logger, string messageTemplate, params object?[] args) => logger.LogWhenEnabled(LogLevel.Trace, messageTemplate, args);
  public static void LogWarningWhenEnabled<T>(this ILogger<T> logger, string messageTemplate, params object?[] args) => logger.LogWhenEnabled(LogLevel.Warning, messageTemplate, args);

  public static void LogInformationWhenEnabled<T>(this ILogger<T> logger, string messageTemplate) => logger.LogWhenEnabled(LogLevel.Information, messageTemplate);
  public static void LogInformationWhenEnabled<T, T0>(this ILogger<T> logger, string messageTemplate, T0 arg0) => logger.LogWhenEnabled(LogLevel.Information, messageTemplate, arg0);
  public static void LogInformationWhenEnabled<T, T0, T1>(this ILogger<T> logger, string messageTemplate, T0 arg0, T1 arg1) => logger.LogWhenEnabled(LogLevel.Information, messageTemplate, arg0, arg1);
  public static void LogInformationWhenEnabled<T, T0, T1, T2>(this ILogger<T> logger, string messageTemplate, T0 arg0, T1 arg1, T2 arg2) => logger.LogWhenEnabled(LogLevel.Information, messageTemplate, arg0, arg1, arg2);

}