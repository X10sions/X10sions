//using LinqToDB.Expressions;
//using System;
//using System.Data;

//namespace LinqToDB.DataProvider.Wrappers {
//  [Wrapper]
//  public class OdbcConnection : IDbConnection {
//    public string ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

//    public int ConnectionTimeout => throw new NotImplementedException();

//    public string Database => throw new NotImplementedException();

//    public ConnectionState State => throw new NotImplementedException();

//    public IDbTransaction BeginTransaction() => throw new NotImplementedException();
//    public IDbTransaction BeginTransaction(IsolationLevel il) => throw new NotImplementedException();
//    public void ChangeDatabase(string databaseName) => throw new NotImplementedException();
//    public void Close() => throw new NotImplementedException();
//    public IDbCommand CreateCommand() => throw new NotImplementedException();
//    public void Dispose() => throw new NotImplementedException();
//    public void Open() => throw new NotImplementedException();
//  }

//  [Wrapper]
//  public class OdbcParameter : IDbDataParameter {
//    public OdbcType OdbcType { get; set; }

//    public byte Precision { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//    public byte Scale { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//    public int Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//    public DbType DbType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//    public ParameterDirection Direction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

//    public bool IsNullable => throw new NotImplementedException();

//    public string ParameterName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//    public string SourceColumn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//    public DataRowVersion SourceVersion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//    public object Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//  }

//  [Wrapper]
//  public enum OdbcType {
//    BigInt = 1,
//    Binary = 2,
//    Bit = 3,
//    Char = 4,
//    Date = 23,
//    DateTime = 5,
//    Decimal = 6,
//    Double = 8,
//    Image = 9,
//    Int = 10,
//    NChar = 11,
//    NText = 12,
//    Numeric = 7,
//    NVarChar = 13,
//    Real = 14,
//    SmallDateTime = 16,
//    SmallInt = 17,
//    Text = 18,
//    Time = 24,
//    Timestamp = 19,
//    TinyInt = 20,
//    UniqueIdentifier = 15,
//    VarBinary = 21,
//    VarChar = 22
//  }

//}