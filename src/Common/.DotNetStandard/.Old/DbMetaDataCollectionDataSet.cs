using System.Data;
using System.Data.Common;

namespace Common.Data;
public class DbMetaDataCollectionDataSet : DataSet {
  public DbMetaDataCollectionDataSet() { }
  public DbMetaDataCollectionDataSet(DbConnection dbConnection) {
    GetSchemas(dbConnection);
  }

  public void GetSchemas(DbConnection dbConnection) {
    DataSourceInformation = dbConnection.GetSchema(DbMetaDataCollectionNames.DataSourceInformation);
    DataTypes = dbConnection.GetSchema(DbMetaDataCollectionNames.DataTypes);
    MetaDataCollections = dbConnection.GetSchema(DbMetaDataCollectionNames.MetaDataCollections);
    Restrictions = dbConnection.GetSchema(DbMetaDataCollectionNames.Restrictions);
    ReservedWords = dbConnection.GetSchema(DbMetaDataCollectionNames.ReservedWords);

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

  DataTable? Get(string key) => base.Tables[key];

  void Set(string key, DataTable? value) {
    if (value == null) return;
    if (base.Tables.Contains(key)) base.Tables.Remove(key);
    value.TableName = key;
    base.Tables.Add(value);
  }

  #region All
  public DataTable? DataSourceInformation { get => Get(DbMetaDataCollectionNames.DataSourceInformation); set => Set(DbMetaDataCollectionNames.DataSourceInformation, value); }
  public DataTable? DataTypes { get => Get(DbMetaDataCollectionNames.DataTypes); set => Set(DbMetaDataCollectionNames.DataTypes, value); }
  public DataTable? MetaDataCollections { get => Get(DbMetaDataCollectionNames.MetaDataCollections); set => Set(DbMetaDataCollectionNames.MetaDataCollections, value); }
  public DataTable? ReservedWords { get => Get(DbMetaDataCollectionNames.ReservedWords); set => Set(DbMetaDataCollectionNames.ReservedWords, value); }
  public DataTable? Restrictions { get => Get(DbMetaDataCollectionNames.Restrictions); set => Set(DbMetaDataCollectionNames.Restrictions,  value); }
  #endregion

  #region Common 
  public DataTable? Columns { get => Get(nameof(Columns)); set => Set(nameof(Columns), value); }
  public DataTable? Indexes { get => Get(nameof(Indexes)); set => Set(nameof(Indexes), value); }
  public DataTable? Procedures { get => Get(nameof(Procedures)); set => Set(nameof(Procedures), value); }
  public DataTable? Tables { get => Get(nameof(Tables)); set => Set(nameof(Tables), value); }
  public DataTable? Views { get => Get(nameof(Views)); set => Set(nameof(Views), value); }
  #endregion

  #region Other
  public DataTable? AllColumns { get => Get(nameof(AllColumns)); set => Set(nameof(AllColumns), value); }
  public DataTable? Arguments { get => Get(nameof(Arguments)); set => Set(nameof(Arguments), value); }
  public DataTable? Catalogs { get => Get(nameof(Catalogs)); set => Set(nameof(Catalogs), value); }
  public DataTable? CharacterSets { get => Get(nameof(CharacterSets)); set => Set(nameof(CharacterSets), value); }
  public DataTable? CheckConstraints { get => Get(nameof(CheckConstraints)); set => Set(nameof(CheckConstraints), value); }
  public DataTable? CheckConstraintsByTable { get => Get(nameof(CheckConstraintsByTable)); set => Set(nameof(CheckConstraintsByTable), value); }
  public DataTable? Collations { get => Get(nameof(Collations)); set => Set(nameof(Collations), value); }
  public DataTable? ColumnPrivileges { get => Get(nameof(ColumnPrivileges)); set => Set(nameof(ColumnPrivileges), value); }
  public DataTable? ColumnSetColumns { get => Get(nameof(ColumnSetColumns)); set => Set(nameof(ColumnSetColumns), value); }
  public DataTable? Databases { get => Get(nameof(Databases)); set => Set(nameof(Databases), value); }
  public DataTable? Domains { get => Get(nameof(Domains)); set => Set(nameof(Domains), value); }
  public DataTable? ForeignKeyColumns { get => Get(nameof(ForeignKeyColumns)); set => Set(nameof(ForeignKeyColumns), value); }
  public DataTable? ForeignKeys { get => Get(nameof(ForeignKeys)); set => Set(nameof(ForeignKeys), value); }
  public DataTable? FunctionArguments { get => Get(nameof(FunctionArguments)); set => Set(nameof(FunctionArguments), value); }
  public DataTable? FunctionPrivileges { get => Get(nameof(FunctionPrivileges)); set => Set(nameof(FunctionPrivileges), value); }
  public DataTable? Functions { get => Get(nameof(Functions)); set => Set(nameof(Functions), value); }
  public DataTable? Generators { get => Get(nameof(Generators)); set => Set(nameof(Generators), value); }
  public DataTable? IndexColumns { get => Get(nameof(IndexColumns)); set => Set(nameof(IndexColumns), value); }
  public DataTable? JavaClasses { get => Get(nameof(JavaClasses)); set => Set(nameof(JavaClasses), value); }
  public DataTable? Packages { get => Get(nameof(Packages)); set => Set(nameof(Packages), value); }
  public DataTable? PackageBodies { get => Get(nameof(PackageBodies)); set => Set(nameof(PackageBodies), value); }
  public DataTable? PrimaryKeys { get => Get(nameof(PrimaryKeys)); set => Set(nameof(PrimaryKeys), value); }
  public DataTable? ProcedureColumns { get => Get(nameof(ProcedureColumns)); set => Set(nameof(ProcedureColumns), value); }
  public DataTable? ProcedureParameters { get => Get(nameof(ProcedureParameters)); set => Set(nameof(ProcedureParameters), value); }
  public DataTable? ProcedurePrivileges { get => Get(nameof(ProcedurePrivileges)); set => Set(nameof(ProcedurePrivileges), value); }
  public DataTable? Roles { get => Get(nameof(Roles)); set => Set(nameof(Roles), value); }
  public DataTable? Schemas { get => Get(nameof(Schemas)); set => Set(nameof(Schemas), value); }
  public DataTable? Sequences { get => Get(nameof(Sequences)); set => Set(nameof(Sequences), value); }
  public DataTable? Synonyms { get => Get(nameof(Synonyms)); set => Set(nameof(Synonyms), value); }
  public DataTable? StructuredTypeMembers { get => Get(nameof(StructuredTypeMembers)); set => Set(nameof(StructuredTypeMembers), value); }
  public DataTable? TableConstraints { get => Get(nameof(TableConstraints)); set => Set(nameof(TableConstraints), value); }
  public DataTable? TablePrivileges { get => Get(nameof(TablePrivileges)); set => Set(nameof(TablePrivileges), value); }
  public DataTable? Triggers { get => Get(nameof(Triggers)); set => Set(nameof(Triggers), value); }
  public DataTable? UDF { get => Get(nameof(UDF)); set => Set(nameof(UDF), value); }
  public DataTable? UniqueKeys { get => Get(nameof(UniqueKeys)); set => Set(nameof(UniqueKeys), value); }
  public DataTable? UserDefinedTypes { get => Get(nameof(UserDefinedTypes)); set => Set(nameof(UserDefinedTypes), value); }
  public DataTable? Users { get => Get(nameof(Users)); set => Set(nameof(Users), value); }
  public DataTable? ViewColumns { get => Get(nameof(ViewColumns)); set => Set(nameof(ViewColumns), value); }
  public DataTable? ViewPrivileges { get => Get(nameof(ViewPrivileges)); set => Set(nameof(ViewPrivileges), value); }
  public DataTable? XMLSchemas { get => Get(nameof(XMLSchemas)); set => Set(nameof(XMLSchemas), value); }
  #endregion
}
