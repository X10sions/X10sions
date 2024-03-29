﻿namespace Common.Data.GetSchemaTyped.DataRows;
public class ViewRow : ITypedDataRow {
  public class Query {
    public string? Catalog { get; set; }
    public string? Schema { get; set; }
    public string? Name { get; set; }
    public string? Table { get; set; }

    public string?[] RestrictionValues_ODBC => new[] { Catalog, Schema, Table };
    public string?[] RestrictionValues_IDB2 => new[] { Schema, Name };
  }

  ////IDB2
  //public static DataTable Databases(this DbConnection connection, string name) => connection.GetSchemaDataTable(nameof(Databases), new[] { name });
  //public static DataTable IndexColumns(this DbConnection connection, string schema, string constraint, string name) => connection.GetSchemaDataTable(nameof(IndexColumns), new[] { schema, constraint, name });
  //public static DataTable Schemas(this DbConnection connection, string name) => connection.GetSchemaDataTable(nameof(Schemas), new[] { name });
  //public static DataTable Tables(this DbConnection connection, string schema, string name, string type) => connection.GetSchemaDataTable(nameof(Tables), new[] { schema, name, type });
  //public static DataTable ViewColumns(this DbConnection connection, string schema, string view, string name) => connection.GetSchemaDataTable(nameof(ViewColumns), new[] { schema, view, name });

  public string? TABLE_CAT { get; set; }
  public string? TABLE_SCHEM { get; set; }
  public string? TABLE_NAME { get; set; }
  public string? TABLE_TYPE { get; set; }
  public string? REMARKS { get; set; }

}