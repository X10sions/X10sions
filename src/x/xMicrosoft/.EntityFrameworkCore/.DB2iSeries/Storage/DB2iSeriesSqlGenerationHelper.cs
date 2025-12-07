using Microsoft.EntityFrameworkCore.Storage;
using System.Text;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Storage;

public class DB2iSeriesSqlGenerationHelper : RelationalSqlGenerationHelper {
  public DB2iSeriesSqlGenerationHelper(RelationalSqlGenerationHelperDependencies dependencies)
      : base(dependencies) {
  }

  public override string DelimitIdentifier(string identifier)      => $"\"{identifier}\"";

  public override void DelimitIdentifier(StringBuilder builder, string identifier) {
    builder.Append('"');
    builder.Append(identifier);
    builder.Append('"');
  }

  public override string EscapeIdentifier(string identifier)      => identifier.Replace("\"", "\"\"");

  public override void EscapeIdentifier(StringBuilder builder, string identifier) {
    var initialLength = builder.Length;
    builder.Append(identifier);
    builder.Replace("\"", "\"\"", initialLength, identifier.Length);
  }

  public override string StatementTerminator => ";";
}