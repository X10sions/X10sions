using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace Common.Data.GetSchemaTyped.DataRows;

public class DataSourceInformationRow<T> : DataSourceInformationRow where T : DbConnection, new() {
  public DataSourceInformationRow(DbConnection dbConnection) : base(dbConnection) { }
  public DataSourceInformationRow(string connectionString) : this(new T { ConnectionString = connectionString }) { }
  public DataSourceInformationRow(DbConnectionStringBuilder connectionStringBuilder) : this(connectionStringBuilder.ConnectionString) { }
}

public class DataSourceInformationRow : ITypedDataRow { //}, IEquatable<DataSourceInformationRow> {
  #region Instances

  public static Dictionary<string, DataSourceInformationRow?> Instances = new Dictionary<string, DataSourceInformationRow?>();

  public static DataSourceInformationRow? GetInstance<T>(T connection) where T : DbConnection, new() {
    Instances.TryGetValue(connection.ConnectionString, out var dataSourceInfo);
    if (dataSourceInfo != null) {
      return (DataSourceInformationRow<T>)dataSourceInfo;
    }
    try {
      var isOpen = connection.State == ConnectionState.Open;
      if (!isOpen) connection.Open();
      var dt = connection.GetSchema(DbMetaDataCollectionNames.DataSourceInformation);
      var dataSourceInformationRow = new DataSourceInformationRow(dt);
      Instances[connection.ConnectionString] = dataSourceInformationRow;
      Console.WriteLine($"{nameof(DataSourceInformationRow<T>)}: {dataSourceInformationRow.GetDataSourceProductNameWithVersion()}");
      if (!isOpen) connection.Close();
      return dataSourceInformationRow;
    } catch (Exception ex) {
      Console.WriteLine($"{nameof(DataSourceInformationRow<T>)}: {ex.Message}");
      return null;
    }
  }
  public static DataSourceInformationRow? GetInstance<T>(string connectionString) where T : DbConnection, new() => GetInstance(new T { ConnectionString = connectionString });
  #endregion

  // https://github.com/vince-koch/Sqlzor/tree/0442347fc153e10ea74a3896ed2ae642b29f042e/Sqlzor/Drivers/Models

  public DataSourceInformationRow() { }

  public DataSourceInformationRow(DataRow dataRow) {
    SetValues(dataRow);
  }

  public void SetValues(DataRow dataRow) {
    CompositeIdentifierSeparatorPattern = dataRow.Field<string?>(DbMetaDataColumnNames.CompositeIdentifierSeparatorPattern);
    DataSourceProductName = dataRow.Field<string?>(DbMetaDataColumnNames.DataSourceProductName);
    DataSourceProductVersion = dataRow.Field<string?>(DbMetaDataColumnNames.DataSourceProductVersion);
    DataSourceProductVersionNormalized = dataRow.Field<string?>(DbMetaDataColumnNames.DataSourceProductVersionNormalized);
    GroupByBehavior = dataRow.Field<GroupByBehavior>(DbMetaDataColumnNames.GroupByBehavior);
    IdentifierPattern = dataRow.Field<string?>(DbMetaDataColumnNames.IdentifierPattern);
    IdentifierCase = dataRow.Field<IdentifierCase>(DbMetaDataColumnNames.IdentifierCase);
    OrderByColumnsInSelect = dataRow.Field<bool?>(DbMetaDataColumnNames.OrderByColumnsInSelect);
    ParameterMarkerFormat = dataRow.Field<string?>(DbMetaDataColumnNames.ParameterMarkerFormat);
    ParameterMarkerPattern = dataRow.Field<string?>(DbMetaDataColumnNames.ParameterMarkerPattern);
    ParameterNameMaxLength = dataRow.Field<int?>(DbMetaDataColumnNames.ParameterNameMaxLength);
    ParameterNamePattern = dataRow.Field<string?>(DbMetaDataColumnNames.ParameterNamePattern);
    QuotedIdentifierPattern = dataRow.Field<string?>(DbMetaDataColumnNames.QuotedIdentifierPattern);
    QuotedIdentifierCase = dataRow.Field<IdentifierCase>(DbMetaDataColumnNames.QuotedIdentifierCase);
    StatementSeparatorPattern = dataRow.Field<string?>(DbMetaDataColumnNames.StatementSeparatorPattern);
    StringLiteralPattern = dataRow.Field<string?>(DbMetaDataColumnNames.StringLiteralPattern);
    SupportedJoinOperators = dataRow.Field<SupportedJoinOperators?>(DbMetaDataColumnNames.SupportedJoinOperators);
  }

  public DataSourceInformationRow(DataTable dataTable) : this(dataTable.Rows[0]) { }

  public DataSourceInformationRow(DbConnection dbConnection) : this(dbConnection.GetSchemaDataTable(DbMetaDataCollectionNames.DataSourceInformation)) { }

  //public DataSourceInformationRow(GetSchemaHelper getSchema) : this(getSchema.DataSourceInformation().DataTable) { }

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

  public Version Version => GetVersion();

  public string GetDataSourceProductNameWithVersion() => $"{DataSourceProductName}.v{Version ?? new Version()}";

  public Version GetVersion() {
    Version.TryParse(DataSourceProductVersion?.Split(' ')[0], out var version);
    if (version == null) {
      Version.TryParse(DataSourceProductVersionNormalized?.Split(' ')[0], out version);
    }
    return version ?? new Version();
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

  public string? ParameterName(string name) => ParameterNameMaxLength > 0 ? string.Format(ParameterMarker() + ParameterMarkerFormat, name) : ParameterMarkerFormat;

}

public static class DataSourceInformationRowExtensions {

  public static DataSourceProduct GetDataSourceProduct(this DataSourceInformationRow row) => DataSourceProduct.FromName(row.DataSourceProductName, true);

  public static DbSystem? GetDbSystem(this DataSourceInformationRow row) => row.GetDataSourceProduct().DbSystem;

}
