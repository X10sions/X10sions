using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows;
public static class Extensions {
  public static ITypedDataRow SetValues(this ITypedDataRow x, DataRow dataRow) {
    if (x is ColumnRow cr) return cr.SetValues(dataRow);
    if (x is DataSourceInformationRow dsir) return dsir.SetValues(dataRow);
    if (x is DataTypeRow dtr) return dtr.SetValues(dataRow);
    if (x is IndexRow ir) return ir.SetValues(dataRow);
    if (x is MetaDataCollectionRow mdcr) return mdcr.SetValues(dataRow);
    if (x is ProcedureColumnRow pcr) return pcr.SetValues(dataRow);
    if (x is ProcedureParameterRow ppr) return ppr.SetValues(dataRow);
    if (x is ProcedureRow pr) return pr.SetValues(dataRow);
    if (x is ReservedWordRow rwr) return rwr.SetValues(dataRow);
    if (x is RestrictionRow rr) return rr.SetValues(dataRow);
    if (x is TableRow tr) return tr.SetValues(dataRow);
    if (x is ViewRow vr) return vr.SetValues(dataRow);
    throw new NotImplementedException();
  }

  public static ColumnRow SetValues(this ColumnRow x, DataRow dataRow) {
    x.TABLE_CAT = dataRow.Field<string?>(nameof(x.TABLE_CAT));
    x.TABLE_SCHEM = dataRow.Field<string?>(nameof(x.TABLE_SCHEM));
    x.TABLE_NAME = dataRow.Field<string?>(nameof(x.TABLE_NAME));
    x.COLUMN_NAME = dataRow.Field<string>(nameof(x.COLUMN_NAME));
    x.DATA_TYPE = dataRow.Field<short?>(nameof(x.DATA_TYPE));
    x.TYPE_NAME = dataRow.Field<string>(nameof(x.TYPE_NAME));
    x.COLUMN_SIZE = dataRow.Field<int?>(nameof(x.COLUMN_SIZE));
    x.BUFFER_LENGTH = dataRow.Field<int?>(nameof(x.BUFFER_LENGTH));
    x.DECIMAL_DIGITS = dataRow.Field<short?>(nameof(x.DECIMAL_DIGITS));
    x.NUM_PREC_RADIX = dataRow.Field<short?>(nameof(x.NUM_PREC_RADIX));
    x.NULLABLE = dataRow.Field<short?>(nameof(x.NULLABLE));
    x.REMARKS = dataRow.Field<string>(nameof(x.REMARKS));
    x.COLUMN_DEF = dataRow.Field<string>(nameof(x.COLUMN_DEF));
    x.SQL_DATA_TYPE = dataRow.Field<short?>(nameof(x.SQL_DATA_TYPE));
    x.SQL_DATETIME_SUB = dataRow.Field<short?>(nameof(x.SQL_DATETIME_SUB));
    x.CHAR_OCTET_LENGTH = dataRow.Field<int?>(nameof(x.CHAR_OCTET_LENGTH));
    x.ORDINAL_POSITION = dataRow.Field<int?>(nameof(x.ORDINAL_POSITION));
    x.IS_NULLABLE = dataRow.Field<string>(nameof(x.IS_NULLABLE));
    return x;
  }

  public static IndexRow SetValues(this IndexRow x, DataRow dataRow) {
    x.TABLE_CAT = dataRow.Field<string>(nameof(x.TABLE_CAT));
    x.TABLE_SCHEM = dataRow.Field<string>(nameof(x.TABLE_SCHEM));
    x.TABLE_NAME = dataRow.Field<string>(nameof(x.TABLE_NAME));
    x.NON_UNIQUE = dataRow.Field<int?>(nameof(x.NON_UNIQUE));
    x.INDEX_QUALIFIER = dataRow.Field<string>(nameof(x.INDEX_QUALIFIER));
    x.INDEX_NAME = dataRow.Field<string>(nameof(x.INDEX_NAME));
    x.TYPE = dataRow.Field<int?>(nameof(x.TYPE));
    x.ORDINAL_POSITION = dataRow.Field<int?>(nameof(x.ORDINAL_POSITION));
    x.COLUMN_NAME = dataRow.Field<string>(nameof(x.COLUMN_NAME));
    x.ASC_OR_DESC = dataRow.Field<string>(nameof(x.ASC_OR_DESC));
    x.CARDINALITY = dataRow.Field<int?>(nameof(x.CARDINALITY));
    x.PAGES = dataRow.Field<int?>(nameof(x.PAGES));
    x.FILTER_CONDITION = dataRow.Field<string>(nameof(x.FILTER_CONDITION));
    return x;
  }

  public static ProcedureColumnRow SetValues(this ProcedureColumnRow x, DataRow dataRow) {
    x.BUFFER_LENGTH = dataRow.Field<int?>(nameof(x.BUFFER_LENGTH));
    x.CHAR_OCTET_LENGTH = dataRow.Field<int?>(nameof(x.CHAR_OCTET_LENGTH));
    x.COLUMN_SIZE = dataRow.Field<int?>(nameof(x.COLUMN_SIZE));
    x.ORDINAL_POSITION = dataRow.Field<int?>(nameof(x.ORDINAL_POSITION));
    x.COLUMN_TYPE = dataRow.Field<short?>(nameof(x.COLUMN_TYPE));
    x.DATA_TYPE = dataRow.Field<short?>(nameof(x.DATA_TYPE));
    x.DECIMAL_DIGITS = dataRow.Field<short?>(nameof(x.DECIMAL_DIGITS));
    x.NULLABLE = dataRow.Field<short?>(nameof(x.NULLABLE));
    x.NUM_PREC_RADIX = dataRow.Field<short?>(nameof(x.NUM_PREC_RADIX));
    x.SQL_DATA_TYPE = dataRow.Field<short?>(nameof(x.SQL_DATA_TYPE));
    x.SQL_DATETIME_SUB = dataRow.Field<short?>(nameof(x.SQL_DATETIME_SUB));
    x.COLUMN_DEF = dataRow.Field<string>(nameof(x.COLUMN_DEF));
    x.COLUMN_NAME = dataRow.Field<string>(nameof(x.COLUMN_NAME));
    x.IS_NULLABLE = dataRow.Field<string>(nameof(x.IS_NULLABLE));
    x.PROCEDURE_CAT = dataRow.Field<string>(nameof(x.PROCEDURE_CAT));
    x.PROCEDURE_NAME = dataRow.Field<string>(nameof(x.PROCEDURE_NAME));
    x.PROCEDURE_SCHEM = dataRow.Field<string>(nameof(x.PROCEDURE_SCHEM));
    x.REMARKS = dataRow.Field<string>(nameof(x.REMARKS));
    x.TYPE_NAME = dataRow.Field<string>(nameof(x.TYPE_NAME));
    return x;
  }

  public static ProcedureParameterRow SetValues(this ProcedureParameterRow x, DataRow dataRow) {
    x.BUFFER_LENGTH = dataRow.Field<int?>(nameof(x.BUFFER_LENGTH));
    x.CHAR_OCTET_LENGTH = dataRow.Field<int?>(nameof(x.CHAR_OCTET_LENGTH));
    x.COLUMN_SIZE = dataRow.Field<int?>(nameof(x.COLUMN_SIZE));
    x.ORDINAL_POSITION = dataRow.Field<int?>(nameof(x.ORDINAL_POSITION));
    x.COLUMN_TYPE = dataRow.Field<short?>(nameof(x.COLUMN_TYPE));
    x.DATA_TYPE = dataRow.Field<short?>(nameof(x.DATA_TYPE));
    x.DECIMAL_DIGITS = dataRow.Field<short?>(nameof(x.DECIMAL_DIGITS));
    x.NULLABLE = dataRow.Field<short?>(nameof(x.NULLABLE));
    x.NUM_PREC_RADIX = dataRow.Field<short?>(nameof(x.NUM_PREC_RADIX));
    x.SQL_DATA_TYPE = dataRow.Field<short?>(nameof(x.SQL_DATA_TYPE));
    x.SQL_DATETIME_SUB = dataRow.Field<short?>(nameof(x.SQL_DATETIME_SUB));
    x.COLUMN_DEF = dataRow.Field<string>(nameof(x.COLUMN_DEF));
    x.COLUMN_NAME = dataRow.Field<string>(nameof(x.COLUMN_NAME));
    x.IS_NULLABLE = dataRow.Field<string>(nameof(x.IS_NULLABLE));
    x.PROCEDURE_CAT = dataRow.Field<string>(nameof(x.PROCEDURE_CAT));
    x.PROCEDURE_NAME = dataRow.Field<string>(nameof(x.PROCEDURE_NAME));
    x.PROCEDURE_SCHEM = dataRow.Field<string>(nameof(x.PROCEDURE_SCHEM));
    x.REMARKS = dataRow.Field<string>(nameof(x.REMARKS));
    x.TYPE_NAME = dataRow.Field<string>(nameof(x.TYPE_NAME));
    return x;
  }

  public static ProcedureRow SetValues(this ProcedureRow x, DataRow dataRow) {
    x.PROCEDURE_CAT = dataRow.Field<string>(nameof(x.PROCEDURE_CAT));
    x.PROCEDURE_SCHEM = dataRow.Field<string>(nameof(x.PROCEDURE_SCHEM));
    x.PROCEDURE_NAME = dataRow.Field<string>(nameof(x.PROCEDURE_NAME));
    x.NUM_INPUT_PARAMS = dataRow.Field<short?>(nameof(x.NUM_INPUT_PARAMS));
    x.NUM_OUTPUT_PARAMS = dataRow.Field<short?>(nameof(x.NUM_OUTPUT_PARAMS));
    x.NUM_RESULT_SETS = dataRow.Field<short?>(nameof(x.NUM_RESULT_SETS));
    x.REMARKS = dataRow.Field<string>(nameof(x.REMARKS));
    x.PROCEDURE_TYPE = dataRow.Field<short?>(nameof(x.PROCEDURE_TYPE));
    return x;
  }

  public static TableRow SetValues(this TableRow x, DataRow dataRow) {
    x.TABLE_CAT = dataRow.Field<string?>(nameof(x.TABLE_CAT));
    x.TABLE_SCHEM = dataRow.Field<string?>(nameof(x.TABLE_SCHEM));
    x.TABLE_NAME = dataRow.Field<string?>(nameof(x.TABLE_NAME));
    x.TABLE_TYPE = dataRow.Field<string?>(nameof(x.TABLE_TYPE));
    x.REMARKS = dataRow.Field<string?>(nameof(x.REMARKS));
    return x;
  }

  public static ViewRow SetValues(this ViewRow x, DataRow dataRow) {
    x.TABLE_CAT = dataRow.Field<string>(nameof(x.TABLE_CAT));
    x.TABLE_SCHEM = dataRow.Field<string>(nameof(x.TABLE_SCHEM));
    x.TABLE_NAME = dataRow.Field<string>(nameof(x.TABLE_NAME));
    x.TABLE_TYPE = dataRow.Field<string>(nameof(x.TABLE_TYPE));
    x.REMARKS = dataRow.Field<string>(nameof(x.REMARKS));
    return x;
  }

  public static DataSourceInformationRow SetValues(this DataSourceInformationRow x, DataRow dataRow) {
    x.CompositeIdentifierSeparatorPattern = dataRow.Field<string?>(DbMetaDataColumnNames.CompositeIdentifierSeparatorPattern);
    x.DataSourceProductName = dataRow.Field<string?>(DbMetaDataColumnNames.DataSourceProductName);
    x.DataSourceProductVersion = dataRow.Field<string?>(DbMetaDataColumnNames.DataSourceProductVersion);
    x.DataSourceProductVersionNormalized = dataRow.Field<string?>(DbMetaDataColumnNames.DataSourceProductVersionNormalized);
    x.GroupByBehavior = dataRow.Field<GroupByBehavior>(DbMetaDataColumnNames.GroupByBehavior);
    x.IdentifierPattern = dataRow.Field<string?>(DbMetaDataColumnNames.IdentifierPattern);
    x.IdentifierCase = dataRow.Field<IdentifierCase>(DbMetaDataColumnNames.IdentifierCase);
    x.OrderByColumnsInSelect = dataRow.Field<bool?>(DbMetaDataColumnNames.OrderByColumnsInSelect);
    x.ParameterMarkerFormat = dataRow.Field<string?>(DbMetaDataColumnNames.ParameterMarkerFormat);
    x.ParameterMarkerPattern = dataRow.Field<string?>(DbMetaDataColumnNames.ParameterMarkerPattern);
    x.ParameterNameMaxLength = dataRow.Field<int?>(DbMetaDataColumnNames.ParameterNameMaxLength);
    x.ParameterNamePattern = dataRow.Field<string?>(DbMetaDataColumnNames.ParameterNamePattern);
    x.QuotedIdentifierPattern = dataRow.Field<string?>(DbMetaDataColumnNames.QuotedIdentifierPattern);
    x.QuotedIdentifierCase = dataRow.Field<IdentifierCase>(DbMetaDataColumnNames.QuotedIdentifierCase);
    x.StatementSeparatorPattern = dataRow.Field<string?>(DbMetaDataColumnNames.StatementSeparatorPattern);
    x.StringLiteralPattern = dataRow.Field<string?>(DbMetaDataColumnNames.StringLiteralPattern);
    x.SupportedJoinOperators = dataRow.Field<SupportedJoinOperators?>(DbMetaDataColumnNames.SupportedJoinOperators);
    return x;
  }

  public static DataTypeRow SetValues(this DataTypeRow x, DataRow dataRow) {
    x.TypeName = dataRow.Field<string>(DbMetaDataColumnNames.TypeName);
    x.ProviderDbType = dataRow.Field<int?>(DbMetaDataColumnNames.ProviderDbType);
    x.ColumnSize = dataRow.Field<long?>(DbMetaDataColumnNames.ColumnSize);
    x.CreateFormat = dataRow.Field<string>(DbMetaDataColumnNames.CreateFormat);
    x.CreateParameters = dataRow.Field<string>(DbMetaDataColumnNames.CreateParameters);
    x.DataType = dataRow.Field<string>(DbMetaDataColumnNames.DataType);
    x.IsAutoIncrementable = dataRow.Field<bool?>(DbMetaDataColumnNames.IsAutoIncrementable);
    x.IsBestMatch = dataRow.Field<bool?>(DbMetaDataColumnNames.IsBestMatch);
    x.IsCaseSensitive = dataRow.Field<bool?>(DbMetaDataColumnNames.IsCaseSensitive);
    x.IsFixedLength = dataRow.Field<bool?>(DbMetaDataColumnNames.IsFixedLength);
    x.IsFixedPrecisionScale = dataRow.Field<bool?>(DbMetaDataColumnNames.IsFixedPrecisionScale);
    x.IsLong = dataRow.Field<bool?>(DbMetaDataColumnNames.IsLong);
    x.IsNullable = dataRow.Field<bool?>(DbMetaDataColumnNames.IsNullable);
    x.IsSearchable = dataRow.Field<bool?>(DbMetaDataColumnNames.IsSearchable);
    x.IsSearchableWithLike = dataRow.Field<bool?>(DbMetaDataColumnNames.IsSearchableWithLike);
    x.IsUnsigned = dataRow.Field<bool?>(DbMetaDataColumnNames.IsUnsigned);
    x.MaximumScale = dataRow.Field<short?>(DbMetaDataColumnNames.MaximumScale);
    x.MinimumScale = dataRow.Field<short?>(DbMetaDataColumnNames.MinimumScale);
    x.IsConcurrencyType = dataRow.Field<bool?>(DbMetaDataColumnNames.IsConcurrencyType);
    x.IsLiteralSupported = dataRow.Field<bool?>(DbMetaDataColumnNames.IsLiteralSupported);
    x.LiteralPrefix = dataRow.Field<string?>(DbMetaDataColumnNames.LiteralPrefix);
    x.LiteralSuffix = dataRow.Field<string?>(DbMetaDataColumnNames.LiteralSuffix);

    // Others
    x.DbType = dataRow.Field<DbType?>(nameof(x.DbType));
    x.MaximumVersion = dataRow.Field<string?>(nameof(x.MaximumVersion));
    x.MinimumVersion = dataRow.Field<string?>(nameof(x.MinimumVersion));
    x.INTERVAL_PRECISION = dataRow.Field<short?>(nameof(x.INTERVAL_PRECISION));
    x.LOCAL_TYPE_NAME = dataRow.Field<string?>(nameof(x.LOCAL_TYPE_NAME));
    x.NativeDataType = dataRow.Field<short?>(nameof(x.NativeDataType));
    x.OID = dataRow.Field<uint?>(nameof(x.OID));
    x.NUM_PREC_RADIX = dataRow.Field<int?>(nameof(x.NUM_PREC_RADIX));
    x.PROVIDER_TYPE = dataRow.Field<int?>(nameof(x.PROVIDER_TYPE));
    x.PROVIDER_TYPE_NAME = dataRow.Field<string?>(nameof(x.PROVIDER_TYPE_NAME));
    x.INTERVAL_PRECISION = dataRow.Field<short?>(nameof(x.SEARCHABLE));
    x.SQL_TYPE = dataRow.Field<short?>(nameof(x.SQL_TYPE));
    // Alias 

    //foreach (var columnName in new[] { "COLUMN_SIZE" }) {
    //  dic[columnName] = dataRow => ColumnSize = dataRow.Field<int?>(nameof(x.DbType));
    //}

    x.IsAutoIncrementable = GetBoolean(dataRow.Field<short?>("AUTO_UNIQUE_VALUE"));
    x.IsCaseSensitive = GetBoolean(dataRow.Field<short?>("CASE_SENSITIVE"));
    x.ColumnSize = dataRow.Field<int?>("COLUMN_SIZE");
    x.CreateParameters = dataRow.Field<string>("CREATE_PARAMS");
    x.IsFixedPrecisionScale = GetBoolean(dataRow.Field<short?>("FIXED_PREC_SCALE"));
    x.DataType = dataRow.Field<string>("FRAMEWORK_TYPE");
    x.IsFixedPrecisionScale = dataRow.Field<bool?>("IsFixedPrecisionAndScale");
    x.LiteralPrefix = dataRow.Field<string?>("LITERAL_PREFIX");
    x.LiteralSuffix = dataRow.Field<string?>("LITERAL_SUFFIX");
    x.MaximumScale = dataRow.Field<short?>("MAXIMUM_SCALE");
    x.MinimumScale = dataRow.Field<short?>("MINIMUM_SCALE");
    x.IsNullable = GetBoolean(dataRow.Field<short?>("NULLABLE"));
    x.SQL_TYPE = dataRow.Field<short?>("SQLType");
    x.TypeName = dataRow.Field<string>("SQL_TYPE_NAME");
    x.IsUnsigned = GetBoolean(dataRow.Field<short?>("UNSIGNED_ATTRIBUTE"));
    return x;
  }

  public static MetaDataCollectionRow SetValues(this MetaDataCollectionRow x, DataRow dataRow) {
    x.CollectionName = dataRow.Field<string>(DbMetaDataColumnNames.CollectionName);
    x.NumberOfRestrictions = dataRow.Field<int>(DbMetaDataColumnNames.NumberOfRestrictions);
    x.NumberOfIdentifierParts = dataRow.Field<int>(DbMetaDataColumnNames.NumberOfIdentifierParts);

    x.PopulationMechanism = dataRow.Field<string?>(nameof(x.PopulationMechanism));
    x.PopulationString = dataRow.Field<string?>(nameof(x.PopulationString));
    x.MinimumVersion = dataRow.Field<string?>(nameof(x.MinimumVersion));
    x.MaximumVersion = dataRow.Field<string?>(nameof(x.MaximumVersion));
    return x;
  }
  public static ReservedWordRow SetValues(this ReservedWordRow x, DataRow dataRow) {
    x.ReservedWord = dataRow.Field<string>(DbMetaDataColumnNames.ReservedWord);
    x.MaximumVersion = dataRow.Field<string?>(nameof(x.MaximumVersion));
    x.MinimumVersion = dataRow.Field<string?>(nameof(x.MinimumVersion));
    return x;
  }
  public static RestrictionRow SetValues(this RestrictionRow x, DataRow dataRow) {
    x.CollectionName = dataRow.Field<string>(DbMetaDataColumnNames.CollectionName);
    x.MaximumVersion = dataRow.Field<string?>(nameof(x.MaximumVersion));
    x.MinimumVersion = dataRow.Field<string?>(nameof(x.MinimumVersion));
    x.ParameterName = dataRow.Field<string?>(nameof(x.ParameterName));
    x.RestrictionName = dataRow.Field<string>(nameof(x.RestrictionName));
    x.RestrictionDefault = dataRow.Field<string>(nameof(x.RestrictionDefault));
    x.RestrictionNumber = dataRow.Field<int>(nameof(x.RestrictionNumber));
    return x;
  }
  public static DataSourceInformationRow GetDataSourceInformationRow(this DataTable dataTable) => new DataSourceInformationRow().SetValues(dataTable.Rows[0]);
  public static DataSourceInformationRow GetDataSourceInformationRow(this DbConnection dbConnection) => dbConnection.GetSchemaDataTable(DbMetaDataCollectionNames.DataSourceInformation).GetDataSourceInformationRow();

  //public DataSourceInformationRow(GetSchemaHelper getSchema) : this(getSchema.DataSourceInformation().DataTable) { }
  static bool? GetBoolean(short? value) => value switch {
    null => null,
    -1 => true,
    0 => false,
    1 => true,
    _ => throw new NotImplementedException($"{value}")
  };

  public static DataSourceInformationRow? GetInstance<T>(T connection) where T : DbConnection, new() {
    DataSourceInformationRow.Instances.TryGetValue(connection.ConnectionString, out var dataSourceInfo);
    if (dataSourceInfo != null) {
      return (DataSourceInformationRow<T>)dataSourceInfo;
    }
    try {
      var isOpen = connection.State == ConnectionState.Open;
      if (!isOpen) connection.Open();
      var dt = connection.GetSchema(DbMetaDataCollectionNames.DataSourceInformation);
      var dataSourceInformationRow = dt.GetDataSourceInformationRow();
      DataSourceInformationRow.Instances[connection.ConnectionString] = dataSourceInformationRow;
      Console.WriteLine($"{nameof(DataSourceInformationRow<T>)}: {dataSourceInformationRow.GetDataSourceProductNameWithVersion()}");
      if (!isOpen) connection.Close();
      return dataSourceInformationRow;
    } catch (Exception ex) {
      Console.WriteLine($"{nameof(DataSourceInformationRow<T>)}: {ex.Message}");
      return null;
    }
  }
  public static DataSourceInformationRow? GetInstance<T>(string connectionString) where T : DbConnection, new() => GetInstance(new T { ConnectionString = connectionString });


}

public static class DataSourceInformationRowExtensions {
  public static DataSourceProduct GetDataSourceProduct(this DataSourceInformationRow row) => DataSourceProduct.FromName(row.DataSourceProductName, true);
  public static DbSystem? GetDbSystem(this DataSourceInformationRow row) => row.GetDataSourceProduct().DbSystem;

}