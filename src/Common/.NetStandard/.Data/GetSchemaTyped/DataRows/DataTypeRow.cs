using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class DataTypeRow : _BaseTypedDataRow {
    // public DataTypeRow(DbConnection dbConnection) : base(dbConnection, DbMetaDataCollectionNames.DataTypes) { }
    public DataTypeRow(DataRow row) : base(row) { }
    public string TypeName => row.Field<string>(DbMetaDataColumnNames.TypeName);
    public string CreateFormat => row.Field<string>(DbMetaDataColumnNames.CreateFormat);
    public string CreateParameters => row.Field<string>(DbMetaDataColumnNames.CreateParameters);
    public string DataType => row.Field<string>(DbMetaDataColumnNames.DataType);
    public string LiteralPrefix => row.Field<string>(DbMetaDataColumnNames.LiteralPrefix);
    public string LiteralSuffix => row.Field<string>(DbMetaDataColumnNames.LiteralSuffix);

    public bool? IsAutoIncrementable => row.Field<bool?>(DbMetaDataColumnNames.IsAutoIncrementable);
    public bool? IsBestMatch => row.Field<bool?>(DbMetaDataColumnNames.IsBestMatch);
    public bool? IsCaseSensitive => row.Field<bool?>(DbMetaDataColumnNames.IsCaseSensitive);
    public bool? IsFixedLength => row.Field<bool?>(DbMetaDataColumnNames.IsFixedLength);
    public bool? IsFixedPrecisionScale => row.Field<bool?>(DbMetaDataColumnNames.IsFixedPrecisionScale);
    public bool? IsLong => row.Field<bool?>(DbMetaDataColumnNames.IsLong);
    public bool? IsNullable => row.Field<bool?>(DbMetaDataColumnNames.IsNullable);
    public bool? IsSearchable => row.Field<bool?>(DbMetaDataColumnNames.IsSearchable);
    public bool? IsSearchableWithLike => row.Field<bool?>(DbMetaDataColumnNames.IsSearchable);
    public bool? IsUnsigned => row.Field<bool?>(DbMetaDataColumnNames.IsUnsigned);
    public bool? IsConcurrencyType => row.Field<bool?>(DbMetaDataColumnNames.IsConcurrencyType);
    public bool? IsLiteralSupported => row.Field<bool?>(DbMetaDataColumnNames.IsLiteralSupported);

    public int? ProviderDbType => row.Field<int?>(DbMetaDataColumnNames.ProviderDbType);
    public int? ColumnSize => row.Field<int?>(DbMetaDataColumnNames.ColumnSize);
    public int? MaximumScale => row.Field<int?>(DbMetaDataColumnNames.MaximumScale);
    public int? MinimumScale => row.Field<int?>(DbMetaDataColumnNames.MinimumScale);

    #region OdbcConnection
    //public int? SqlType { get; }
    #endregion

    //    public string NativeDataType { get => (string)_DataRow[nameof(NativeDataType)]; set => _DataRow[nameof(NativeDataType)] = value; }
  }


}