using RepoDb.Extensions;
using RepoDb.Interfaces;
using RepoDb.Resolvers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace RepoDb.DbHelpers {
  public class DB2iSeriesDbHelper<TConnection> : IDbHelper where TConnection : DbConnection {
    public DB2iSeriesDbHelper() {
      DbTypeResolver = new DB2iSeriesDbTypeNameToClientTypeResolver();
    }

    private IDbSetting m_dbSetting = DbSettingMapper.Get<TConnection>();

    public IResolver<string, Type> DbTypeResolver { get; }

    public IEnumerable<DbField> GetFields(IDbConnection connection, string tableName, IDbTransaction transaction = null) {
      var commandText = GetCommandText();
      var param = new {
        Schema = GetSchema(tableName),
        TableName = GetTableName(tableName)
      };
      using (var reader = connection.ExecuteReader(commandText, param, transaction: transaction)) {
        var dbFields = new List<DbField>();
        while (reader.Read()) {
          dbFields.Add(ReaderToDbField(reader));
        }
        return dbFields;
      }
    }

    public async Task<IEnumerable<DbField>> GetFieldsAsync(IDbConnection connection, string tableName, IDbTransaction transaction = null, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      var commandText = GetCommandText();
      var param = new {
        Schema = GetSchema(tableName),
        TableName = GetTableName(tableName)
      };
      using (var reader = (DbDataReader)await connection.ExecuteReaderAsync(commandText, param, transaction: transaction, cancellationToken: cancellationToken)) {
        var dbFields = new List<DbField>();
        while (await reader.ReadAsync(cancellationToken)) {
          dbFields.Add(await ReaderToDbFieldAsync(reader, cancellationToken));
        }
        return dbFields;
      }
    }

    public const string GetIdentitySql = "SELECT IDENTITY_VAL_LOCAL() From SYSIBM/SYSDUMMY1;";

    public object GetScopeIdentity(IDbConnection connection, IDbTransaction transaction = null)
      => connection.ExecuteScalar(GetIdentitySql, transaction: transaction);

    public async Task<object> GetScopeIdentityAsync(IDbConnection connection, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
      => await connection.ExecuteScalarAsync(GetIdentitySql, transaction: transaction, cancellationToken: cancellationToken);

    private string GetCommandText() => $@"
With PK As(
  Select cst.constraint_Name  
  , cst.System_table_SCHEMA
  , cst.System_table_NAME 
  , col.Ordinal_position 
  , col.Column_Name   
  From QSYS2/SYSKEYCST col
  Join QSYS2/SYSCST    cst On(cst.constraint_SCHEMA, cst.constraint_NAME, cst.constraint_type) = (col.constraint_SCHEMA, col.constraint_NAME, 'PRIMARY KEY')
			  And cst.System_Table_Schema = @Schema And cst.SYSTEM_TABLE_NAME = @TableName
) 
SELECT 
  C.Column_Name As ColumnName
, CASE WHEN PK.Column_Name Is Null Then 0 Else 1 End AS IsPrimary
, CASE WHEN Is_Identity = 'YES' THEN 1 ELSE 0 END AS IsIdentity
, CASE WHEN IS_NULLABLE = 'YES' THEN 1 ELSE 0 END AS IsNullable
, DATA_TYPE AS ColumnType
, CHARACTER_MAXIMUM_LENGTH AS Size
, COALESCE(NUMERIC_PRECISION, DATETIME_PRECISION) AS Precision
, NUMERIC_SCALE AS Scale
, DATA_TYPE AS DatabaseType
FROM  QSYS2/SYSCOLUMNS C
Left Join PK On (PK.System_Table_Schema, PK.System_Table_Name, Pk.Column_Name)=(C.System_Table_Schema, C.System_Table_Name, C.Column_Name)
WHERE C.SYSTEM_TABLE_SCHEMA = @Schema And C.SYSTEM_TABLE_NAME = @TableName
ORDER BY C.Ordinal_Position;";

    private string GetSchema(string tableName) => tableName.IndexOf(m_dbSetting.SchemaSeparator) > 0
      ? tableName.Split(m_dbSetting.SchemaSeparator.ToCharArray())[0].AsUnquoted(true, m_dbSetting)
      : m_dbSetting.DefaultSchema;

    private string GetTableName(string tableName) => tableName.IndexOf(m_dbSetting.SchemaSeparator) > 0
      ? tableName.Split(m_dbSetting.SchemaSeparator.ToCharArray())[1].AsUnquoted(true, m_dbSetting)
      : tableName.AsUnquoted(true, m_dbSetting);

    private DbField ReaderToDbField(IDataReader reader) => new DbField(
     reader.GetString(0),
     reader.GetBoolean(1),
     reader.GetBoolean(2),
     reader.GetBoolean(3),
     DbTypeResolver.Resolve(reader.GetString(4)),
     reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
     reader.IsDBNull(6) ? (byte?)null : byte.Parse(reader.GetInt32(6).ToString()),
     reader.IsDBNull(7) ? (byte?)null : byte.Parse(reader.GetInt32(7).ToString()),
     reader.GetString(8));

    private async Task<DbField> ReaderToDbFieldAsync(DbDataReader reader, CancellationToken cancellationToken = default) => new DbField(
      await reader.GetFieldValueAsync<string>(0, cancellationToken),
      await reader.GetFieldValueAsync<bool>(1, cancellationToken),
      await reader.GetFieldValueAsync<bool>(2, cancellationToken),
      await reader.GetFieldValueAsync<bool>(3, cancellationToken),
      await reader.IsDBNullAsync(4, cancellationToken) ? DbTypeResolver.Resolve("text") : DbTypeResolver.Resolve(await reader.GetFieldValueAsync<string>(4, cancellationToken)),
      await reader.GetFieldValueAsync<int>(5, cancellationToken),
      await reader.GetFieldValueAsync<byte>(6, cancellationToken),
      await reader.GetFieldValueAsync<byte>(7, cancellationToken),
      await reader.GetFieldValueAsync<string>(4, cancellationToken));

  }
}