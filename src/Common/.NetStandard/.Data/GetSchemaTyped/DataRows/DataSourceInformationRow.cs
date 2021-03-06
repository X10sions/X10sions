﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common.Data.GetSchemaTyped.DataRows {

  public class DataSourceInformationRow<T> : DataSourceInformationRow where T : DbConnection, new() {

    public DataSourceInformationRow(DbConnection dbConnection) : base(dbConnection) { }

    public DataSourceInformationRow(string connectionString) : this(new T { ConnectionString = connectionString }) { }

    public DataSourceInformationRow(DbConnectionStringBuilder connectionStringBuilder) : this(connectionStringBuilder.ConnectionString) { }

  }

  public class DataSourceInformationRow : _BaseTypedDataRow, IEquatable<DataSourceInformationRow> {
    // https://github.com/vince-koch/Sqlzor/tree/0442347fc153e10ea74a3896ed2ae642b29f042e/Sqlzor/Drivers/Models

    public DataSourceInformationRow(DataRow row) : base(row) {
      foreach (var col in row.Table.Columns.Cast<DataColumn>().Where(x => !dbMetaDataColumnNames.Contains(x.ColumnName))) {
        OtherColumns.Add(col.ColumnName, row.Field<object?>(col.ColumnName));
      }
      Version = GetVersion();
      DataSourceProduct = GetDataSourceProduct();
      //DbSystem = GetDbSystem();

    }

    public override int GetHashCode(){
      var hash = new HashCode();
      hash.Add(CompositeIdentifierSeparatorPattern);
      hash.Add(DataSourceProductName);
      hash.Add(DataSourceProductVersion);
      hash.Add(DataSourceProductVersionNormalized);
      hash.Add(GroupByBehavior);
      hash.Add(IdentifierCase);
      hash.Add(IdentifierPattern);
      hash.Add(OrderByColumnsInSelect);
      hash.Add(ParameterMarkerFormat);
      hash.Add(ParameterMarkerPattern);
      hash.Add(ParameterNameMaxLength);
      hash.Add(ParameterNamePattern);
      hash.Add(QuotedIdentifierPattern);
      hash.Add(QuotedIdentifierCase);
      hash.Add(StatementSeparatorPattern);
      hash.Add(StringLiteralPattern);
      hash.Add(SupportedJoinOperators);
      return hash.ToHashCode();
      //return HashCode.Combine(DataSourceProductName, DataSourceProductVersion);
      //return (DataSourceProductName, DataSourceProductVersion).GetHashCode();
    }

    public override bool Equals(object obj) => obj is DataSourceInformationRow && Equals(obj as DataSourceInformationRow);

    //public override bool Equals(object other) =>    other is Point3D p && (p.X, p.Y, p.Z).Equals((X, Y, Z));
    public bool Equals(DataSourceInformationRow? other)
      => other != null
      && CompositeIdentifierSeparatorPattern.Equals(other.CompositeIdentifierSeparatorPattern, StringComparison.OrdinalIgnoreCase)
      && DataSourceProductName.Equals(other.DataSourceProductName, StringComparison.OrdinalIgnoreCase)
      && DataSourceProductVersion.Equals(other.DataSourceProductVersion, StringComparison.OrdinalIgnoreCase)
      && DataSourceProductVersionNormalized.Equals(other.DataSourceProductVersionNormalized, StringComparison.OrdinalIgnoreCase)
      && GroupByBehavior == other.GroupByBehavior
      && IdentifierCase == other.IdentifierCase
      && IdentifierPattern.Equals(other.IdentifierPattern, StringComparison.OrdinalIgnoreCase)
      && OrderByColumnsInSelect == other.OrderByColumnsInSelect
      && ParameterMarkerFormat.Equals(other.ParameterMarkerFormat, StringComparison.OrdinalIgnoreCase)
      && ParameterMarkerPattern.Equals(other.ParameterMarkerPattern, StringComparison.OrdinalIgnoreCase)
      && ParameterNameMaxLength == other.ParameterNameMaxLength
      && ParameterNamePattern.Equals(other.ParameterNamePattern, StringComparison.OrdinalIgnoreCase)
      && QuotedIdentifierPattern.Equals(other.QuotedIdentifierPattern, StringComparison.OrdinalIgnoreCase)
      && QuotedIdentifierCase == other.QuotedIdentifierCase
      && StatementSeparatorPattern.Equals(other.StatementSeparatorPattern, StringComparison.OrdinalIgnoreCase)
      && StringLiteralPattern.Equals(other.StringLiteralPattern, StringComparison.OrdinalIgnoreCase)
      && SupportedJoinOperators == other.SupportedJoinOperators
      ;


    public Dictionary<string, object?> OtherColumns { get; } = new Dictionary<string, object?>();

    public DataSourceInformationRow(DataTable dataTable) : this(dataTable.Rows[0]) { }

    public DataSourceInformationRow(DbConnection dbConnection) : this(dbConnection.GetSchemaDataTable(DbMetaDataCollectionNames.DataSourceInformation)) { }

    public DataSourceInformationRow(GetSchemaHelper getSchema) : this(getSchema.DataSourceInformation().DataTable) { }

    #region Columns
    public string CompositeIdentifierSeparatorPattern => row.Field<string>(DbMetaDataColumnNames.CompositeIdentifierSeparatorPattern);
    public string DataSourceProductName => row.Field<string>(DbMetaDataColumnNames.DataSourceProductName);
    public string DataSourceProductVersion => row.Field<string>(DbMetaDataColumnNames.DataSourceProductVersion);
    public string DataSourceProductVersionNormalized => row.Field<string>(DbMetaDataColumnNames.DataSourceProductVersionNormalized);
    public GroupByBehavior GroupByBehavior => row.Field<GroupByBehavior>(DbMetaDataColumnNames.GroupByBehavior);
    public IdentifierCase IdentifierCase => row.Field<IdentifierCase>(DbMetaDataColumnNames.IdentifierCase);
    public string IdentifierPattern => row.Field<string>(DbMetaDataColumnNames.IdentifierPattern);
    public bool OrderByColumnsInSelect => row.Field<bool?>(DbMetaDataColumnNames.OrderByColumnsInSelect) ?? false;
    public string ParameterMarkerFormat => row.Field<string>(DbMetaDataColumnNames.ParameterMarkerFormat);
    public string ParameterMarkerPattern => row.Field<string>(DbMetaDataColumnNames.ParameterMarkerPattern);
    public int? ParameterNameMaxLength => row.Field<int?>(DbMetaDataColumnNames.ParameterNameMaxLength);
    public string ParameterNamePattern => row.Field<string>(DbMetaDataColumnNames.ParameterNamePattern);
    public string QuotedIdentifierPattern => row.Field<string>(DbMetaDataColumnNames.QuotedIdentifierPattern);
    public IdentifierCase QuotedIdentifierCase => row.Field<IdentifierCase>(DbMetaDataColumnNames.QuotedIdentifierCase);
    public string StatementSeparatorPattern => row.Field<string>(DbMetaDataColumnNames.StatementSeparatorPattern);
    public string StringLiteralPattern => row.Field<string>(DbMetaDataColumnNames.StringLiteralPattern);
    public SupportedJoinOperators SupportedJoinOperators => row.Field<SupportedJoinOperators>(DbMetaDataColumnNames.SupportedJoinOperators);
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
      DbMetaDataColumnNames.SupportedJoinOperators,
    };

    public Regex CompositeIdentifierSeparatorRegEx => new Regex(CompositeIdentifierSeparatorPattern);
    public Regex IdentifierRegEx => new Regex(IdentifierPattern);
    public Regex ParameterMarkerRegEx => new Regex(ParameterMarkerPattern);
    public Regex ParameterNameRegEx => new Regex(ParameterNamePattern);
    public Regex QuotedIdentifierRegEx => new Regex(QuotedIdentifierPattern);
    public Regex StatementSeparatorRegEx => new Regex(StatementSeparatorPattern);
    public Regex StringLiteralRegEx => new Regex(StringLiteralPattern);

    public bool CanJoinInner => SupportedJoinOperators.HasFlag(SupportedJoinOperators.Inner);
    public bool CanJoinLeft => SupportedJoinOperators.HasFlag(SupportedJoinOperators.LeftOuter);
    public bool CanJoinRight => SupportedJoinOperators.HasFlag(SupportedJoinOperators.RightOuter);
    public bool CanJoinFull => SupportedJoinOperators.HasFlag(SupportedJoinOperators.FullOuter);

    public DataSourceProduct? DataSourceProduct { get; }
    //public DbSystem? DbSystem { get; }
    public Version? Version { get; }

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
    //public DbSystem? GetDbSystem() => GetDataSourceProduct()?.DbSystem;

    public string ParameterName(string name) => string.Format(ParameterMarkerFormat, name);

  }

}
