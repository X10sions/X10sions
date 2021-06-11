using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Common {
  public static class DbConnectionExtensions {

    static ParameterExpression p = Expression.Parameter(typeof(DbConnection));
    static MemberExpression prop = Expression.Property(p, nameof(DbProviderFactory));
    static UnaryExpression con = Expression.Convert(prop, typeof(DbProviderFactory));
    static LambdaExpression exp = Expression.Lambda(con, p);
    static Func<DbConnection, DbProviderFactory> s_func = (Func<DbConnection, DbProviderFactory>)exp.Compile();

    public static DbTransaction CurrentTransactionAsync { get; set; }

    public static Task<DbTransaction> BeginTransactionAsync(this DbConnection conn) => conn.BeginTransactionAsync(IsolationLevel.RepeatableRead, CancellationToken.None);

    public static Task<DbTransaction> BeginTransactionAsync(this DbConnection conn, CancellationToken cancellationToken) => conn.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken);

    public static Task<DbTransaction> BeginTransactionAsync(this DbConnection conn, IsolationLevel iso) => conn.BeginTransactionAsync(iso, CancellationToken.None);

    public static Task<DbTransaction> BeginTransactionAsync(this DbConnection conn, IsolationLevel iso, CancellationToken cancellationToken) {
      var result = new TaskCompletionSource<DbTransaction>();
      if (cancellationToken == CancellationToken.None || !cancellationToken.IsCancellationRequested) {
        try {
          CurrentTransactionAsync = conn.BeginTransaction(iso);
          result.SetResult(CurrentTransactionAsync);
        } catch (Exception ex) {
          result.SetException(ex);
        }
      } else {
        result.SetCanceled();
      }
      return result.Task;
    }

    public static DbDataAdapter CreateDataAdapter(this DbConnection connection) {
      var fact = GetProvider(connection);
      return fact.CreateDataAdapter();
    }

    public static DataSet GetDataSet(this DbConnection connection, string commandText) {
      var cmd = connection.CreateCommand();
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
      cmd.CommandText = commandText;
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
      var adapter = connection.CreateDataAdapter();
      adapter.SelectCommand = cmd;
      var dataset = new DataSet();
      adapter.Fill(dataset);
      return dataset;
    }

    public static DbProviderFactory GetProviderByReflection(DbConnection conn) {
      var t = conn.GetType();
      var pi = t.GetProperty(nameof(DbProviderFactory), BindingFlags.NonPublic | BindingFlags.Instance);
      return (DbProviderFactory)pi.GetValue(conn);
    }

    public static DbProviderFactory GetProvider(this DbConnection connection) => s_func(connection);


  }
}