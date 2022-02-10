using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class DataTypeRow : BaseTypedDataRow {
    // public DataTypeRow(DbConnection dbConnection) : base(dbConnection, DbMetaDataCollectionNames.DataTypes) { }
    public DataTypeRow(DataRow row) : base(row) { }
    public string TypeName => DataRow.Field<string>(DbMetaDataColumnNames.TypeName);
    public string CreateFormat => DataRow.Field<string>(DbMetaDataColumnNames.CreateFormat);
    public string CreateParameters => DataRow.Field<string>(DbMetaDataColumnNames.CreateParameters);
    public string DataType => DataRow.Field<string>(DbMetaDataColumnNames.DataType);
    public string LiteralPrefix => DataRow.Field<string>(DbMetaDataColumnNames.LiteralPrefix);
    public string LiteralSuffix => DataRow.Field<string>(DbMetaDataColumnNames.LiteralSuffix);

    public bool? IsAutoIncrementable => DataRow.Field<bool?>(DbMetaDataColumnNames.IsAutoIncrementable);
    public bool? IsBestMatch => DataRow.Field<bool?>(DbMetaDataColumnNames.IsBestMatch);
    public bool? IsCaseSensitive => DataRow.Field<bool?>(DbMetaDataColumnNames.IsCaseSensitive);
    public bool? IsFixedLength => DataRow.Field<bool?>(DbMetaDataColumnNames.IsFixedLength);
    public bool? IsFixedPrecisionScale => DataRow.Field<bool?>(DbMetaDataColumnNames.IsFixedPrecisionScale);
    public bool? IsLong => DataRow.Field<bool?>(DbMetaDataColumnNames.IsLong);
    public bool? IsNullable => DataRow.Field<bool?>(DbMetaDataColumnNames.IsNullable);
    public bool? IsSearchable => DataRow.Field<bool?>(DbMetaDataColumnNames.IsSearchable);
    public bool? IsSearchableWithLike => DataRow.Field<bool?>(DbMetaDataColumnNames.IsSearchable);
    public bool? IsUnsigned => DataRow.Field<bool?>(DbMetaDataColumnNames.IsUnsigned);
    public bool? IsConcurrencyType => DataRow.Field<bool?>(DbMetaDataColumnNames.IsConcurrencyType);
    public bool? IsLiteralSupported => DataRow.Field<bool?>(DbMetaDataColumnNames.IsLiteralSupported);

    public int? ProviderDbType => DataRow.Field<int?>(DbMetaDataColumnNames.ProviderDbType);
    public int? ColumnSize => DataRow.Field<int?>(DbMetaDataColumnNames.ColumnSize);
    public int? MaximumScale => DataRow.Field<int?>(DbMetaDataColumnNames.MaximumScale);
    public int? MinimumScale => DataRow.Field<int?>(DbMetaDataColumnNames.MinimumScale);

    #region OdbcConnection
    //public int? SqlType { get; }
    #endregion

    //    public string NativeDataType { get => (string)_DataRow[nameof(NativeDataType)]; set => _DataRow[nameof(NativeDataType)] = value; }
  }


}