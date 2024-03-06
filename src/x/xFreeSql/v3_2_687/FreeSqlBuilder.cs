using FreeSql.v3_2_687.DB2i;
using System.Data.Common;

namespace FreeSql.v3_2_687;

/// <summary>
/// v2022_12_23
/// </summary>
public class xFreeSqlBuilder : FreeSqlBuilder {
  public xFreeSqlBuilder(xDataType dataType) {
    _dataType = dataType;
  }

  xDataType _dataType;

  public xFreeSqlBuilder UseConnectionString(string connectionString) {
    base.UseConnectionString(DataType.Custom, connectionString, null);
    return this;
  }

  public xFreeSqlBuilder UseConnectionFactory(Func<DbConnection> connectionFactory) {
    base.UseConnectionFactory(DataType.Custom, connectionFactory, null);
    return this;
  }

  public new xFreeSqlBuilder UseQuoteSqlName(bool value) { base.UseQuoteSqlName(value); return this; }

  public new IFreeSql<TMark> Build<TMark>() =>
    _dataType switch {
      xDataType.DB2i => new DB2iProvider<TMark>(),
      xDataType.DB2iOdbc => new DB2iOdbcProvider<TMark>(),
      xDataType.DB2iOleDb => new DB2iOleDbProvider<TMark>(),
      _ => base.Build<TMark>()
    };
}

public enum xDataType {
  DB2i,
  DB2iOdbc,
  DB2iOleDb
  //CustomPostgreSQL
}