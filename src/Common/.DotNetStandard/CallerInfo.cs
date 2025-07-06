using System.Runtime.CompilerServices;

namespace Common;
public readonly record struct CallerInfo([CallerFilePath] string FilePath = "", [CallerMemberName] string MemberName = "", [CallerLineNumber] int LineNumber = -1)  {
  //const string empty = "";

  // https://stackoverflow.com/questions/16101152/is-performance-hit-by-using-caller-information-attributes

  public CallerInfo(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = -1)
  : this(filePath, memberName, lineNumber) {
    Message = message;
  }

  public CallerInfo(Exception exception, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = -1)
    : this(filePath, memberName, lineNumber) {
    Exception = exception;
  }

  public static CallerInfo TraceMessage(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = -1)
    => new CallerInfo(message, memberName, sourceFilePath, sourceLineNumber);

  //public static void ValidateArgument(string parameterName, bool condition, [System.Runtime.CompilerServices.CallerArgumentExpression("condition")] string? message = null) {
  //  if (!condition) {
  //    throw new ArgumentException($"Argument failed validation: <{message}>", parameterName);
  //  }
  //}

  public string? Message { get; }
  public Exception? Exception { get; }

}
