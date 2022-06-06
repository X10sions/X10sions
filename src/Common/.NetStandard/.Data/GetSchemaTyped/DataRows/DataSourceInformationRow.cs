using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace Common.Data.GetSchemaTyped.DataRows;

public class DataSourceInformationRow<T> : DataSourceInformationRow where T : DbConnection, new() {
  public DataSourceInformationRow(DbConnection dbConnection) : base(dbConnection) { }
  public DataSourceInformationRow(string connectionString) : this(new T { ConnectionString = connectionString }) { }
  public DataSourceInformationRow(DbConnectionStringBuilder connectionStringBuilder) : this(connectionStringBuilder.ConnectionString) { }
}

public class DataSourceInformationRow : BaseTypedDataRow { //}, IEquatable<DataSourceInformationRow> {
  // https://github.com/vince-koch/Sqlzor/tree/0442347fc153e10ea74a3896ed2ae642b29f042e/Sqlzor/Drivers/Models

  public class DataSourceProductNames {
    public const string DB2_400_SQL = "DB2/400 SQL";
    public const string DB2_for_IBM_i = "DB2 for IBM i";
    public const string IBM_DB2_for_i = "IBM DB2 for i";
    public const string Microsoft_SQL_Server = "	Microsoft SQL Server";
  }

  public DataSourceInformationRow() { }

  public DataSourceInformationRow(DataRow dataRow) : base(dataRow) { }


  public override Dictionary<string, Action<DataRow>> GetColumnSetValueDictionary() {
    var dic = new Dictionary<string, Action<DataRow>>();
    dic[DbMetaDataColumnNames.CompositeIdentifierSeparatorPattern] = dataRow => CompositeIdentifierSeparatorPattern = dataRow.Field<string?>(DbMetaDataColumnNames.CompositeIdentifierSeparatorPattern);
    dic[DbMetaDataColumnNames.DataSourceProductName] = dataRow => DataSourceProductName = dataRow.Field<string?>(DbMetaDataColumnNames.DataSourceProductName);
    dic[DbMetaDataColumnNames.DataSourceProductVersion] = dataRow => DataSourceProductVersion = dataRow.Field<string?>(DbMetaDataColumnNames.DataSourceProductVersion);
    dic[DbMetaDataColumnNames.DataSourceProductVersionNormalized] = dataRow => DataSourceProductVersionNormalized = dataRow.Field<string?>(DbMetaDataColumnNames.DataSourceProductVersionNormalized);
    dic[DbMetaDataColumnNames.GroupByBehavior] = dataRow => GroupByBehavior = dataRow.Field<GroupByBehavior>(DbMetaDataColumnNames.GroupByBehavior);
    dic[DbMetaDataColumnNames.IdentifierPattern] = dataRow => IdentifierPattern = dataRow.Field<string?>(DbMetaDataColumnNames.IdentifierPattern);
    dic[DbMetaDataColumnNames.IdentifierCase] = dataRow => IdentifierCase = dataRow.Field<IdentifierCase>(DbMetaDataColumnNames.IdentifierCase);
    dic[DbMetaDataColumnNames.OrderByColumnsInSelect] = dataRow => OrderByColumnsInSelect = dataRow.Field<bool?>(DbMetaDataColumnNames.OrderByColumnsInSelect);
    dic[DbMetaDataColumnNames.ParameterMarkerFormat] = dataRow => ParameterMarkerFormat = dataRow.Field<string?>(DbMetaDataColumnNames.ParameterMarkerFormat);
    dic[DbMetaDataColumnNames.ParameterMarkerPattern] = dataRow => ParameterMarkerPattern = dataRow.Field<string?>(DbMetaDataColumnNames.ParameterMarkerPattern);
    dic[DbMetaDataColumnNames.ParameterNameMaxLength] = dataRow => ParameterNameMaxLength = dataRow.Field<int?>(DbMetaDataColumnNames.ParameterNameMaxLength);
    dic[DbMetaDataColumnNames.ParameterNamePattern] = dataRow => ParameterNamePattern = dataRow.Field<string?>(DbMetaDataColumnNames.ParameterNamePattern);
    dic[DbMetaDataColumnNames.QuotedIdentifierPattern] = dataRow => QuotedIdentifierPattern = dataRow.Field<string?>(DbMetaDataColumnNames.QuotedIdentifierPattern);
    dic[DbMetaDataColumnNames.QuotedIdentifierCase] = dataRow => QuotedIdentifierCase = dataRow.Field<IdentifierCase>(DbMetaDataColumnNames.QuotedIdentifierCase);
    dic[DbMetaDataColumnNames.StatementSeparatorPattern] = dataRow => StatementSeparatorPattern = dataRow.Field<string?>(DbMetaDataColumnNames.StatementSeparatorPattern);
    dic[DbMetaDataColumnNames.StringLiteralPattern] = dataRow => StringLiteralPattern = dataRow.Field<string?>(DbMetaDataColumnNames.StringLiteralPattern);
    dic[DbMetaDataColumnNames.SupportedJoinOperators] = dataRow => SupportedJoinOperators = dataRow.Field<SupportedJoinOperators?>(DbMetaDataColumnNames.SupportedJoinOperators);
    return dic;
  }

  public DataSourceInformationRow(DataTable dataTable) : this(dataTable.Rows[0]) { }

  public DataSourceInformationRow(DbConnection dbConnection) : this(dbConnection.GetSchemaDataTable(DbMetaDataCollectionNames.DataSourceInformation)) { }

  public DataSourceInformationRow(GetSchemaHelper getSchema) : this(getSchema.DataSourceInformation().DataTable) { }

  #region Columns
  public string? CompositeIdentifierSeparatorPattern { get; set; }
  public string? DataSourceProductName { get; set; }
  public string? DataSourceProductVersion { get; set; }
  public string? DataSourceProductVersionNormalized { get; set; }
  public GroupByBehavior? GroupByBehavior { get; set; }
  public IdentifierCase? IdentifierCase { get; set; }
  public string? IdentifierPattern { get; set; }
  public bool? OrderByColumnsInSelect { get; set; }
  public string? ParameterMarkerFormat { get; set; }
  public string? ParameterMarkerPattern { get; set; }
  public int? ParameterNameMaxLength { get; set; }
  public string? ParameterNamePattern { get; set; }
  public string? QuotedIdentifierPattern { get; set; }
  public IdentifierCase? QuotedIdentifierCase { get; set; }
  public string? StatementSeparatorPattern { get; set; }
  public string? StringLiteralPattern { get; set; }
  public SupportedJoinOperators? SupportedJoinOperators { get; set; }
  #endregion

  public Regex CompositeIdentifierSeparatorRegEx => new Regex(CompositeIdentifierSeparatorPattern);
  public Regex IdentifierRegEx => new Regex(IdentifierPattern);
  public Regex ParameterMarkerRegEx => new Regex(ParameterMarkerPattern);
  public Regex ParameterNameRegEx => new Regex(ParameterNamePattern ?? string.Empty);
  public Regex QuotedIdentifierRegEx => new Regex(QuotedIdentifierPattern);
  public Regex StatementSeparatorRegEx => new Regex(StatementSeparatorPattern);
  public Regex StringLiteralRegEx => new Regex(StringLiteralPattern);

  public DataSourceProduct? DataSourceProduct => GetDataSourceProduct();
  //public DbSystem? DbSystem { get; }
  public Version? Version => GetVersion();

  public string GetDataSourceProductNameWithVersion() => $"{DataSourceProductName}.v{Version ?? new Version()}";

  public Version? GetVersion() {
    Version.TryParse(DataSourceProductVersion?.Split(' ')[0], out var version);
    if (version == null) {
      Version.TryParse(DataSourceProductVersionNormalized?.Split(' ')[0], out version);
    }
    return version;
  }

  //public static class Characters {
  //  public const char QuesitonMark = '?';
  //  public const char AtSign = '@';
  //  public const char Colon = ':';

  //  public static char[] ForParameters = new[] { AtSign, Colon };
  //}

  public bool UsesPositionalParameters() => ParameterNameMaxLength == 0;
  public string ParameterMarker() => (ParameterNameMaxLength != 0 ? ParameterMarkerPattern?.Substring(0, 1) : ParameterMarkerFormat) ?? string.Empty;

  public string GetPlaceholder(string parameterName) => UsesPositionalParameters() ? ParameterMarker() : parameterName.StartsWith(ParameterMarker()) ? parameterName : ParameterMarker + parameterName;

  //public string QuoteIdentifierPart(string unquotedIdentifierPart) {
  //  if (string.IsNullOrWhiteSpace(unquotedIdentifierPart)) {
  //    return string.Empty;
  //  }
  //  string identifierQuotePrefix = Connection.Information.IdentifierQuotePrefix;
  //  string identifierQuoteSuffix = Connection.Information.IdentifierQuoteSuffix;
  //  if (!unquotedIdentifierPart.StartsWith(identifierQuotePrefix, StringComparison.Ordinal) || !unquotedIdentifierPart.EndsWith(identifierQuoteSuffix, StringComparison.Ordinal)) {
  //    return identifierQuotePrefix + unquotedIdentifierPart + identifierQuoteSuffix;
  //  }
  //  return unquotedIdentifierPart;
  //}

  public string StripParameterMarker(string parameterName) => UsesPositionalParameters() ? ParameterMarker() : parameterName.StartsWith(ParameterMarker()) ? parameterName.Substring(ParameterMarker().Length) : parameterName;

  //public string GetConnectionTypeVersionName<T>() where T : IDbConnection => $"{typeof(T).Name}.v{Version}";
  public DataSourceProduct? GetDataSourceProduct() => DataSourceProduct.List.FirstOrDefault(x => x.Name.Equals(DataSourceProductName, StringComparison.OrdinalIgnoreCase));
  //public DbSystem GetDbSystem(DataSourceInformationRow dataSourceInformationRow) => dataSourceInformationRow.DataSourceProductName switch { 
  //  DataSourceProductNames.DB2_for_IBM_i => DB2iSeriesDbSystem.DB2iSeries, 
  //  DataSourceProductNames.DB2_400_SQL => DB2iSeriesDbSystem.DB2iSeries, 
  //  DataSourceProductNames.IBM_DB2_for_i => DB2iSeriesDbSystem.DB2iSeries, 
  //  _ => throw new NotImplementedException($"{dataSourceInformationRow.DataSourceProductName}: v{dataSourceInformationRow.Version}")
  //};

  public DbSystem.Enum GetDbSystemEnum(DataSourceInformationRow dataSourceInformationRow) => dataSourceInformationRow.DataSourceProductName switch {
    DataSourceProductNames.DB2_for_IBM_i => DbSystem.Enum.DB2iSeries,
    DataSourceProductNames.DB2_400_SQL => DbSystem.Enum.DB2iSeries,
    DataSourceProductNames.IBM_DB2_for_i => DbSystem.Enum.DB2iSeries,
    _ => DbSystem.Enum._Unkown
    //_ => throw new NotImplementedException($"{dataSourceInformationRow.DataSourceProductName}: v{dataSourceInformationRow.Version}")
  };

  //public DbSystem DbSystem { get; }
  public DbSystem.Enum DbSystemEnum() => GetDbSystemEnum(this);

  public string? ParameterName(string name) => ParameterNameMaxLength > 0 ? string.Format(ParameterMarker() + ParameterMarkerFormat, name) : ParameterMarkerFormat;

}
