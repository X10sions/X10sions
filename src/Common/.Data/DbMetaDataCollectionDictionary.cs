using System.Data;
using System.Data.Common;

namespace Common.Data;
public class DbMetaDataCollectionDictionary : Dictionary<string, DataTable?> {
  public DbMetaDataCollectionDictionary() { }
  public DbMetaDataCollectionDictionary(DbConnection dbConnection, bool doSchemasWithRestrictions) {
    GetSchemas(dbConnection, doSchemasWithRestrictions);
  }

  public void GetSchemas(DbConnection dbConnection, bool doSchemasWithRestrictions) {
    DataSourceInformation = dbConnection.GetSchema(DbMetaDataCollectionNames.DataSourceInformation);
    DataTypes = dbConnection.GetSchema(DbMetaDataCollectionNames.DataTypes);
    MetaDataCollections = dbConnection.GetSchema(DbMetaDataCollectionNames.MetaDataCollections);
    Restrictions = dbConnection.GetSchema(DbMetaDataCollectionNames.Restrictions);
    ReservedWords = dbConnection.GetSchema(DbMetaDataCollectionNames.ReservedWords);
    if (!doSchemasWithRestrictions) return;
    var filterExpression = $"{DbMetaDataColumnNames.CollectionName} Not In ('{DbMetaDataCollectionNames.DataSourceInformation}','{DbMetaDataCollectionNames.DataTypes}','{DbMetaDataCollectionNames.MetaDataCollections}','{DbMetaDataCollectionNames.ReservedWords}','{DbMetaDataCollectionNames.Restrictions}')";
    foreach (var mdc in MetaDataCollections.Select(filterExpression, DbMetaDataColumnNames.NumberOfRestrictions)) {
      var collectionName = mdc.Field<string>(DbMetaDataColumnNames.CollectionName);
      var numberOfRestrictions = mdc.Field<int>(DbMetaDataColumnNames.NumberOfRestrictions);
      var restrictionValues = new string?[numberOfRestrictions].Select(x => string.Empty).ToArray();

      //var restrictions = Restrictions?.Select($"{DbMetaDataColumnNames.CollectionName} = '{collectionName}'", "RestrictionNumber");
      //if (restrictions != null) {
      //  foreach (var d in restrictions) {
      //    var restrictionDefault = d.Field<string?>("RestrictionDefault");
      //    var restrictionName = d.Field<string>("RestrictionName");
      //    var restrictionNumber = d.Field<int>("RestrictionNumber");
      //    Console.WriteLine($"restrictionValues[{collectionName}:{restrictionNumber}:{restrictionName}] = {restrictionDefault};");
      //    if (!string.IsNullOrWhiteSpace(restrictionDefault)) {
      //      restrictionValues[restrictionNumber - 1] = restrictionDefault;
      //    } else if (collectionName == nameof(Procedures) && restrictionName == "PROCEDURE_TYPE") {
      //      restrictionValues[restrictionNumber - 1] = "0";
      //    }
      //  }
      //}

      Console.WriteLine($"restrictionValues[{collectionName}: {string.Join(",", restrictionValues)}];");

      var dataTable = dbConnection.GetSchema(collectionName, restrictionValues);
      collectionName = collectionName.Replace(" ", "");
      //Common
      if (collectionName == nameof(Columns)) { Columns = dataTable; continue; }
      if (collectionName == nameof(Indexes)) { Indexes = dataTable; continue; }
      if (collectionName == nameof(Procedures)) { Procedures = dataTable; continue; }
      if (collectionName == nameof(Schemas)) { Schemas = dataTable; continue; }
      if (collectionName == nameof(Tables)) { Tables = dataTable; continue; }
      if (collectionName == nameof(Views)) { Views = dataTable; continue; }
      // Other
      if (collectionName == nameof(AllColumns)) { AllColumns = dataTable; continue; }
      if (collectionName == nameof(Arguments)) { Arguments = dataTable; continue; }
      if (collectionName == nameof(Catalogs)) { Catalogs = dataTable; continue; }
      if (collectionName == nameof(CharacterSets)) { CharacterSets = dataTable; continue; }
      if (collectionName == nameof(CheckConstraints)) { CheckConstraints = dataTable; continue; }
      if (collectionName == nameof(CheckConstraintsByTable)) { CheckConstraintsByTable = dataTable; continue; }
      if (collectionName == nameof(Collations)) { Collations = dataTable; continue; }
      if (collectionName == nameof(ColumnPrivileges)) { ColumnPrivileges = dataTable; continue; }
      if (collectionName == nameof(ColumnSetColumns)) { ColumnSetColumns = dataTable; continue; }
      if (collectionName == nameof(Databases)) { Databases = dataTable; continue; }
      if (collectionName == nameof(Domains)) { Domains = dataTable; continue; }
      if (new[] { nameof(ForeignKeyColumns), "Foreign Key Columns" }.Contains(collectionName)) { ForeignKeyColumns = dataTable; continue; }
      if (new[] { nameof(ForeignKeys), "Foreign Keys" }.Contains(collectionName)) { ForeignKeys = dataTable; continue; }
      if (collectionName == nameof(FunctionArguments)) { FunctionArguments = dataTable; continue; }
      if (collectionName == nameof(FunctionPrivileges)) { FunctionPrivileges = dataTable; continue; }
      if (collectionName == nameof(Functions)) { Functions = dataTable; continue; }
      if (collectionName == nameof(Generators)) { Generators = dataTable; continue; }
      if (collectionName == nameof(IndexColumns)) { IndexColumns = dataTable; continue; }
      if (collectionName == nameof(JavaClasses)) { JavaClasses = dataTable; continue; }
      if (collectionName == nameof(Packages)) { Packages = dataTable; continue; }
      if (collectionName == nameof(PackageBodies)) { PackageBodies = dataTable; continue; }
      if (collectionName == nameof(PrimaryKeys)) { PrimaryKeys = dataTable; continue; }
      if (collectionName == nameof(ProcedureColumns)) { ProcedureColumns = dataTable; continue; }
      if (collectionName == nameof(ProcedureParameters)) { ProcedureParameters = dataTable; continue; }
      if (collectionName == nameof(ProcedurePrivileges)) { ProcedurePrivileges = dataTable; continue; }
      if (collectionName == nameof(Roles)) { Roles = dataTable; continue; }
      if (collectionName == nameof(Schemas)) { Schemas = dataTable; continue; }
      if (collectionName == nameof(Sequences)) { Sequences = dataTable; continue; }
      if (collectionName == nameof(Synonyms)) { Synonyms = dataTable; continue; }
      if (collectionName == nameof(StructuredTypeMembers)) { StructuredTypeMembers = dataTable; continue; }
      if (collectionName == nameof(TableConstraints)) { TableConstraints = dataTable; continue; }
      if (collectionName == nameof(TablePrivileges)) { TablePrivileges = dataTable; continue; }
      if (collectionName == nameof(Triggers)) { Triggers = dataTable; continue; }
      if (collectionName == nameof(UDF)) { UDF = dataTable; continue; }
      if (collectionName == nameof(UniqueKeys)) { UniqueKeys = dataTable; continue; }
      if (collectionName == nameof(UserDefinedTypes)) { UserDefinedTypes = dataTable; continue; }
      if (collectionName == nameof(Users)) { Users = dataTable; continue; }
      if (collectionName == nameof(ViewColumns)) { ViewColumns = dataTable; continue; }
      if (collectionName == nameof(ViewPrivileges)) { ViewPrivileges = dataTable; continue; }
      if (collectionName == nameof(XMLSchemas)) { XMLSchemas = dataTable; continue; }
      throw new NotImplementedException(collectionName);
    }
  }

  DataTable? Get(string key) {
    var exists = TryGetValue(key, out var dataTable);
    return dataTable;
  }

  #region All
  public DataTable? DataSourceInformation { get => Get(DbMetaDataCollectionNames.DataSourceInformation); set => this[DbMetaDataCollectionNames.DataSourceInformation] = value; }
  public DataTable? DataTypes { get => Get(DbMetaDataCollectionNames.DataTypes); set => this[DbMetaDataCollectionNames.DataTypes] = value; }
  public DataTable? MetaDataCollections { get => Get(DbMetaDataCollectionNames.MetaDataCollections); set => this[DbMetaDataCollectionNames.MetaDataCollections] = value; }
  public DataTable? ReservedWords { get => Get(DbMetaDataCollectionNames.ReservedWords); set => this[DbMetaDataCollectionNames.ReservedWords] = value; }
  public DataTable? Restrictions { get => Get(DbMetaDataCollectionNames.Restrictions); set => this[DbMetaDataCollectionNames.Restrictions] = value; }
  #endregion

  #region Common 
  public DataTable? Columns { get => Get(nameof(Columns)); set => this[nameof(Columns)] = value; }
  public DataTable? Indexes { get => Get(nameof(Indexes)); set => this[nameof(Indexes)] = value; }
  public DataTable? Procedures { get => Get(nameof(Procedures)); set => this[nameof(Procedures)] = value; }
  public DataTable? Tables { get => Get(nameof(Tables)); set => this[nameof(Tables)] = value; }
  public DataTable? Views { get => Get(nameof(Views)); set => this[nameof(Views)] = value; }
  #endregion

  #region Other
  public DataTable? AllColumns { get => Get(nameof(AllColumns)); set => this[nameof(AllColumns)] = value; }
  public DataTable? Arguments { get => Get(nameof(Arguments)); set => this[nameof(Arguments)] = value; }
  public DataTable? Catalogs { get => Get(nameof(Catalogs)); set => this[nameof(Catalogs)] = value; }
  public DataTable? CharacterSets { get => Get(nameof(CharacterSets)); set => this[nameof(CharacterSets)] = value; }
  public DataTable? CheckConstraints { get => Get(nameof(CheckConstraints)); set => this[nameof(CheckConstraints)] = value; }
  public DataTable? CheckConstraintsByTable { get => Get(nameof(CheckConstraintsByTable)); set => this[nameof(CheckConstraintsByTable)] = value; }
  public DataTable? Collations { get => Get(nameof(Collations)); set => this[nameof(Collations)] = value; }
  public DataTable? ColumnPrivileges { get => Get(nameof(ColumnPrivileges)); set => this[nameof(ColumnPrivileges)] = value; }
  public DataTable? ColumnSetColumns { get => Get(nameof(ColumnSetColumns)); set => this[nameof(ColumnSetColumns)] = value; }
  public DataTable? Databases { get => Get(nameof(Databases)); set => this[nameof(Databases)] = value; }
  public DataTable? Domains { get => Get(nameof(Domains)); set => this[nameof(Domains)] = value; }
  public DataTable? ForeignKeyColumns { get => Get(nameof(ForeignKeyColumns)); set => this[nameof(ForeignKeyColumns)] = value; }
  public DataTable? ForeignKeys { get => Get(nameof(ForeignKeys)); set => this[nameof(ForeignKeys)] = value; }
  public DataTable? FunctionArguments { get => Get(nameof(FunctionArguments)); set => this[nameof(FunctionArguments)] = value; }
  public DataTable? FunctionPrivileges { get => Get(nameof(FunctionPrivileges)); set => this[nameof(FunctionPrivileges)] = value; }
  public DataTable? Functions { get => Get(nameof(Functions)); set => this[nameof(Functions)] = value; }
  public DataTable? Generators { get => Get(nameof(Generators)); set => this[nameof(Generators)] = value; }
  public DataTable? IndexColumns { get => Get(nameof(IndexColumns)); set => this[nameof(IndexColumns)] = value; }
  public DataTable? JavaClasses { get => Get(nameof(JavaClasses)); set => this[nameof(JavaClasses)] = value; }
  public DataTable? Packages { get => Get(nameof(Packages)); set => this[nameof(Packages)] = value; }
  public DataTable? PackageBodies { get => Get(nameof(PackageBodies)); set => this[nameof(PackageBodies)] = value; }
  public DataTable? PrimaryKeys { get => Get(nameof(PrimaryKeys)); set => this[nameof(PrimaryKeys)] = value; }
  public DataTable? ProcedureColumns { get => Get(nameof(ProcedureColumns)); set => this[nameof(ProcedureColumns)] = value; }
  public DataTable? ProcedureParameters { get => Get(nameof(ProcedureParameters)); set => this[nameof(ProcedureParameters)] = value; }
  public DataTable? ProcedurePrivileges { get => Get(nameof(ProcedurePrivileges)); set => this[nameof(ProcedurePrivileges)] = value; }
  public DataTable? Roles { get => Get(nameof(Roles)); set => this[nameof(Roles)] = value; }
  public DataTable? Schemas { get => Get(nameof(Schemas)); set => this[nameof(Schemas)] = value; }
  public DataTable? Sequences { get => Get(nameof(Sequences)); set => this[nameof(Sequences)] = value; }
  public DataTable? Synonyms { get => Get(nameof(Synonyms)); set => this[nameof(Synonyms)] = value; }
  public DataTable? StructuredTypeMembers { get => Get(nameof(StructuredTypeMembers)); set => this[nameof(StructuredTypeMembers)] = value; }
  public DataTable? TableConstraints { get => Get(nameof(TableConstraints)); set => this[nameof(TableConstraints)] = value; }
  public DataTable? TablePrivileges { get => Get(nameof(TablePrivileges)); set => this[nameof(TablePrivileges)] = value; }
  public DataTable? Triggers { get => Get(nameof(Triggers)); set => this[nameof(Triggers)] = value; }
  public DataTable? UDF { get => Get(nameof(UDF)); set => this[nameof(UDF)] = value; }
  public DataTable? UniqueKeys { get => Get(nameof(UniqueKeys)); set => this[nameof(UniqueKeys)] = value; }
  public DataTable? UserDefinedTypes { get => Get(nameof(UserDefinedTypes)); set => this[nameof(UserDefinedTypes)] = value; }
  public DataTable? Users { get => Get(nameof(Users)); set => this[nameof(Users)] = value; }
  public DataTable? ViewColumns { get => Get(nameof(ViewColumns)); set => this[nameof(ViewColumns)] = value; }
  public DataTable? ViewPrivileges { get => Get(nameof(ViewPrivileges)); set => this[nameof(ViewPrivileges)] = value; }
  public DataTable? XMLSchemas { get => Get(nameof(XMLSchemas)); set => this[nameof(XMLSchemas)] = value; }
  #endregion
}