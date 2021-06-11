using System;

namespace Microsoft.Extensions.Logging {
  public static class ILoggerExtensions {

    #region "Twitter Logging Extensions"

    static Action<ILogger, Exception> _obtainRequestToken = LoggerMessage.Define(eventId: 1, logLevel: LogLevel.Debug, formatString: nameof(ObtainRequestToken));
    static Action<ILogger, Exception> _obtainAccessToken = LoggerMessage.Define(eventId: 2, logLevel: LogLevel.Debug, formatString: nameof(ObtainAccessToken));
    static Action<ILogger, Exception> _retrieveUserDetails = LoggerMessage.Define(eventId: 3, logLevel: LogLevel.Debug, formatString: nameof(RetrieveUserDetails));
    static Action<ILogger, Exception> _accessDeniedError = LoggerMessage.Define(eventId: 17, logLevel: LogLevel.Information, formatString: "Access was denied by the resource owner or by the remote server.");
    static Action<ILogger, Exception> _accessDeniedContextHandled = LoggerMessage.Define(eventId: 18, logLevel: LogLevel.Debug, formatString: "The AccessDenied event returned Handled.");
    static Action<ILogger, Exception> _accessDeniedContextSkipped = LoggerMessage.Define(eventId: 19, logLevel: LogLevel.Debug, formatString: "The AccessDenied event returned Skipped.");

    public static void ObtainAccessToken(this ILogger logger) => _obtainAccessToken(logger, null);
    public static void ObtainRequestToken(this ILogger logger) => _obtainRequestToken(logger, null);
    public static void RetrieveUserDetails(this ILogger logger) => _retrieveUserDetails(logger, null);
    public static void AccessDeniedError(this ILogger logger) => _accessDeniedError(logger, null);
    public static void AccessDeniedContextHandled(this ILogger logger) => _accessDeniedContextHandled(logger, null);
    public static void AccessDeniedContextSkipped(this ILogger logger) => _accessDeniedContextSkipped(logger, null);

    #endregion

  }
}