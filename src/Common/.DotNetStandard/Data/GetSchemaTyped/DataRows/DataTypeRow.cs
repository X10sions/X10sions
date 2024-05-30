using System.Data;

namespace Common.Data.GetSchemaTyped.DataRows;
public class DataTypeRow : ITypedDataRow {
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