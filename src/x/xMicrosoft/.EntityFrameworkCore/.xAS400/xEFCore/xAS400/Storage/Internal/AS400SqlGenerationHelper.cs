using IBM.Data.DB2.iSeries;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Text;
using xEFCore.xAS400.Infrastructure.Internal;

namespace xEFCore.xAS400.Storage.Internal {
  public class AS400SqlGenerationHelper : RelationalSqlGenerationHelper {

    char schemaTableSeperator;

    public AS400SqlGenerationHelper(
      [NotNull] RelationalSqlGenerationHelperDependencies dependencies,
      [NotNull] IAS400Options options)
        : base(dependencies) {
      //iDB2Connection conn;
      //conn.GetConnectionStringBuilder().Naming ?
      iDB2NamingConvention naming = iDB2NamingConvention.System;
      naming = options.NamingConvention;
      schemaTableSeperator = naming.Seperator();
    }

    public override string DelimitIdentifier(string identifier)
        => EscapeIdentifier(Check.NotEmpty(identifier, nameof(identifier)));

    public override void DelimitIdentifier(StringBuilder builder, string identifier) {
      Check.NotEmpty(identifier, nameof(identifier));
      //builder.Append('[');
      EscapeIdentifier(builder, identifier);
      //builder.Append(']');
    }

    public override string DelimitIdentifier(string name, string schema) {
      return (!string.IsNullOrEmpty(schema)
                         ? DelimitIdentifier(schema) + schemaTableSeperator
                         : string.Empty)
                     + DelimitIdentifier(Check.NotEmpty(name, nameof(name)));
    }

    public override void DelimitIdentifier(StringBuilder builder, string name, string schema) {
      if (!string.IsNullOrEmpty(schema)) {
        DelimitIdentifier(builder, schema);
        builder.Append(schemaTableSeperator);
      }
      DelimitIdentifier(builder, name);
    }

  }
}