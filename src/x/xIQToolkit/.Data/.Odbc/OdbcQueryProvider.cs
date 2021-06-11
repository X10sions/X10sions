using IQToolkit.Data.Ado;
using IQToolkit.Data.Common;
using System;
using System.Data.Common;
using System.Data.Odbc;

namespace IQToolkit.Data.Odbc {
  /// <summary>
  /// A base <see cref="DbEntityProvider"/> for Odbc database providers
  /// </summary>
  public abstract class OdbcQueryProvider : DbEntityProvider {
    public OdbcQueryProvider(OdbcConnection connection, QueryLanguage language, QueryMapping mapping, QueryPolicy policy)
        : base(connection, language, mapping, policy) {
    }

    protected override QueryExecutor CreateExecutor() => new Executor(this);

    public new class Executor : DbEntityProvider.Executor {
      OdbcQueryProvider provider;

      public Executor(OdbcQueryProvider provider)
          : base(provider) {
        this.provider = provider;
      }

      protected override void AddParameter(DbCommand command, QueryParameter parameter, object value) {
        var qt = parameter.QueryType;
        if (qt == null) {
          qt = provider.Language.TypeSystem.GetColumnType(parameter.Type);
        }
        var p = ((OdbcCommand)command).Parameters.Add(parameter.Name, GetOdbcType(qt), qt.Length);
        if (qt.Precision != 0) {
          p.Precision = (byte)qt.Precision;
        }
        if (qt.Scale != 0) {
          p.Scale = (byte)qt.Scale;
        }
        p.Value = value ?? DBNull.Value;
      }
      protected virtual OdbcType GetOdbcType(QueryType type) => ((SqlQueryType)type).SqlDbType.ToOdbcType();
    }

  }
}