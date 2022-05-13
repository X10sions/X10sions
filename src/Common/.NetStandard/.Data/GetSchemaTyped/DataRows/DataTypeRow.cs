using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class DataTypeRow : BaseTypedDataRow {
    // public DataTypeRow(DbConnection dbConnection) : base(dbConnection, DbMetaDataCollectionNames.DataTypes) { }
    public DataTypeRow() {     }
    public DataTypeRow(DataRow dataRow) :base(dataRow) {    }
    public override void SetValues(DataRow dataRow) {
      TypeName = dataRow.Field<string>(DbMetaDataColumnNames.TypeName);
      CreateFormat = dataRow.Field<string>(DbMetaDataColumnNames.CreateFormat);
      CreateParameters = dataRow.Field<string>(DbMetaDataColumnNames.CreateParameters);
      DataType = dataRow.Field<string>(DbMetaDataColumnNames.DataType);
      LiteralPrefix = dataRow.Field<string>(DbMetaDataColumnNames.LiteralPrefix);
      LiteralSuffix = dataRow.Field<string>(DbMetaDataColumnNames.LiteralSuffix);

      IsAutoIncrementable = dataRow.Field<bool?>(DbMetaDataColumnNames.IsAutoIncrementable);
      IsBestMatch = dataRow.Field<bool?>(DbMetaDataColumnNames.IsBestMatch);
      IsCaseSensitive = dataRow.Field<bool?>(DbMetaDataColumnNames.IsCaseSensitive);
      IsFixedLength = dataRow.Field<bool?>(DbMetaDataColumnNames.IsFixedLength);
      IsFixedPrecisionScale = dataRow.Field<bool?>(DbMetaDataColumnNames.IsFixedPrecisionScale);
      IsLong = dataRow.Field<bool?>(DbMetaDataColumnNames.IsLong);
      IsNullable = dataRow.Field<bool?>(DbMetaDataColumnNames.IsNullable);
      IsSearchable = dataRow.Field<bool?>(DbMetaDataColumnNames.IsSearchable);
      IsSearchableWithLike = dataRow.Field<bool?>(DbMetaDataColumnNames.IsSearchable);
      IsUnsigned = dataRow.Field<bool?>(DbMetaDataColumnNames.IsUnsigned);
      IsConcurrencyType = dataRow.Field<bool?>(DbMetaDataColumnNames.IsConcurrencyType);
      IsLiteralSupported = dataRow.Field<bool?>(DbMetaDataColumnNames.IsLiteralSupported);

      ProviderDbType = dataRow.Field<int?>(DbMetaDataColumnNames.ProviderDbType);
      ColumnSize = dataRow.Field<int?>(DbMetaDataColumnNames.ColumnSize);
      MaximumScale = dataRow.Field<int?>(DbMetaDataColumnNames.MaximumScale);
      MinimumScale = dataRow.Field<int?>(DbMetaDataColumnNames.MinimumScale);
    }
    public string TypeName { get; set; } = string.Empty;
    public string CreateFormat { get; set; } = string.Empty;
    public string CreateParameters { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public string LiteralPrefix { get; set; } = string.Empty;
    public string LiteralSuffix { get; set; } = string.Empty;

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
    public int? ColumnSize { get; set; }
    public int? MaximumScale { get; set; }
    public int? MinimumScale { get; set; }

    #region OdbcConnection
    //public int? SqlType { get; }
    #endregion

    //    public string NativeDataType { get => (string)_DataRow[nameof(NativeDataType)]; set => _DataRow[nameof(NativeDataType)] = value; }
  }


}