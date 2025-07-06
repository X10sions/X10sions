using System;

namespace Common.Models {

  public class DebugInfo {
    public string FilePath { get; set; } = string.Empty;
    public int LineNumber { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Namespace { get; set; } = string.Empty;

    public string ClassCsvString(string delimeter = ",") => string.Join(delimeter, DateTime.Now.ToSqlDate(), Namespace, Name, MemberName, LineNumber);
    public string FileCsvString(string delimeter = ",") => string.Join(delimeter, DateTime.Now.ToSqlDate(), FilePath, MemberName, LineNumber);
  }
}
