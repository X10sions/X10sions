﻿using LinqToDB;
using LinqToDB.Data;
using LinqToDB.SchemaProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_2_3.TB {
  public class DB2iSeriesSchemaProvider_TB : SchemaProviderBase {
    // http://wiki.midrange.com/index.php/DB2_catalog

    protected override List<ColumnInfo> GetColumns(DataConnection dataConnection, GetSchemaOptions options) {
      var sql = $@"
                Select 
                  Column_text 
                , Data_Type
                , Is_Identity
                , Is_Nullable
                , Length
                , Numeric_Scale
                , Ordinal_Position
                , System_Column_Name
                , System_Table_Name
                , System_Table_Schema
                From QSYS2/SYSCOLUMNS
                 ";
      Func<IDataReader, ColumnInfo> drf = (IDataReader dr) => {
        var ci = new ColumnInfo {
          DataType = dr["Data_Type"].ToString(),
          Description = dr["Column_Text"].ToString(),
          IsIdentity = dr["Is_Identity"].ToString() == "YES",
          IsNullable = dr["Is_Nullable"].ToString() == "Y",
          Length = (long?)dr["Length"],
          Name = dr["System_Column_Name"].ToString(),
          Ordinal = (int)dr["Ordinal_Position"],
          TableID = dataConnection.Connection.Database + "." + dr["System_Table_Schema"] + "." + dr["System_Table_Name"]
        };
        SetColumnParameters(ci, Convert.ToInt64(dr["Length"]), Convert.ToInt32(dr["Numeric_Scale"]));
        return ci;
      };
      return dataConnection.Query(drf, sql).ToList();
    }

    protected override DataType GetDataType(string dataType, string columnType, long? length, int? prec, int? scale) => dataType switch {
      "BIGINT" => DataType.Int64,
      "BINARY" => DataType.Binary,
      "BLOB" => DataType.Blob,
      "CHAR" => DataType.Char,
      "CHAR FOR BIT DATA" => DataType.Binary,
      "CLOB" => DataType.Text,
      "DATALINK" => DataType.Undefined,
      "DATE" => DataType.Date,
      "DBCLOB" => DataType.Undefined,
      "DECIMAL" => DataType.Decimal,
      "DOUBLE" => DataType.Double,
      "GRAPHIC" => DataType.Text,
      "INTEGER" => DataType.Int32,
      "NUMERIC" => DataType.Decimal,
      "REAL" => DataType.Single,
      "ROWID" => DataType.Undefined,
      "SMALLINT" => DataType.Int16,
      "TIME" => DataType.Time,
      "TIMESTAMP" => DataType.Timestamp,
      "VARBINARY" => DataType.VarBinary,
      "VARCHAR" => DataType.VarChar,
      "VARCHAR FOR BIT DATA" => DataType.VarBinary,
      "VARGRAPHIC" => DataType.Text,
      _ => DataType.Undefined
    };

    protected override string GetDbType(GetSchemaOptions options, string columnType, DataTypeInfo dataType, long? length, int? prec, int? scale, string udtCatalog, string udtSchema, string udtName) {
      var dt = GetDataType(columnType, options);
      if (dt != null) {
        if (dt.CreateParameters == null) {
          scale = 0;
          prec = scale;
          length = prec;
        } else {
          if (dt.CreateParameters == "LENGTH") {
            scale = 0;
            prec = scale;
          } else
            length = 0;
          if (dt.CreateFormat == null) {
            if (dt.TypeName.IndexOf("()", StringComparison.Ordinal) >= 0)
              dt.CreateFormat = dt.TypeName.Replace("()", "({0})");
            else {
              var format = string.Join(",", dt.CreateParameters.Split(',').Select((p, i) => "{" + i + "}").ToArray());
              dt.CreateFormat = dt.TypeName + "(" + format + ")";
            }
          }
        }
      }
      return base.GetDbType(options, columnType, dataType, length, prec, scale, udtCatalog, udtSchema, udtName);
    }

    protected override IReadOnlyCollection<ForeignKeyInfo> GetForeignKeys(DataConnection dataConnection, IEnumerable<TableSchema> tables, GetSchemaOptions options) {
      var sql = $@"
          Select ref.Constraint_Name 
          , fk.Ordinal_Position
          , fk.System_Column_Name  As ThisColumn
          , fk.System_Table_Name   As ThisTable
          , fk.System_Table_Schema As ThisSchema
          , uk.System_Column_Name  As OtherColumn
          , uk.System_Table_Schema As OtherSchema
          , uk.System_Table_Name   As OtherTable
          From QSYS2/SYSREFCST ref
          Join QSYS2/SYSKEYCST fk on(fk.Constraint_Schema, fk.Constraint_Name) = (ref.Constraint_Schema, ref.Constraint_Name)
          Join QSYS2/SYSKEYCST uk on(uk.Constraint_Schema, uk.Constraint_Name) = (ref.Unique_Constraint_Schema, ref.Unique_Constraint_Name)
          Where uk.Ordinal_Position = fk.Ordinal_Position
          Order By ThisSchema, ThisTable, Constraint_Name, Ordinal_Position
          ";
      // And {GetSchemaFilter("col.TBCREATOR")}
      Func<IDataReader, ForeignKeyInfo> drf = (IDataReader dr) => {
        return new ForeignKeyInfo() {
          Name = dr["Constraint_Name"].ToString(),
          Ordinal = (int)dr["Ordinal_Position"],
          OtherColumn = dr["OtherColumn"].ToString(),
          OtherTableID = dataConnection.Connection.Database + "." + Convert.ToString(dr["OtherSchema"]) + "." + Convert.ToString(dr["OtherTable"]),
          ThisColumn = dr["ThisColumn"].ToString(),
          ThisTableID = dataConnection.Connection.Database + "." + Convert.ToString(dr["ThisSchema"]) + "." + Convert.ToString(dr["ThisTable"])
        };
      };
      return dataConnection.Query(drf, sql).ToList();
    }

    protected override IReadOnlyCollection<PrimaryKeyInfo> GetPrimaryKeys(DataConnection dataConnection, IEnumerable<TableSchema> tables, GetSchemaOptions options) {
      var sql = $@"
          Select cst.constraint_Name  
               , cst.system_table_SCHEMA
               , cst.system_table_NAME 
               , col.Ordinal_position 
               , col.system_Column_Name   
          From QSYS2/SYSKEYCST col
          Join QSYS2/SYSCST    cst On(cst.constraint_SCHEMA, cst.constraint_NAME, cst.constraint_type) = (col.constraint_SCHEMA, col.constraint_NAME, 'PRIMARY KEY')
          Order By cst.system_table_SCHEMA, cst.system_table_NAME, col.Ordinal_position
          ";
      Func<IDataReader, PrimaryKeyInfo> drf = (IDataReader dr) => {
        return new PrimaryKeyInfo() {
          ColumnName = Convert.ToString(dr["system_Column_Name"]),
          Ordinal = (int)(dr["Ordinal_position"]),
          PrimaryKeyName = Convert.ToString(dr["constraint_Name"]),
          TableID = dataConnection.Connection.Database + "." + Convert.ToString(dr["system_table_SCHEMA"]) + "." + Convert.ToString(dr["system_table_NAME"])
        };
      };
      var _list = dataConnection.Query(drf, sql).ToList();
      return _list;
    }

    protected override List<ProcedureInfo>? GetProcedures(DataConnection dataConnection, GetSchemaOptions options) {
      var sql = $@"
          Select
            CAST(CURRENT_SERVER AS VARCHAR(128)) AS Catalog_Name
          , Function_Type
          , Routine_Definition
          , Routine_Name
          , Routine_Schema
          , Routine_Type
          , Specific_Name
          , Specific_Schema
          From QSYS2/SYSROUTINES 
          Order By Specific_Schema, Specific_Name
          ";
      // And {GetSchemaFilter("col.TBCREATOR")}
      var defaultSchema = dataConnection.Execute<string>("select current_schema from sysibm.sysdummy1");
      Func<IDataReader, ProcedureInfo> drf = (IDataReader dr) => {
        return new ProcedureInfo {
          CatalogName = Convert.ToString(dr["Catalog_Name"]),
          IsDefaultSchema = Convert.ToString(dr["Routine_Schema"]) == defaultSchema,
          IsFunction = Convert.ToString(dr["Routine_Type"]) == "FUNCTION",
          IsTableFunction = Convert.ToString(dr["Function_Type"]) == "T",
          ProcedureDefinition = Convert.ToString(dr["Routine_Definition"]),
          ProcedureID = dataConnection.Connection.Database + "." + Convert.ToString(dr["Specific_Schema"]) + "." + Convert.ToString(dr["Specific_Name"]),
          ProcedureName = Convert.ToString(dr["Routine_Name"]),
          SchemaName = Convert.ToString(dr["Routine_Schema"])
        };
      };
      var _list = dataConnection.Query(drf, sql).ToList();
      return _list;
    }

    protected override List<ProcedureParameterInfo>? GetProcedureParameters(DataConnection dataConnection, IEnumerable<ProcedureInfo> procedures, GetSchemaOptions options) {
      var sql = $@"
          Select 
            CHARACTER_MAXIMUM_LENGTH
          , Data_Type
          , Numeric_Precision
          , Numeric_Scale
          , Ordinal_position
          , Parameter_Mode
          , Parameter_Name
          , Specific_Name
          , Specific_Schema
          From QSYS2/SYSPARMS 
          Order By Specific_Schema, Specific_Name, Parameter_Name
          ";
      // And {GetSchemaFilter("col.TBCREATOR")}
      Func<IDataReader, ProcedureParameterInfo> drf = (IDataReader dr) => {
        return new ProcedureParameterInfo() {
          DataType = Convert.ToString(dr["Parameter_Name"]),
          IsIn = dr["Parameter_Mode"].ToString().Contains("IN"),
          IsOut = dr["Parameter_Mode"].ToString().Contains("OUT"),
          Length = (int)(dr["CHARACTER_MAXIMUM_LENGTH"]),
          Ordinal = (int)(dr["Ordinal_position"]),
          ParameterName = Convert.ToString(dr["Parameter_Name"]),
          Precision = (int?)(dr["Numeric_Precision"]),
          ProcedureID = dataConnection.Connection.Database + "." + Convert.ToString(dr["Specific_Schema"]) + "." + Convert.ToString(dr["Specific_Name"]),
          Scale = (int?)(dr["Numeric_Scale"])
        };
      };
      return dataConnection.Query(drf, sql).ToList();
    }

    protected override string GetProviderSpecificTypeNamespace() => IBM.Data.DB2.iSeries.iDB2Constants.PROVIDER_NAME;

    protected override List<TableInfo> GetTables(DataConnection dataConnection, GetSchemaOptions options) {
      var sql = $@"
                  Select 
                    CAST(CURRENT_SERVER AS VARCHAR(128)) AS Catalog_Name
                  , System_Table_Schema 
                  , System_Table_Name
                  , Table_Text
                  , Table_Type
                  From QSYS2/SYSTABLES 
                  Where Table_Type In('L', 'P', 'T', 'V')
                  Order By System_Table_Schema, System_Table_Name
                 ";
      var defaultSchema = dataConnection.Execute<string>("select current_schema from sysibm.sysdummy1");
      Func<IDataReader, TableInfo> drf = (IDataReader dr) => {
        return new TableInfo() {
          CatalogName = dr["Catalog_Name"].ToString(),
          Description = dr["Table_Text"].ToString(),
          IsDefaultSchema = dr["System_Table_Schema"].ToString() == defaultSchema,
          IsProviderSpecific = dr["System_Table_Schema"].ToString().StartsWith("Q", StringComparison.OrdinalIgnoreCase),
          IsView = new[] { "L", "V" }.Contains(dr["Table_Type"].ToString()),
          SchemaName = dr["System_Table_Schema"].ToString(),
          TableID = dataConnection.Connection.Database + "." + dr["System_Table_Schema"].ToString() + "." + dr["System_Table_Name"].ToString(),
          TableName = dr["System_Table_Name"].ToString()
        };
      };
      var _list = dataConnection.Query(drf, sql).ToList();
      return _list;
    }

    public static void SetColumnParameters(ColumnInfo ci, long? size, int? scale) {
      switch (ci.DataType) {
        case "DECIMAL":
        case "NUMERIC": {
            ci.Precision = (int?)size;
            ci.Scale = scale;
            break;
          }
        case "BINARY":
        case "BLOB":
        case "CHAR":
        case "CHAR FOR BIT DATA":
        case "CLOB":
        case "DATALINK":
        case "DBCLOB":
        case "GRAPHIC":
        case "VARBINARY":
        case "VARCHAR":
        case "VARCHAR FOR BIT DATA":
        case "VARGRAPHIC": {
            ci.Length = size;
            break;
          }

        default: {
            throw new NotImplementedException($"unknown data type: {ci.DataType}");
          }
      }
    }
  }
}
