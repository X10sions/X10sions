using IQToolkit.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace IQToolkit.Data.Ado {

  /// <summary>
  /// The base type for <see cref="EntityProvider"/>'s that use a System.Data.<see cref="DbConnection"/>.
  /// </summary>
  public abstract class DbEntityProvider : EntityProvider {
    private readonly DbConnection connection;
    private DbTransaction transaction;
    private int nConnectedActions = 0;

    /// <summary>
    /// Constructs a new <see cref="DbEntityProvider"/>
    /// </summary>
    protected DbEntityProvider(DbConnection connection, QueryLanguage language, QueryMapping mapping, QueryPolicy policy)
        : base(language, mapping, policy) {
      if (connection == null)
        throw new InvalidOperationException("Connection not specified");
      this.connection = connection;
    }

    /// <summary>
    /// The <see cref="DbConnection"/> used for executing queries.
    /// </summary>
    public virtual DbConnection Connection => connection;

    /// <summary>
    /// The <see cref="DbTransaction"/> to use for updates.
    /// </summary>
    public virtual DbTransaction Transaction {
      get => transaction;

      set {
        if (value != null && value.Connection != connection)
          throw new InvalidOperationException("Transaction does not match connection.");
        transaction = value;
      }
    }

    /// <summary>
    /// The <see cref="System.Data.IsolationLevel"/> used for transactions.
    /// </summary>
    public IsolationLevel Isolation { get; set; } = IsolationLevel.ReadCommitted;

    protected virtual DbEntityProvider New(DbConnection connection, QueryMapping mapping, QueryPolicy policy) => (DbEntityProvider)Activator.CreateInstance(GetType(), new object[] { connection, mapping, policy });

    /// <summary>
    /// Creates a new instance of the <see cref="DbEntityProvider"/> that uses the specified <see cref="QueryMapping"/>.
    /// </summary>
    public DbEntityProvider WithMapping(QueryMapping mapping) {
      var n = New(Connection, mapping, Policy);
      n.Log = Log;
      return n;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="DbEntityProvider"/> that uses the specified <see cref="QueryPolicy"/>.
    /// </summary>
    public DbEntityProvider WithPolicy(QueryPolicy policy) {
      var n = New(Connection, Mapping, policy);
      n.Log = Log;
      return n;
    }

    /// <summary>
    /// True if a query or other action caused the connection to become open.
    /// </summary>
    protected bool ActionOpenedConnection { get; private set; } = false;

    /// <summary>
    /// Opens the connection if it is currently closed.
    /// </summary>
    protected void StartUsingConnection() {
      if (connection.State == ConnectionState.Closed) {
        connection.Open();
        ActionOpenedConnection = true;
      }

      nConnectedActions++;
    }

    /// <summary>
    /// Closes the connection if no actions still require it.
    /// </summary>
    protected void StopUsingConnection() {
      System.Diagnostics.Debug.Assert(nConnectedActions > 0);

      nConnectedActions--;

      if (nConnectedActions == 0 && ActionOpenedConnection) {
        connection.Close();
        ActionOpenedConnection = false;
      }
    }

    /// <summary>
    /// Invokes the specified <see cref="Action"/> while the connection is open.
    /// </summary>
    public override void DoConnected(Action action) {
      StartUsingConnection();
      try {
        action();
      } finally {
        StopUsingConnection();
      }
    }

    /// <summary>
    /// Invokes the specified <see cref="Action"/> during a database transaction.
    /// If no transaction is currently associated with the <see cref="DbEntityProvider"/> a new
    /// one is started for the duration of the action. If the action completes without exception
    /// the transation is commited, otherwise it is aborted.
    /// </summary>
    public override void DoTransacted(Action action) {
      StartUsingConnection();
      try {
        if (Transaction == null) {
          var trans = Connection.BeginTransaction(Isolation);
          try {
            Transaction = trans;
            action();
            trans.Commit();
          } finally {
            Transaction = null;
            trans.Dispose();
          }
        } else {
          action();
        }
      } finally {
        StopUsingConnection();
      }
    }

    /// <summary>
    /// Execute the command specified in the database's language against the database.
    /// </summary>
    public override int ExecuteCommand(string commandText) {
      if (Log != null) {
        Log.WriteLine(commandText);
      }

      StartUsingConnection();
      try {
        var cmd = Connection.CreateCommand();
        cmd.CommandText = commandText;
        return cmd.ExecuteNonQuery();
      } finally {
        StopUsingConnection();
      }
    }

    protected override QueryExecutor CreateExecutor() => new Executor(this);

    public class Executor : QueryExecutor {
      private readonly DbEntityProvider provider;
      private int rowsAffected;

      public Executor(DbEntityProvider provider) {
        this.provider = provider;
      }

      public DbEntityProvider Provider => provider;

      public override int RowsAffected => rowsAffected;

      protected virtual bool BufferResultRows => false;

      protected bool ActionOpenedConnection => provider.ActionOpenedConnection;

      protected void StartUsingConnection() => provider.StartUsingConnection();

      protected void StopUsingConnection() => provider.StopUsingConnection();

      public override object Convert(object value, Type type) {
        if (value == null) {
          return TypeHelper.GetDefault(type);
        }

        type = TypeHelper.GetNonNullableType(type);
        var vtype = value.GetType();

        if (type != vtype) {
          if (type.GetTypeInfo().IsEnum) {
            if (vtype == typeof(string)) {
              return Enum.Parse(type, (string)value);
            } else {
              var utype = Enum.GetUnderlyingType(type);

              if (utype != vtype) {
                value = System.Convert.ChangeType(value, utype);
              }

              return Enum.ToObject(type, value);
            }
          }

          return System.Convert.ChangeType(value, type);
        }

        return value;
      }

      public override IEnumerable<T> Execute<T>(QueryCommand command, Func<FieldReader, T> fnProjector, MappingEntity entity, object[] paramValues) {
        LogCommand(command, paramValues);
        StartUsingConnection();

        try {
          var cmd = GetCommand(command, paramValues);
          var reader = ExecuteReader(cmd);
          var result = Project(reader, fnProjector, entity, true);

          if (provider.ActionOpenedConnection) {
            result = result.ToList();
          } else {
            result = new EnumerateOnce<T>(result);
          }

          return result;
        } finally {
          StopUsingConnection();
        }
      }

      protected virtual DbDataReader ExecuteReader(DbCommand command) {
        var reader = command.ExecuteReader();

#if false
                if (this.BufferResultRows)
                {
                    // use data table to buffer results
                    var ds = new DataSet();
                    ds.EnforceConstraints = false;
                    var table = new DataTable();
                    ds.Tables.Add(table);
                    ds.EnforceConstraints = false;
                    table.Load(reader);
                    reader = table.CreateDataReader();
                }
#endif

        return reader;
      }

      protected virtual IEnumerable<T> Project<T>(DbDataReader reader, Func<FieldReader, T> fnProjector, MappingEntity entity, bool closeReader) {
        var freader = new DbFieldReader(this, reader);
        try {
          while (reader.Read()) {
            yield return fnProjector(freader);
          }
        } finally {
          if (closeReader) {
            ((IDataReader)reader).Close();
          }
        }
      }

      public override int ExecuteCommand(QueryCommand query, object[] paramValues) {
        LogCommand(query, paramValues);
        StartUsingConnection();
        try {
          var cmd = GetCommand(query, paramValues);
          rowsAffected = cmd.ExecuteNonQuery();
          return rowsAffected;
        } finally {
          StopUsingConnection();
        }
      }

      public override IEnumerable<int> ExecuteBatch(QueryCommand query, IEnumerable<object[]> paramSets, int batchSize, bool stream) {
        StartUsingConnection();
        try {
          var result = ExecuteBatch(query, paramSets);
          if (!stream || ActionOpenedConnection) {
            return result.ToList();
          } else {
            return new EnumerateOnce<int>(result);
          }
        } finally {
          StopUsingConnection();
        }
      }

      private IEnumerable<int> ExecuteBatch(QueryCommand query, IEnumerable<object[]> paramSets) {
        LogCommand(query, null);
        var cmd = GetCommand(query, null);
        foreach (var paramValues in paramSets) {
          LogParameters(query, paramValues);
          LogMessage("");
          SetParameterValues(query, cmd, paramValues);
          rowsAffected = cmd.ExecuteNonQuery();
          yield return rowsAffected;
        }
      }

      public override IEnumerable<T> ExecuteBatch<T>(QueryCommand query, IEnumerable<object[]> paramSets, Func<FieldReader, T> fnProjector, MappingEntity entity, int batchSize, bool stream) {
        StartUsingConnection();
        try {
          var result = ExecuteBatch(query, paramSets, fnProjector, entity);
          if (!stream || ActionOpenedConnection) {
            return result.ToList();
          } else {
            return new EnumerateOnce<T>(result);
          }
        } finally {
          StopUsingConnection();
        }
      }

      private IEnumerable<T> ExecuteBatch<T>(QueryCommand query, IEnumerable<object[]> paramSets, Func<FieldReader, T> fnProjector, MappingEntity entity) {
        LogCommand(query, null);
        var cmd = GetCommand(query, null);
        cmd.Prepare();
        foreach (var paramValues in paramSets) {
          LogParameters(query, paramValues);
          LogMessage("");
          SetParameterValues(query, cmd, paramValues);
          var reader = ExecuteReader(cmd);
          var freader = new DbFieldReader(this, reader);
          try {
            if (reader.HasRows) {
              reader.Read();
              yield return fnProjector(freader);
            } else {
              yield return default(T);
            }
          } finally {
            ((IDataReader)reader).Close();
          }
        }
      }

      public override IEnumerable<T> ExecuteDeferred<T>(QueryCommand query, Func<FieldReader, T> fnProjector, MappingEntity entity, object[] paramValues) {
        LogCommand(query, paramValues);
        StartUsingConnection();
        try {
          var cmd = GetCommand(query, paramValues);
          var reader = ExecuteReader(cmd);
          var freader = new DbFieldReader(this, reader);
          try {
            while (reader.Read()) {
              yield return fnProjector(freader);
            }
          } finally {
            ((IDataReader)reader).Close();
          }
        } finally {
          StopUsingConnection();
        }
      }

      /// <summary>
      /// Get an ADO command object initialized with the command-text and parameters
      /// </summary>
      protected virtual DbCommand GetCommand(QueryCommand query, object[] paramValues) {
        // create command object (and fill in parameters)
        var cmd = provider.Connection.CreateCommand();
        cmd.CommandText = query.CommandText;
        if (provider.Transaction != null)
          cmd.Transaction = provider.Transaction;
        SetParameterValues(query, cmd, paramValues);
        return cmd;
      }

      protected virtual void SetParameterValues(QueryCommand query, DbCommand command, object[] paramValues) {
        if (query.Parameters.Count > 0 && command.Parameters.Count == 0) {
          for (int i = 0, n = query.Parameters.Count; i < n; i++) {
            AddParameter(command, query.Parameters[i], paramValues != null ? paramValues[i] : null);
          }
        } else if (paramValues != null) {
          for (int i = 0, n = command.Parameters.Count; i < n; i++) {
            var p = command.Parameters[i];
            if (p.Direction == System.Data.ParameterDirection.Input
             || p.Direction == System.Data.ParameterDirection.InputOutput) {
              p.Value = paramValues[i] ?? DBNull.Value;
            }
          }
        }
      }

      protected virtual void AddParameter(DbCommand command, QueryParameter parameter, object value) {
        var p = command.CreateParameter();
        p.ParameterName = parameter.Name;
        p.Value = value ?? DBNull.Value;
        command.Parameters.Add(p);
      }

      protected virtual void GetParameterValues(DbCommand command, object[] paramValues) {
        if (paramValues != null) {
          for (int i = 0, n = command.Parameters.Count; i < n; i++) {
            if (command.Parameters[i].Direction != System.Data.ParameterDirection.Input) {
              var value = command.Parameters[i].Value;
              if (value == DBNull.Value)
                value = null;
              paramValues[i] = value;
            }
          }
        }
      }

      protected virtual void LogMessage(string message) {
        if (provider.Log != null) {
          provider.Log.WriteLine(message);
        }
      }

      /// <summary>
      /// Write a command and parameters to the log
      /// </summary>
      /// <param name="command"></param>
      /// <param name="paramValues"></param>
      protected virtual void LogCommand(QueryCommand command, object[] paramValues) {
        if (provider.Log != null) {
          provider.Log.WriteLine(command.CommandText);
          if (paramValues != null) {
            LogParameters(command, paramValues);
          }
          provider.Log.WriteLine();
        }
      }

      protected virtual void LogParameters(QueryCommand command, object[] paramValues) {
        if (provider.Log != null && paramValues != null) {
          for (int i = 0, n = command.Parameters.Count; i < n; i++) {
            var p = command.Parameters[i];
            var v = paramValues[i];

            if (v == null || v == DBNull.Value) {
              provider.Log.WriteLine("-- {0} = NULL", p.Name);
            } else {
              provider.Log.WriteLine("-- {0} = [{1}]", p.Name, v);
            }
          }
        }
      }
    }
  }
}
