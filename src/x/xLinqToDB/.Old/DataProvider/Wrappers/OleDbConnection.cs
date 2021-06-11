//using LinqToDB.Expressions;
//using System;
//using System.Data;

//namespace LinqToDB.DataProvider.Wrappers {
//  [Wrapper]
//  public class OleDbConnection : IDbConnection {
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
//    public DataTable GetOleDbSchemaTable(Guid schema, object[]? restrictions) => throw new NotImplementedException();
//    public void Open() => throw new NotImplementedException();
//  }

//  [Wrapper]
//  public class OleDbParameter : IDbDataParameter {
//    public OleDbType OleDbType { get; set; }

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
//  public enum OleDbType {
//    BigInt = 20,
//    Binary = 128,
//    Boolean = 11,
//    BSTR = 8,
//    Char = 129,
//    Currency = 6,
//    Date = 7,
//    DBDate = 133,
//    DBTime = 134,
//    DBTimeStamp = 135,
//    Decimal = 14,
//    Double = 5,
//    Empty = 0,
//    Error = 10,
//    Filetime = 64,
//    Guid = 72,
//    IDispatch = 9,
//    Integer = 3,
//    IUnknown = 13,
//    LongVarBinary = 205,
//    LongVarChar = 201,
//    LongVarWChar = 203,
//    Numeric = 131,
//    PropVariant = 138,
//    Single = 4,
//    SmallInt = 2,
//    TinyInt = 16,
//    UnsignedBigInt = 21,
//    UnsignedInt = 19,
//    UnsignedSmallInt = 18,
//    UnsignedTinyInt = 17,
//    VarBinary = 204,
//    VarChar = 200,
//    Variant = 12,
//    VarNumeric = 139,
//    VarWChar = 202,
//    WChar = 130
//  }

//}