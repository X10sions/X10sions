extern alias MySqlConnector;
extern alias MySqlData;

using IQToolkit.Data.Ado;
using IQToolkit.Data.Common;
using MySqlData::MySql.Data.MySqlClient;
using System;
using System.Data.Common;

namespace IQToolkit.Data.MySqlClient {

  /// <summary>
  /// A <see cref="DbEntityProvider"/> for MySql databases
  /// </summary>
  public class MySqlQueryProvider : DbEntityProvider {
    /// <summary>
    /// Constructs a <see cref="MySqlQueryProvider"/>
    /// </summary>
    public MySqlQueryProvider(MySqlConnection connection, QueryMapping mapping = null, QueryPolicy policy = null)
        : base(connection, MySqlLanguage.Default, mapping, policy) {
    }

    /// <summary>
    /// Constructs a <see cref="MySqlQueryProvider"/>
    /// </summary>
    public MySqlQueryProvider(string connectionString, QueryMapping mapping = null, QueryPolicy policy = null)
        : this(new MySqlConnection(connectionString), mapping, policy) {
    }

    protected override DbEntityProvider New(DbConnection connection, QueryMapping mapping, QueryPolicy policy) => new MySqlQueryProvider((MySqlConnection)connection, mapping, policy);

    protected override QueryExecutor CreateExecutor() => new Executor(this);

    new class Executor : DbEntityProvider.Executor {
      MySqlQueryProvider provider;

      public Executor(MySqlQueryProvider provider)
          : base(provider) {
        this.provider = provider;
      }

      protected override bool BufferResultRows => true;

      protected override void AddParameter(DbCommand command, QueryParameter parameter, object value) {
        var sqlType = (SqlQueryType)parameter.QueryType;
        if (sqlType == null) {
          sqlType = (SqlQueryType)provider.Language.TypeSystem.GetColumnType(parameter.Type);
        }

        var p = ((MySqlCommand)command).Parameters.Add(parameter.Name, sqlType.SqlDbType.ToMySqlDbType(), sqlType.Length);
        if (sqlType.Precision != 0) {
          p.Precision = (byte)sqlType.Precision;
        }
        if (sqlType.Scale != 0) {
          p.Scale = (byte)sqlType.Scale;
        }
        p.Value = value ?? DBNull.Value;
      }
    }

  }
}