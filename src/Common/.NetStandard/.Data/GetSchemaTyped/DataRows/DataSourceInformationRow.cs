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
  }

  public DataSourceInformationRow() { }

  public DataSourceInformationRow(DataRow dataRow) : base(dataRow) {  }

  public override void SetValues(DataRow dataRow) {
    CompositeIdentifierSeparatorPattern = dataRow.Field<string>(DbMetaDataColumnNames.CompositeIdentifierSeparatorPattern);
    DataSourceProductName = dataRow.Field<string>(DbMetaDataColumnNames.DataSourceProductName);
    DataSourceProductVersion = dataRow.Field<string>(DbMetaDataColumnNames.DataSourceProductVersion);
    DataSourceProductVersionNormalized = dataRow.Field<string>(DbMetaDataColumnNames.DataSourceProductVersionNormalized);
    GroupByBehavior = dataRow.Field<GroupByBehavior>(DbMetaDataColumnNames.GroupByBehavior);
    IdentifierCase = dataRow.Field<IdentifierCase>(DbMetaDataColumnNames.IdentifierCase);
    IdentifierPattern = dataRow.Field<string>(DbMetaDataColumnNames.IdentifierPattern);
    OrderByColumnsInSelect = dataRow.Field<bool?>(DbMetaDataColumnNames.OrderByColumnsInSelect) ?? false;
    ParameterMarkerFormat = dataRow.Field<string>(DbMetaDataColumnNames.ParameterMarkerFormat);
    ParameterMarkerPattern = dataRow.Field<string>(DbMetaDataColumnNames.ParameterMarkerPattern);
    ParameterNameMaxLength = dataRow.Field<int?>(DbMetaDataColumnNames.ParameterNameMaxLength);
    ParameterNamePattern = dataRow.Field<string?>(DbMetaDataColumnNames.ParameterNamePattern);
    QuotedIdentifierPattern = dataRow.Field<string>(DbMetaDataColumnNames.QuotedIdentifierPattern);
    QuotedIdentifierCase = dataRow.Field<IdentifierCase>(DbMetaDataColumnNames.QuotedIdentifierCase);
    StatementSeparatorPattern = dataRow.Field<string>(DbMetaDataColumnNames.StatementSeparatorPattern);
    StringLiteralPattern = dataRow.Field<string>(DbMetaDataColumnNames.StringLiteralPattern);
    SupportedJoinOperators = dataRow.Field<SupportedJoinOperators?>(DbMetaDataColumnNames.SupportedJoinOperators);

    foreach (var col in dataRow.Table.Columns.Cast<DataColumn>().Where(x => !dbMetaDataColumnNames.Contains(x.ColumnName))) {
      OtherColumns.Add(col.ColumnName, dataRow.Field<object?>(col.ColumnName));
    }
  }



  //public override int GetHashCode() {
  //  var hash = new HashCode();
  //  hash.Add(CompositeIdentifierSeparatorPattern);
  //  hash.Add(DataSourceProductName);
  //  hash.Add(DataSourceProductVersion);
  //  hash.Add(DataSourceProductVersionNormalized);
  //  hash.Add(GroupByBehavior);
  //  hash.Add(IdentifierCase);
  //  hash.Add(IdentifierPattern);
  //  hash.Add(OrderByColumnsInSelect);
  //  hash.Add(ParameterMarkerFormat);
  //  hash.Add(ParameterMarkerPattern);
  //  hash.Add(ParameterNameMaxLength);
  //  hash.Add(ParameterNamePattern);
  //  hash.Add(QuotedIdentifierPattern);
  //  hash.Add(QuotedIdentifierCase);
  //  hash.Add(StatementSeparatorPattern);
  //  hash.Add(StringLiteralPattern);
  //  hash.Add(SupportedJoinOperators);
  //  return hash.ToHashCode();
  //  //return HashCode.Combine(DataSourceProductName, DataSourceProductVersion);
  //  //return (DataSourceProductName, DataSourceProductVersion).GetHashCode();
  //}

  //public override bool Equals(object obj) => obj is DataSourceInformationRow && Equals(obj as DataSourceInformationRow);

  ////public override bool Equals(object other) =>    other is Point3D p && (p.X, p.Y, p.Z).Equals((X, Y, Z));
  //public bool Equals(DataSourceInformationRow? other)
  //  => other != null
  //  && CompositeIdentifierSeparatorPattern.Equals(other.CompositeIdentifierSeparatorPattern, StringComparison.OrdinalIgnoreCase)
  //  && DataSourceProductName.Equals(other.DataSourceProductName, StringComparison.OrdinalIgnoreCase)
  //  && DataSourceProductVersion.Equals(other.DataSourceProductVersion, StringComparison.OrdinalIgnoreCase)
  //  && DataSourceProductVersionNormalized.Equals(other.DataSourceProductVersionNormalized, StringComparison.OrdinalIgnoreCase)
  //  && GroupByBehavior == other.GroupByBehavior
  //  && IdentifierCase == other.IdentifierCase
  //  && IdentifierPattern.Equals(other.IdentifierPattern, StringComparison.OrdinalIgnoreCase)
  //  && OrderByColumnsInSelect == other.OrderByColumnsInSelect
  //  && ParameterMarkerFormat.Equals(other.ParameterMarkerFormat, StringComparison.OrdinalIgnoreCase)
  //  && ParameterMarkerPattern.Equals(other.ParameterMarkerPattern, StringComparison.OrdinalIgnoreCase)
  //  && ParameterNameMaxLength == other.ParameterNameMaxLength
  //  && ParameterNamePattern.Equals(other.ParameterNamePattern, StringComparison.OrdinalIgnoreCase)
  //  && QuotedIdentifierPattern.Equals(other.QuotedIdentifierPattern, StringComparison.OrdinalIgnoreCase)
  //  && QuotedIdentifierCase == other.QuotedIdentifierCase
  //  && StatementSeparatorPattern.Equals(other.StatementSeparatorPattern, StringComparison.OrdinalIgnoreCase)
  //  && StringLiteralPattern.Equals(other.StringLiteralPattern, StringComparison.OrdinalIgnoreCase)
  //  && SupportedJoinOperators == other.SupportedJoinOperators
  //  ;


  public Dictionary<string, object?> OtherColumns { get; } = new Dictionary<string, object?>();

  public DataSourceInformationRow(DataTable dataTable) : this(dataTable.Rows[0]) { }

  public DataSourceInformationRow(DbConnection dbConnection) : this(dbConnection.GetSchemaDataTable(DbMetaDataCollectionNames.DataSourceInformation)) { }

  public DataSourceInformationRow(GetSchemaHelper getSchema) : this(getSchema.DataSourceInformation().DataTable) { }

  #region Columns
  public string CompositeIdentifierSeparatorPattern { get; set; } = string.Empty;
  public string DataSourceProductName { get; set; } = string.Empty;
  public string DataSourceProductVersion { get; set; } = string.Empty;
  public string DataSourceProductVersionNormalized { get; set; } = string.Empty;
  public GroupByBehavior GroupByBehavior { get; set; }
  public IdentifierCase IdentifierCase { get; set; }
  public string IdentifierPattern { get; set; } = string.Empty;
  public bool OrderByColumnsInSelect { get; set; }
  public string ParameterMarkerFormat { get; set; } = string.Empty;
  public string ParameterMarkerPattern { get; set; } = string.Empty;
  public int? ParameterNameMaxLength { get; set; }
  public string? ParameterNamePattern { get; set; }
  public string QuotedIdentifierPattern { get; set; } = string.Empty;
  public IdentifierCase QuotedIdentifierCase { get; set; }
  public string StatementSeparatorPattern { get; set; } = string.Empty;
  public string StringLiteralPattern { get; set; } = string.Empty;
  public SupportedJoinOperators? SupportedJoinOperators { get; set; }
  #endregion

  string[] dbMetaDataColumnNames = new[]{
      DbMetaDataColumnNames.CompositeIdentifierSeparatorPattern,
      DbMetaDataColumnNames.DataSourceProductName,
      DbMetaDataColumnNames.DataSourceProductVersion,
      DbMetaDataColumnNames.DataSourceProductVersionNormalized,
      DbMetaDataColumnNames.GroupByBehavior,
      DbMetaDataColumnNames.IdentifierCase,
      DbMetaDataColumnNames.IdentifierPattern,
      DbMetaDataColumnNames.OrderByColumnsInSelect,
      DbMetaDataColumnNames.ParameterMarkerFormat,
      DbMetaDataColumnNames.ParameterMarkerPattern,
      DbMetaDataColumnNames.ParameterNameMaxLength,
      DbMetaDataColumnNames.ParameterNamePattern,
      DbMetaDataColumnNames.QuotedIdentifierPattern,
      DbMetaDataColumnNames.QuotedIdentifierCase,
      DbMetaDataColumnNames.StatementSeparatorPattern,
      DbMetaDataColumnNames.StringLiteralPattern,
      DbMetaDataColumnNames.SupportedJoinOperators
    };

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

  public bool UsesPositionalParameters => ParameterNameMaxLength == 0;
  public string ParameterMarker => ParameterNameMaxLength != 0 ? ParameterMarkerPattern.Substring(0, 1) : ParameterMarkerFormat;

  public string GetPlaceholder(string parameterName) => UsesPositionalParameters ? ParameterMarker : parameterName.StartsWith(ParameterMarker) ? parameterName : ParameterMarker + parameterName;

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

  public string StripParameterMarker(string parameterName) => UsesPositionalParameters ? ParameterMarker : parameterName.StartsWith(ParameterMarker) ? parameterName.Substring(ParameterMarker.Length) : parameterName;

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
    _ => throw new NotImplementedException($"{dataSourceInformationRow.DataSourceProductName}: v{dataSourceInformationRow.Version}")
  };

  //public DbSystem DbSystem { get; }
  public DbSystem.Enum DbSystemEnum => GetDbSystemEnum(this);

  public string ParameterName(string name) => ParameterNameMaxLength > 0 ? string.Format(ParameterMarker + ParameterMarkerFormat, name) : ParameterMarkerFormat;

}
