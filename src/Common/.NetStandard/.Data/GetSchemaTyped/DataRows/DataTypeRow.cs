using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows;
public class DataTypeRow : BaseTypedDataRow {
  public DataTypeRow() { }
  public DataTypeRow(DataRow dataRow) : base(dataRow) { }

  public override Dictionary<string, Action<DataRow>> GetColumnSetValueDictionary() {
    var dic = new Dictionary<string, Action<DataRow>>();
    dic[DbMetaDataColumnNames.TypeName] = dataRow => TypeName = dataRow.Field<string>(DbMetaDataColumnNames.TypeName);
    dic[DbMetaDataColumnNames.ProviderDbType] = dataRow => ProviderDbType = dataRow.Field<int?>(DbMetaDataColumnNames.ProviderDbType);
    dic[DbMetaDataColumnNames.ColumnSize] = dataRow => ColumnSize = dataRow.Field<long?>(DbMetaDataColumnNames.ColumnSize);
    dic[DbMetaDataColumnNames.CreateFormat] = dataRow => CreateFormat = dataRow.Field<string>(DbMetaDataColumnNames.CreateFormat);
    dic[DbMetaDataColumnNames.CreateParameters] = dataRow => CreateParameters = dataRow.Field<string>(DbMetaDataColumnNames.CreateParameters);
    dic[DbMetaDataColumnNames.DataType] = dataRow => DataType = dataRow.Field<string>(DbMetaDataColumnNames.DataType);
    dic[DbMetaDataColumnNames.IsAutoIncrementable] = dataRow => IsAutoIncrementable = dataRow.Field<bool?>(DbMetaDataColumnNames.IsAutoIncrementable);
    dic[DbMetaDataColumnNames.IsBestMatch] = dataRow => IsBestMatch = dataRow.Field<bool?>(DbMetaDataColumnNames.IsBestMatch);
    dic[DbMetaDataColumnNames.IsCaseSensitive] = dataRow => IsCaseSensitive = dataRow.Field<bool?>(DbMetaDataColumnNames.IsCaseSensitive);
    dic[DbMetaDataColumnNames.IsFixedLength] = dataRow => IsFixedLength = dataRow.Field<bool?>(DbMetaDataColumnNames.IsFixedLength);
    dic[DbMetaDataColumnNames.IsFixedPrecisionScale] = dataRow => IsFixedPrecisionScale = dataRow.Field<bool?>(DbMetaDataColumnNames.IsFixedPrecisionScale);
    dic[DbMetaDataColumnNames.IsLong] = dataRow => IsLong = dataRow.Field<bool?>(DbMetaDataColumnNames.IsLong);
    dic[DbMetaDataColumnNames.IsNullable] = dataRow => IsNullable = dataRow.Field<bool?>(DbMetaDataColumnNames.IsNullable);
    dic[DbMetaDataColumnNames.IsSearchable] = dataRow => IsSearchable = dataRow.Field<bool?>(DbMetaDataColumnNames.IsSearchable);
    dic[DbMetaDataColumnNames.IsSearchableWithLike] = dataRow => IsSearchableWithLike = dataRow.Field<bool?>(DbMetaDataColumnNames.IsSearchable);
    dic[DbMetaDataColumnNames.IsUnsigned] = dataRow => IsUnsigned = dataRow.Field<bool?>(DbMetaDataColumnNames.IsUnsigned);
    dic[DbMetaDataColumnNames.MaximumScale] = dataRow => MaximumScale = dataRow.Field<short?>(DbMetaDataColumnNames.MaximumScale);
    dic[DbMetaDataColumnNames.MinimumScale] = dataRow => MinimumScale = dataRow.Field<short?>(DbMetaDataColumnNames.MinimumScale);
    dic[DbMetaDataColumnNames.IsConcurrencyType] = dataRow => IsConcurrencyType = dataRow.Field<bool?>(DbMetaDataColumnNames.IsConcurrencyType);
    dic[DbMetaDataColumnNames.IsLiteralSupported] = dataRow => IsLiteralSupported = dataRow.Field<bool?>(DbMetaDataColumnNames.IsLiteralSupported);
    dic[DbMetaDataColumnNames.LiteralPrefix] = dataRow => LiteralPrefix = dataRow.Field<string?>(DbMetaDataColumnNames.LiteralPrefix);
    dic[DbMetaDataColumnNames.LiteralSuffix] = dataRow => LiteralSuffix = dataRow.Field<string?>(DbMetaDataColumnNames.LiteralSuffix);
    // Others
    dic[nameof(DbType)] = dataRow => DbType = dataRow.Field<DbType?>(nameof(DbType));
    dic[nameof(MaximumVersion)] = dataRow => MaximumVersion = dataRow.Field<string?>(nameof(MaximumVersion));
    dic[nameof(MinimumVersion)] = dataRow => MinimumVersion = dataRow.Field<string?>(nameof(MinimumVersion));
    dic[nameof(SQL_TYPE_NAME)] = dataRow => SQL_TYPE_NAME = dataRow.Field<string?>(nameof(SQL_TYPE_NAME));
    dic[nameof(SQL_TYPE)] = dataRow => SQL_TYPE = dataRow.Field<short?>(nameof(SQL_TYPE));
    dic[nameof(CREATE_PARAMS)] = dataRow => CREATE_PARAMS = dataRow.Field<string?>(nameof(CREATE_PARAMS));
    dic[nameof(LOCAL_TYPE_NAME)] = dataRow => LOCAL_TYPE_NAME = dataRow.Field<string?>(nameof(LOCAL_TYPE_NAME));
    dic[nameof(PROVIDER_TYPE)] = dataRow => PROVIDER_TYPE = dataRow.Field<int?>(nameof(PROVIDER_TYPE));
    dic[nameof(PROVIDER_TYPE_NAME)] = dataRow => PROVIDER_TYPE_NAME = dataRow.Field<string?>(nameof(PROVIDER_TYPE_NAME));
    dic[nameof(FRAMEWORK_TYPE)] = dataRow => FRAMEWORK_TYPE = dataRow.Field<string?>(nameof(FRAMEWORK_TYPE));
    dic[nameof(NUM_PREC_RADIX)] = dataRow => NUM_PREC_RADIX = dataRow.Field<int?>(nameof(NUM_PREC_RADIX));
    dic[nameof(INTERVAL_PRECISION)] = dataRow => INTERVAL_PRECISION = dataRow.Field<short?>(nameof(INTERVAL_PRECISION));
    // Alias 

    //foreach (var columnName in new[] { "COLUMN_SIZE" }) {
    //  dic[columnName] = dataRow => ColumnSize = dataRow.Field<int?>(nameof(DbType));
    //}


    dic["COLUMN_SIZE"] = dataRow => ColumnSize = dataRow.Field<int?>("COLUMN_SIZE");
    dic["LITERAL_PREFIX"] = dataRow => LiteralPrefix = dataRow.Field<string?>("LITERAL_PREFIX");
    dic["LITERAL_SUFFIX"] = dataRow => LiteralSuffix = dataRow.Field<string?>("LITERAL_SUFFIX");
    dic["NULLABLE"] = dataRow => IsNullable = GetBoolean(dataRow.Field<short?>("NULLABLE"));
    dic["CASE_SENSITIVE"] = dataRow => IsCaseSensitive = GetBoolean(dataRow.Field<short?>("CASE_SENSITIVE"));
    dic["SEARCHABLE"] = dataRow => IsSearchable = GetBoolean(dataRow.Field<short?>("SEARCHABLE"));
    dic["UNSIGNED_ATTRIBUTE"] = dataRow => IsUnsigned = GetBoolean(dataRow.Field<short?>("UNSIGNED_ATTRIBUTE"));
    dic["FIXED_PREC_SCALE"] = dataRow => IsFixedPrecisionScale = GetBoolean(dataRow.Field<short?>("FIXED_PREC_SCALE"));
    dic["AUTO_UNIQUE_VALUE"] = dataRow => IsAutoIncrementable = GetBoolean(dataRow.Field<short?>("AUTO_UNIQUE_VALUE"));
    dic["MINIMUM_SCALE"] = dataRow => MinimumScale = dataRow.Field<short?>("MINIMUM_SCALE");
    dic["MAXIMUM_SCALE"] = dataRow =>MaximumScale= dataRow.Field<short?>("MAXIMUM_SCALE");
    return dic;
  }

  bool? GetBoolean(short? value) => value switch {
    null => null,
    -1 => true,
    0 => false,
    1 => true,
    _ => throw new NotImplementedException($"{value}")
  };

  #region Columns

  public string TypeName { get; set; } = string.Empty;
  public string CreateFormat { get; set; } = string.Empty;
  public string CreateParameters { get; set; } = string.Empty;
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

  public int? ProviderDbType { get; set; }
  public long? ColumnSize { get; set; }
  public short? MaximumScale { get; set; }
  public short? MinimumScale { get; set; }
  #endregion

  public DbType? DbType { get; set; }
  public string? MaximumVersion { get; set; }
  public string? MinimumVersion { get; set; }

  public string? SQL_TYPE_NAME { get; set; }
  public short? SQL_TYPE { get; set; }
  public string? CREATE_PARAMS { get; set; }
  public string? LOCAL_TYPE_NAME { get; set; }
  public int? NUM_PREC_RADIX { get; set; }
  public short? INTERVAL_PRECISION { get; set; }
  public int? PROVIDER_TYPE { get; set; }
  public string? PROVIDER_TYPE_NAME { get; set; }
  public string? FRAMEWORK_TYPE { get; set; }

}