using IQToolkit.Data.Ado;
using IQToolkit.Data.Common;
using IQToolkit.Data.Odbc;
using System.Data.Common;
using System.Data.Odbc;

namespace IQToolkit.Data.Access;

/// <summary>
/// A <see cref="DbEntityProvider"/> for Microsoft Access databases
/// </summary>
public class AccessQueryProvider : OdbcQueryProvider {
  /// <summary>
  /// Construct a <see cref="AccessQueryProvider"/>
  /// </summary>
  public AccessQueryProvider(OdbcConnection connection, QueryMapping mapping = null, QueryPolicy policy = null)
    : base(connection, AccessLanguage.Default, mapping, policy) { }

  /// <summary>
  /// Constructs a <see cref="AccessQueryProvider"/>
  /// </summary>
  public AccessQueryProvider(string connectionStringOrDatabaseFile, QueryMapping mapping = null, QueryPolicy policy = null)
    : this(CreateConnection(connectionStringOrDatabaseFile), mapping, policy) { }

  protected override DbEntityProvider New(DbConnection connection, QueryMapping mapping, QueryPolicy policy) => new AccessQueryProvider((OdbcConnection)connection, mapping, policy);

  /// <summary>
  /// Creates a <see cref="OdbcConnection"/> given a connection string or database file.
  /// </summary>
  public static OdbcConnection CreateConnection(string connectionStringOrDatabaseFile) {
    if (!connectionStringOrDatabaseFile.Contains("=")) {
      connectionStringOrDatabaseFile = GetConnectionString(connectionStringOrDatabaseFile);
    }
    return new OdbcConnection(connectionStringOrDatabaseFile);
  }

  /// <summary>
  /// Gets a connection string appropriate for openning the specified dadtabase file.
  /// </summary>
  public static string GetConnectionString(string databaseFile) {
    databaseFile = Path.GetFullPath(databaseFile);
    string dbLower = databaseFile.ToLower();
    if (dbLower.Contains(".mdb")) {
      return GetConnectionString(AccessOleDbProvider2000, databaseFile);
    } else if (dbLower.Contains(".accdb")) {
      return GetConnectionString(AccessOleDbProvider2007, databaseFile);
    } else {
      throw new InvalidOperationException(string.Format("Unrecognized file extension on database file '{0}'", databaseFile));
    }
  }

  private static string GetConnectionString(string provider, string databaseFile) {
    return string.Format("Provider={0};ole db services=0;Data Source={1}", provider, databaseFile);
  }

  public const string AccessOleDbProvider2000 = "Microsoft.Jet.OLEDB.4.0";
  public const string AccessOleDbProvider2007 = "Microsoft.ACE.OLEDB.12.0";

  protected override QueryExecutor CreateExecutor() => new Executor(this);

  public new class Executor : OdbcQueryProvider.Executor {
    AccessQueryProvider provider;

    public Executor(AccessQueryProvider provider) : base(provider) {
      this.provider = provider;
    }

    protected override DbCommand GetCommand(QueryCommand query, object[] paramValues) {
      var cmd = (OdbcCommand)provider.Connection.CreateCommand();
      cmd.CommandText = query.CommandText;
      SetParameterValues(query, cmd, paramValues);
      if (provider.Transaction != null) {
        cmd.Transaction = (OdbcTransaction)provider.Transaction;
      }
      return cmd;
    }

    protected override OdbcType GetOdbcType(QueryType type) {
      var sqlType = type as SqlQueryType;
      if (sqlType != null) {
        return sqlType.SqlDbType.ToOdbcType();
      }

      return base.GetOdbcType(type);
    }
  }
}
