using Common.Data;
using Common.Data.GetSchemaTyped.DataRows;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped {
  public class GetSchemaHelper {
    public GetSchemaHelper(DbConnection dbConnection) {
      this.dbConnection = dbConnection;
    }
    DbConnection dbConnection;

    public GetSchemaTypedCollection<DataSourceInformationRow> DataSourceInformation() => new GetSchemaTypedCollection<DataSourceInformationRow>(dbConnection, DbMetaDataCollectionNames.DataSourceInformation, x => new DataSourceInformationRow(x), null);
    public GetSchemaTypedCollection<DataTypeRow> DataTypes() => new GetSchemaTypedCollection<DataTypeRow>(dbConnection, DbMetaDataCollectionNames.DataTypes, x => new DataTypeRow(x), null);
    public GetSchemaTypedCollection<MetaDataCollectionRow> MetaDataCollections() => new GetSchemaTypedCollection<MetaDataCollectionRow>(dbConnection, DbMetaDataCollectionNames.MetaDataCollections, x => new MetaDataCollectionRow(x), null);
    public GetSchemaTypedCollection<ReservedWordRow> ReservedWords() => new GetSchemaTypedCollection<ReservedWordRow>(dbConnection, DbMetaDataCollectionNames.ReservedWords, x => new ReservedWordRow(x), null);
    public GetSchemaTypedCollection<RestrictionRow> Restrictions() => new GetSchemaTypedCollection<RestrictionRow>(dbConnection, DbMetaDataCollectionNames.Restrictions, x => new RestrictionRow(x), null);

    public GetSchemaTypedCollection<ColumnRow> Columns(string[] restrictionValues) => new GetSchemaTypedCollection<ColumnRow>(dbConnection, nameof(Columns), x => new ColumnRow(x), restrictionValues);
    public GetSchemaTypedCollection<IndexRow> Indexes(string[] restrictionValues) => new GetSchemaTypedCollection<IndexRow>(dbConnection, nameof(Indexes), x => new IndexRow(x), restrictionValues);
    public GetSchemaTypedCollection<ProcedureRow> Procedures(string[] restrictionValues) => new GetSchemaTypedCollection<ProcedureRow>(dbConnection, nameof(Procedures), x => new ProcedureRow(x), restrictionValues);
    public GetSchemaTypedCollection<ProcedureColumnRow> ProcedureColumns(string[] restrictionValues) => new GetSchemaTypedCollection<ProcedureColumnRow>(dbConnection, nameof(ProcedureColumns), x => new ProcedureColumnRow(x), restrictionValues);
    public GetSchemaTypedCollection<ProcedureParameterRow> ProcedureParameters(string[] restrictionValues) => new GetSchemaTypedCollection<ProcedureParameterRow>(dbConnection, nameof(ProcedureParameters), x => new ProcedureParameterRow(x), restrictionValues);
    public GetSchemaTypedCollection<TableRow> Tables(string[] restrictionValues) => new GetSchemaTypedCollection<TableRow>(dbConnection, nameof(Tables), x => new TableRow(x), restrictionValues);
    public GetSchemaTypedCollection<ViewRow> Views(string[] restrictionValues) => new GetSchemaTypedCollection<ViewRow>(dbConnection, nameof(Views), x => new ViewRow(x), restrictionValues);

    #region GetSchemaHelperExtensions {

    //public GetSchemaTypedDataTable GetSchemaTypedDataTable() => new GetSchemaTypedDataTable(dbConnection);

    //public static GetSchemaTypedCollection<T> GetSchemaTypedCollection<T>(this GetSchemaHelper getSchema, string collectionName, string[] restrictionValues) where T:new()=> new GetSchemaTypedCollection<T>(dbConnection, collectionName, x=> new T(x), restrictionValues);
    public GetSchemaTypedCollection GetSchemaTypedCollection(string collectionName, string[] restrictionValues) => new GetSchemaTypedCollection(dbConnection, collectionName, restrictionValues);

    //ODBC
    //public static string[] ColumnsRestrictions(string catalog, string schema, string tableName, string name) => new[] { catalog, schema, tableName, name };
    //public static string[] IndexesRestrictions(string catalog, string schema, string tableName, string name) => new[] { catalog, schema, tableName, name };
    //public static string[] ProceduresRestrictions(string catalog, string schema, string name, string type) => new[] { catalog, schema, name, type };
    //public static string[] ProcedureColumnsRestrictions(string catalog, string schema, string name, string columnName) => new[] { catalog, schema, name, columnName };
    //public static string[] ProcedureParametersRestrictions(string catalog, string schema, string name, string columnName) => new[] { catalog, schema, name, columnName };
    //public static string[] TablesRestrictions(string catalog, string schema, string name) => new[] { catalog, schema, name };
    //public static string[] ViewsRestrictions(string catalog, string schema, string name) => new[] { catalog, schema, name };

    //ODBC
    //public IEnumerable<ColumnsRow> ColumnsTyped(string catalog, string schema, string tableName, string name) => Columns(catalog, schema, tableName, name).DataRowsAs(row => new GetSchemaTyped_ColumnsRow(row));
    //public IEnumerable<IndexesRow> IndexesTyped(string catalog, string schema, string tableName, string name) => Indexes(catalog, schema, tableName, name).DataRowsAs(row => new GetSchemaTyped_IndexesRow(row));
    //public IEnumerable<ProceduresRow> ProceduresTyped(string catalog, string schema, string name, string type) => Procedures(catalog, schema, name, type).DataRowsAs(row => new GetSchemaTyped_ProceduresRow(row));
    //public IEnumerable<ProcedureColumnsRow> ProcedureColumnsTyped(string catalog, string schema, string name, string columnName) => ProcedureColumns(catalog, schema, name, columnName).DataRowsAs(row => new GetSchemaTyped_ProcedureColumnsRow(row));
    //public IEnumerable<ProcedureParametersRow> ProcedureParametersTyped(string catalog, string schema, string name, string columnName) => ProcedureParameters(catalog, schema, name, columnName).DataRowsAs(row => new GetSchemaTyped_ProcedureParametersRow(row));
    //public IEnumerable<TablesRow> TablesTyped(string catalog, string schema, string name) => Tables(catalog, schema, name).DataRowsAs(row => new GetSchemaTyped_TablesRow(row));
    //public IEnumerable<ViewsRow> ViewsTyped(string catalog, string schema, string name) => Views(catalog, schema, name).DataRowsAs(row => new GetSchemaTyped_ViewsRow(row));


    //ODBC
    //public DataTable Columns(string catalog, string schema, string table, string name) => Extensions.Columns(Connection, catalog, schema, table, name);
    //public DataTable Indexes(string catalog, string schema, string table, string name) => Extensions.Indexes(Connection, catalog, schema, table, name);
    //public DataTable Procedures(string catalog, string schema, string name, string type) => Extensions.Procedures(Connection, catalog, schema, name, type);
    //public DataTable ProcedureColumns(string catalog, string schema, string procedure, string name) => Extensions.ProcedureColumns(Connection, catalog, schema, procedure, name);
    //public DataTable ProcedureParameters(string catalog, string schema, string procedure, string column) => Extensions.ProcedureParameters(Connection, catalog, schema, procedure, column);
    //public DataTable Tables(string catalog, string schema, string name) => Extensions.Tables(Connection, catalog, schema, name);
    //public DataTable Views(string catalog, string schema, string table) => Extensions.Views(Connection, catalog, schema, table);

    //IDB2
    //public DataTable Columns(string schema, string table, string name) => Connection.Columns(schema, table, name);
    //public DataTable Databases(string name) => Extensions.Databases(Connection, name);
    //public DataTable Indexes(string schema, string table, string constraint) => Extensions.Indexes(Connection, schema, table, constraint);
    //public DataTable IndexColumns(string schema, string constraint, string name) => Extensions.IndexColumns(Connection, schema, constraint, name);
    //public DataTable Procedures(string specificSchema, string specificName, string routineType) => Extensions.Procedures(Connection, specificSchema, specificName, routineType);
    //public DataTable ProcedureParameters(string specificSchema, string specificName, string parameter) => Extensions.ProcedureParameters(Connection, specificSchema, specificName, parameter);
    //public DataTable Schemas(string name) => Extensions.Schemas(Connection, name);
    //public DataTable Tables(string schema, string name, string type) => Extensions.Tables(Connection, schema, name, type);
    //public DataTable Views(string schema, string name) => Extensions.Views(Connection, schema, name);
    //public DataTable ViewColumns(string schema, string view, string name) => Extensions.ViewColumns(Connection, schema, view, name);


    //ODBC
    //public static DataTable Columns(this GetSchemaHelper getSchema, string catalog, string schema, string table, string name) => connection.GetSchemaDataTable(nameof(Columns), new[] { catalog, schema, table, name });
    //public static DataTable Indexes(this GetSchemaHelper getSchema, string catalog, string schema, string table, string name) => connection.GetSchemaDataTable(nameof(Indexes), new[] { catalog, schema, table, name });
    //public static DataTable Procedures(this GetSchemaHelper getSchema, string catalog, string schema, string name, string type) => connection.GetSchemaDataTable(nameof(Procedures), new[] { catalog, schema, name, type });
    //public static DataTable ProcedureColumns(this GetSchemaHelper getSchema, string catalog, string schema, string procedure, string name) => connection.GetSchemaDataTable(nameof(ProcedureColumns), new[] { catalog, schema, procedure, name });
    //public static DataTable ProcedureParameters(this GetSchemaHelper getSchema, string catalog, string schema, string procedure, string column) => connection.GetSchemaDataTable(nameof(ProcedureParameters), new[] { catalog, schema, procedure, column });
    //public static DataTable Tables(this GetSchemaHelper getSchema, string catalog, string schema, string name) => connection.GetSchemaDataTable(nameof(Tables), new[] { catalog, schema, name });
    //public static DataTable Views(this GetSchemaHelper getSchema, string catalog, string schema, string table) => connection.GetSchemaDataTable(nameof(Views), new[] { catalog, schema, table });
    ////IDB2
    //public static DataTable Columns(this GetSchemaHelper getSchema, string schema, string table, string name) => connection.GetSchemaDataTable(nameof(Columns), new[] { schema, table, name });
    //public static DataTable Databases(this GetSchemaHelper getSchema, string name) => connection.GetSchemaDataTable(nameof(Databases), new[] { name });
    //public static DataTable Indexes(this GetSchemaHelper getSchema, string schema, string table, string constraint) => connection.GetSchemaDataTable(nameof(Indexes), new[] { schema, table, constraint });
    //public static DataTable IndexColumns(this GetSchemaHelper getSchema, string schema, string constraint, string name) => connection.GetSchemaDataTable(nameof(IndexColumns), new[] { schema, constraint, name });
    //public static DataTable Procedures(this GetSchemaHelper getSchema, string specificSchema, string specificName, string routineType) => connection.GetSchemaDataTable(nameof(Procedures), new[] { specificSchema, specificName, routineType });
    //public static DataTable ProcedureParameters(this GetSchemaHelper getSchema, string specificSchema, string specificName, string parameter) => connection.GetSchemaDataTable(nameof(ProcedureParameters), new[] { specificSchema, specificName, parameter });
    //public static DataTable Schemas(this GetSchemaHelper getSchema, string name) => connection.GetSchemaDataTable(nameof(Schemas), new[] { name });
    //public static DataTable Tables(this GetSchemaHelper getSchema, string schema, string name, string type) => connection.GetSchemaDataTable(nameof(Tables), new[] { schema, name, type });
    //public static DataTable Views(this GetSchemaHelper getSchema, string schema, string name) => connection.GetSchemaDataTable(nameof(Views), new[] { schema, name });
    //public static DataTable ViewColumns(this GetSchemaHelper getSchema, string schema, string view, string name) => connection.GetSchemaDataTable(nameof(ViewColumns), new[] { schema, view, name });

    //Common

    #endregion

  }
}

