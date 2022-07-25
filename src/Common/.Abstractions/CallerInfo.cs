using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Common {
  public class CallerInfo {
    // https://stackoverflow.com/questions/16101152/is-performance-hit-by-using-caller-information-attributes
    public static CallerInfo Instance([CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) => new CallerInfo(filePath, memberName, lineNumber);

    public CallerInfo([CallerFilePath] string? filePath = null, [CallerMemberName] string? memberName = null, [CallerLineNumber] int lineNumber = 0) {
      FilePath = filePath ?? "Unknown";
      MemberName = memberName ?? "Unknown";
      LineNumber = lineNumber;
    }

    public CallerInfo(Exception exception, [CallerFilePath] string? filePath = null, [CallerMemberName] string? memberName = null, [CallerLineNumber] int lineNumber = 0)
      : this(filePath, memberName, lineNumber) {
      Exception = exception;
    }

    public string FilePath { get; }
    public string MemberName { get; }
    public int LineNumber { get; }
    public Exception? Exception { get; }

    public string Message() => $@"{nameof(FilePath)}:{FilePath}\n{nameof(MemberName)}:{MemberName}\n{nameof(LineNumber)}:{LineNumber}";

    public string ExceptionMessage() => ExceptionMessage(Exception);

    public string ExceptionMessage(Exception? ex) => $@"{Message()}\n{nameof(Exception)}: {ex?.Message}";

    public string ExceptionMessage(Exception ex, DbCommand command) => $"{ExceptionMessage(ex)}{Environment.NewLine}{command.ToSqlString()}";

  }
}
