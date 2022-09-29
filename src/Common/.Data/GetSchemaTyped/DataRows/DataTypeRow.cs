using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows;
public class DataTypeRow : ITypedDataRow {
  public DataTypeRow() { }
  public DataTypeRow(DataRow dataRow) {
    SetValues(dataRow);
  }

  public void SetValues(DataRow dataRow) {
    TypeName = dataRow.Field<string>(DbMetaDataColumnNames.TypeName);
    ProviderDbType = dataRow.Field<int?>(DbMetaDataColumnNames.ProviderDbType);
    ColumnSize = dataRow.Field<long?>(DbMetaDataColumnNames.ColumnSize);
    CreateFormat = dataRow.Field<string>(DbMetaDataColumnNames.CreateFormat);
    CreateParameters = dataRow.Field<string>(DbMetaDataColumnNames.CreateParameters);
    DataType = dataRow.Field<string>(DbMetaDataColumnNames.DataType);
    IsAutoIncrementable = dataRow.Field<bool?>(DbMetaDataColumnNames.IsAutoIncrementable);
    IsBestMatch = dataRow.Field<bool?>(DbMetaDataColumnNames.IsBestMatch);
    IsCaseSensitive = dataRow.Field<bool?>(DbMetaDataColumnNames.IsCaseSensitive);
    IsFixedLength = dataRow.Field<bool?>(DbMetaDataColumnNames.IsFixedLength);
    IsFixedPrecisionScale = dataRow.Field<bool?>(DbMetaDataColumnNames.IsFixedPrecisionScale);
    IsLong = dataRow.Field<bool?>(DbMetaDataColumnNames.IsLong);
    IsNullable = dataRow.Field<bool?>(DbMetaDataColumnNames.IsNullable);
    IsSearchable = dataRow.Field<bool?>(DbMetaDataColumnNames.IsSearchable);
    IsSearchableWithLike = dataRow.Field<bool?>(DbMetaDataColumnNames.IsSearchableWithLike);
    IsUnsigned = dataRow.Field<bool?>(DbMetaDataColumnNames.IsUnsigned);
    MaximumScale = dataRow.Field<short?>(DbMetaDataColumnNames.MaximumScale);
    MinimumScale = dataRow.Field<short?>(DbMetaDataColumnNames.MinimumScale);
    IsConcurrencyType = dataRow.Field<bool?>(DbMetaDataColumnNames.IsConcurrencyType);
    IsLiteralSupported = dataRow.Field<bool?>(DbMetaDataColumnNames.IsLiteralSupported);
    LiteralPrefix = dataRow.Field<string?>(DbMetaDataColumnNames.LiteralPrefix);
    LiteralSuffix = dataRow.Field<string?>(DbMetaDataColumnNames.LiteralSuffix);

    // Others
    DbType = dataRow.Field<DbType?>(nameof(DbType));
    MaximumVersion = dataRow.Field<string?>(nameof(MaximumVersion));
    MinimumVersion = dataRow.Field<string?>(nameof(MinimumVersion));
    INTERVAL_PRECISION = dataRow.Field<short?>(nameof(INTERVAL_PRECISION));
    LOCAL_TYPE_NAME = dataRow.Field<string?>(nameof(LOCAL_TYPE_NAME));
    NativeDataType = dataRow.Field<short?>(nameof(NativeDataType));
    OID = dataRow.Field<uint?>(nameof(OID));
    NUM_PREC_RADIX = dataRow.Field<int?>(nameof(NUM_PREC_RADIX));
    PROVIDER_TYPE = dataRow.Field<int?>(nameof(PROVIDER_TYPE));
    PROVIDER_TYPE_NAME = dataRow.Field<string?>(nameof(PROVIDER_TYPE_NAME));
    INTERVAL_PRECISION = dataRow.Field<short?>(nameof(SEARCHABLE));
    SQL_TYPE = dataRow.Field<short?>(nameof(SQL_TYPE));
    // Alias 

    //foreach (var columnName in new[] { "COLUMN_SIZE" }) {
    //  dic[columnName] = dataRow => ColumnSize = dataRow.Field<int?>(nameof(DbType));
    //}

    IsAutoIncrementable = GetBoolean(dataRow.Field<short?>("AUTO_UNIQUE_VALUE"));
    IsCaseSensitive = GetBoolean(dataRow.Field<short?>("CASE_SENSITIVE"));
    ColumnSize = dataRow.Field<int?>("COLUMN_SIZE");
    CreateParameters = dataRow.Field<string>("CREATE_PARAMS");
    IsFixedPrecisionScale = GetBoolean(dataRow.Field<short?>("FIXED_PREC_SCALE"));
    DataType = dataRow.Field<string>("FRAMEWORK_TYPE");
    IsFixedPrecisionScale = dataRow.Field<bool?>("IsFixedPrecisionAndScale");
    LiteralPrefix = dataRow.Field<string?>("LITERAL_PREFIX");
    LiteralSuffix = dataRow.Field<string?>("LITERAL_SUFFIX");
    MaximumScale = dataRow.Field<short?>("MAXIMUM_SCALE");
    MinimumScale = dataRow.Field<short?>("MINIMUM_SCALE");
    IsNullable = GetBoolean(dataRow.Field<short?>("NULLABLE"));
    SQL_TYPE = dataRow.Field<short?>("SQLType");
    TypeName = dataRow.Field<string>("SQL_TYPE_NAME");
    IsUnsigned = GetBoolean(dataRow.Field<short?>("UNSIGNED_ATTRIBUTE"));
  }

  bool? GetBoolean(short? value) => value switch {
    null => null,
    -1 => true,
    0 => false,
    1 => true,
    _ => throw new NotImplementedException($"{value}")
  };

  #region Columns

  /// <summary>
  /// DB2: SQL_TYPE_NAME
  /// </summary>
  public string TypeName { get; set; } = string.Empty;
  public string CreateFormat { get; set; } = string.Empty;
  /// <summary>
  /// DB2: CREATE_PARAMS
  /// </summary>
  public string CreateParameters { get; set; } = string.Empty;
  /// <summary>
  /// DB2: FRAMEWORK_TYPE 
  /// </summary>
  public string DataType { get; set; } = string.Empty;
  public string? LiteralPrefix { get; set; }
  public string? LiteralSuffix { get; set; }

  public bool? IsAutoIncrementable { get; set; }
  public bool? IsBestMatch { get; set; }
  public bool? IsCaseSensitive { get; set; }
  public bool? IsFixedLength { get; set; }
  public bool? IsFixedPrecisionScale { get; set; }
  public bool? IsLong { get; set; }
  public bool? IsNullable { get; set; }
  public bool? IsSearchable { get; set; }
  public bool? IsSearchableWithLike { get; set; }
  public bool? IsUnsigned { get; set; }
  public bool? IsConcurrencyType { get; set; }
  public bool? IsLiteralSupported { get; set; }

  /// <summary>
  /// DB2: PROVIDER_TYPE
  /// </summary>
  public int? ProviderDbType { get; set; }
  public long? ColumnSize { get; set; }
  public short? MaximumScale { get; set; }
  public short? MinimumScale { get; set; }
  #endregion

  public DbType? DbType { get; set; }
  public string? MaximumVersion { get; set; }
  public string? MinimumVersion { get; set; }

  public short? SQL_TYPE { get; set; }
  public string? LOCAL_TYPE_NAME { get; set; }
  public int? NUM_PREC_RADIX { get; set; }
  public short? INTERVAL_PRECISION { get; set; }
  public short? SEARCHABLE { get; set; }
  public int? PROVIDER_TYPE { get; set; }
  public string? PROVIDER_TYPE_NAME { get; set; }
  /// <summary>
  /// PostgreSql
  /// </summary>
  public short? NativeDataType { get; set; }
  /// <summary>
  /// PostgreSql
  /// </summary>
  public uint? OID { get; set; }

}