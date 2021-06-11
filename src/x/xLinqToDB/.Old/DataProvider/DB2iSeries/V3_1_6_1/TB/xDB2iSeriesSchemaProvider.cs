using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.SchemaProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.TB {
  public class xDB2iSeriesSchemaProvider : SchemaProviderBase {
    private string schemaTableSeparator;

    private string providerSpecificTypeNamespace;

    public xDB2iSeriesSchemaProvider(xDB2iSeriesDataProviderOptions dataProviderOptions, string providerSpecificTypeNamespace) {
      schemaTableSeparator = dataProviderOptions.NamingConvention.Separator();
      this.providerSpecificTypeNamespace = providerSpecificTypeNamespace;
    }

    protected override DataType GetDataType(string? dataType, string? columnType, long? length, int? prec, int? scale) {
      switch (dataType) {
        case "BIGINT": return DataType.Int64;
        case "BINARY": return DataType.Binary;
        case "BLOB": return DataType.Blob;
        case "CHAR": return DataType.Char;
        case "CHAR FOR BIT DATA": return DataType.Binary;
        case "CLOB": return DataType.Text;
        case "DATALINK": return DataType.Undefined;
        case "DATE": return DataType.Date;
        case "DBCLOB": return DataType.Undefined;
        case "DECIMAL": return DataType.Decimal;
        case "DOUBLE": return DataType.Double;
        case "GRAPHIC": return DataType.Text;
        case "INTEGER": return DataType.Int32;
        case "NUMERIC": return DataType.Decimal;
        case "REAL": return DataType.Single;
        case "ROWID": return DataType.Undefined;
        case "SMALLINT": return DataType.Int16;
        case "TIME": return DataType.Time;
        case "TIMESTAMP": return DataType.Timestamp;
        case "VARBINARY": return DataType.VarBinary;
        case "VARCHAR": return DataType.VarChar;
        case "VARCHAR FOR BIT DATA": return DataType.VarBinary;
        case "VARGRAPHIC": return DataType.Text;
        default: throw new NotImplementedException(dataType);
      }
    }

    protected override List<ColumnInfo> GetColumns(DataConnection dataConnection, GetSchemaOptions options) {
      var dataConnection2 = dataConnection;
      var sql = $@"
Select
 Column_text
, Case when CCSID = 65535 and Data_Type in ('CHAR', 'VARCHAR') then Data_Type || ' FOR BIT DATA' else Data_Type end as Data_Type
, Is_Identity
, Is_Nullable
, Length
, COALESCE(Numeric_Scale,0) Numeric_Scale
, Ordinal_Position
, Column_Name
, Table_Name
, Table_Schema
, Column_Name
From QSYS2{schemaTableSeparator}SYSCOLUMNS
where System_Table_Schema in('{GetLibraryList(dataConnection2)}')
";

      return dataConnection2.Query(new Func<IDataReader, ColumnInfo>(drf), sql).ToList();
      ColumnInfo drf(IDataReader dr) {
        var obj = new ColumnInfo {
          DataType = dr["Data_Type"].ToString().TrimEnd(Array.Empty<char>()),
          Description = dr["Column_Text"].ToString().TrimEnd(Array.Empty<char>()),
          IsIdentity = (dr["Is_Identity"].ToString().TrimEnd(Array.Empty<char>()) == "YES"),
          IsNullable = (dr["Is_Nullable"].ToString().TrimEnd(Array.Empty<char>()) == "Y"),
          Name = dr["Column_Name"].ToString().TrimEnd(Array.Empty<char>()),
          Ordinal = Converter.ChangeTypeTo<int>(dr["Ordinal_Position"]),
          TableID = dataConnection2.Connection.Database + "." + Convert.ToString(dr["Table_Schema"]).TrimEnd(Array.Empty<char>()) + "." + Convert.ToString(dr["Table_Name"]).TrimEnd(Array.Empty<char>())
        };
        SetColumnParameters(obj, Convert.ToInt64(dr["Length"]), Convert.ToInt32(dr["Numeric_Scale"]));
        return obj;
      }
    }

    protected override IReadOnlyCollection<ForeignKeyInfo> GetForeignKeys(DataConnection dataConnection, IEnumerable<TableSchema> tables, GetSchemaOptions options) {
      var dataConnection2 = dataConnection;
      var sql = $@"
Select ref.Constraint_Name
, fk.Ordinal_Position
, fk.Column_Name  As ThisColumn
, fk.Table_Name   As ThisTable
, fk.Table_Schema As ThisSchema
, uk.Column_Name  As OtherColumn
, uk.Table_Schema As OtherSchema
, uk.Table_Name   As OtherTable
From QSYS2{schemaTableSeparator}SYSREFCST ref
Join QSYS2{schemaTableSeparator}SYSKEYCST fk on(fk.Constraint_Schema, fk.Constraint_Name) = (ref.Constraint_Schema, ref.Constraint_Name)
Join QSYS2{schemaTableSeparator}SYSKEYCST uk on(uk.Constraint_Schema, uk.Constraint_Name) = (ref.Unique_Constraint_Schema, ref.Unique_Constraint_Name)
Where uk.Ordinal_Position = fk.Ordinal_Position
  And fk.System_Table_Schema in('{GetLibraryList(dataConnection2)}')
Order By ThisSchema, ThisTable, Constraint_Name, Ordinal_Position
";
      return dataConnection2.Query(new Func<IDataReader, ForeignKeyInfo>(drf), sql).ToList();
      ForeignKeyInfo drf(IDataReader dr) => new ForeignKeyInfo {
        Name = dr["Constraint_Name"].ToString().TrimEnd(Array.Empty<char>()),
        Ordinal = Converter.ChangeTypeTo<int>(dr["Ordinal_Position"]),
        OtherColumn = dr["OtherColumn"].ToString().TrimEnd(Array.Empty<char>()),
        OtherTableID = dataConnection2.Connection.Database + "." + Convert.ToString(dr["OtherSchema"]).TrimEnd(Array.Empty<char>()) + "." + Convert.ToString(dr["OtherTable"]).TrimEnd(Array.Empty<char>()),
        ThisColumn = dr["ThisColumn"].ToString().TrimEnd(Array.Empty<char>()),
        ThisTableID = dataConnection2.Connection.Database + "." + Convert.ToString(dr["ThisSchema"]).TrimEnd(Array.Empty<char>()) + "." + Convert.ToString(dr["ThisTable"]).TrimEnd(Array.Empty<char>())
      };
    }

    protected override IReadOnlyCollection<PrimaryKeyInfo> GetPrimaryKeys(DataConnection dataConnection, IEnumerable<TableSchema> tables, GetSchemaOptions options) {
      var dataConnection2 = dataConnection;
      var sql = $@"
Select cst.constraint_Name
, cst.table_SCHEMA
, cst.table_NAME
, col.Ordinal_position
, col.Column_Name
From QSYS2{schemaTableSeparator}SYSKEYCST col
Join QSYS2{schemaTableSeparator}SYSCST    cst On(cst.constraint_SCHEMA, cst.constraint_NAME, cst.constraint_type) = (col.constraint_SCHEMA, col.constraint_NAME, 'PRIMARY KEY')
And cst.System_Table_Schema in('{GetLibraryList(dataConnection2)}')
Order By cst.table_SCHEMA, cst.table_NAME, col.Ordinal_position
";
      return dataConnection2.Query(new Func<IDataReader, PrimaryKeyInfo>(drf), sql).ToList();
      PrimaryKeyInfo drf(IDataReader dr) => new PrimaryKeyInfo {
        ColumnName = Convert.ToString(dr["Column_Name"]).TrimEnd(Array.Empty<char>()),
        Ordinal = Converter.ChangeTypeTo<int>(dr["Ordinal_position"]),
        PrimaryKeyName = Convert.ToString(dr["constraint_Name"]).TrimEnd(Array.Empty<char>()),
        TableID = dataConnection2.Connection.Database + "." + Convert.ToString(dr["table_SCHEMA"]).TrimEnd(Array.Empty<char>()) + "." + Convert.ToString(dr["table_NAME"]).TrimEnd(Array.Empty<char>())
      };
    }

    protected override List<ProcedureInfo> GetProcedures(DataConnection dataConnection, GetSchemaOptions options) {
      var dataConnection2 = dataConnection;
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
From QSYS2{schemaTableSeparator}SYSROUTINES
Where Specific_Schema in('{GetLibraryList(dataConnection2)}')
Order By Specific_Schema, Specific_Name
";
      var defaultSchema = dataConnection2.Execute<string>("select current_schema from sysibm.sysdummy1");
      return dataConnection2.Query(new Func<IDataReader, ProcedureInfo>(drf), sql).ToList();
      ProcedureInfo drf(IDataReader dr) => new ProcedureInfo {
        CatalogName = Convert.ToString(dr["Catalog_Name"]).TrimEnd(Array.Empty<char>()),
        IsDefaultSchema = (Convert.ToString(dr["Routine_Schema"]).TrimEnd(Array.Empty<char>()) == defaultSchema),
        IsFunction = (Convert.ToString(dr["Routine_Type"]) == "FUNCTION"),
        IsTableFunction = (Convert.ToString(dr["Function_Type"]) == "T"),
        ProcedureDefinition = Convert.ToString(dr["Routine_Definition"]).TrimEnd(Array.Empty<char>()),
        ProcedureID = dataConnection2.Connection.Database + "." + Convert.ToString(dr["Specific_Schema"]).TrimEnd(Array.Empty<char>()) + "." + Convert.ToString(dr["Specific_Name"]).TrimEnd(Array.Empty<char>()),
        ProcedureName = Convert.ToString(dr["Routine_Name"]).TrimEnd(Array.Empty<char>()),
        SchemaName = Convert.ToString(dr["Routine_Schema"]).TrimEnd(Array.Empty<char>())
      };
    }

    protected override List<ProcedureParameterInfo> GetProcedureParameters(DataConnection dataConnection, IEnumerable<ProcedureInfo> procedures, GetSchemaOptions options) {
      var dataConnection2 = dataConnection;
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
From QSYS2{schemaTableSeparator}SYSPARMS
where Specific_Schema in('{GetLibraryList(dataConnection2)}')
Order By Specific_Schema, Specific_Name, Parameter_Name
";
      return dataConnection2.Query(new Func<IDataReader, ProcedureParameterInfo>(drf), sql).ToList();
      ProcedureParameterInfo drf(IDataReader dr) => new ProcedureParameterInfo {
        DataType = Convert.ToString(dr["Parameter_Name"]),
        IsIn = dr["Parameter_Mode"].ToString().Contains("IN"),
        IsOut = dr["Parameter_Mode"].ToString().Contains("OUT"),
        Length = Converter.ChangeTypeTo<long?>(dr["CHARACTER_MAXIMUM_LENGTH"]),
        Ordinal = Converter.ChangeTypeTo<int>(dr["Ordinal_position"]),
        ParameterName = Convert.ToString(dr["Parameter_Name"]).TrimEnd(Array.Empty<char>()),
        Precision = Converter.ChangeTypeTo<int?>(dr["Numeric_Precision"]),
        ProcedureID = dataConnection2.Connection.Database + "." + Convert.ToString(dr["Specific_Schema"]).TrimEnd(Array.Empty<char>()) + "." + Convert.ToString(dr["Specific_Name"]).TrimEnd(Array.Empty<char>()),
        Scale = Converter.ChangeTypeTo<int?>(dr["Numeric_Scale"])
      };
    }

    protected override string GetProviderSpecificTypeNamespace() => providerSpecificTypeNamespace;

    protected override List<TableInfo> GetTables(DataConnection dataConnection, GetSchemaOptions options) {
      var dataConnection2 = dataConnection;
      var sql = $@"
Select
CAST(CURRENT_SERVER AS VARCHAR(128)) AS Catalog_Name
, Table_Schema
, Table_Name
, Table_Text
, Table_Type
, System_Table_Schema
From QSYS2/SYSTABLES
Where Table_Type In('L', 'P', 'T', 'V')
And System_Table_Schema in ('{GetLibraryList(dataConnection2)}')
Order By System_Table_Schema, System_Table_Name
";
      var defaultSchema = dataConnection2.Execute<string>("select current_schema from sysibm.sysdummy1");
      return dataConnection2.Query(new Func<IDataReader, TableInfo>(drf), sql).ToList();
      TableInfo drf(IDataReader dr) => new TableInfo {
        CatalogName = dr["Catalog_Name"].ToString().TrimEnd(Array.Empty<char>()),
        Description = dr["Table_Text"].ToString().TrimEnd(Array.Empty<char>()),
        IsDefaultSchema = (dr["System_Table_Schema"].ToString().TrimEnd(Array.Empty<char>()) == defaultSchema),
        IsView = new string[2]
          {
          "L",
          "V"
          }.Contains(dr["Table_Type"].ToString()),
        SchemaName = dr["Table_Schema"].ToString().TrimEnd(Array.Empty<char>()),
        TableID = dataConnection2.Connection.Database + "." + dr["Table_Schema"].ToString().TrimEnd(Array.Empty<char>()) + "." + dr["Table_Name"].ToString().TrimEnd(Array.Empty<char>()),
        TableName = dr["Table_Name"].ToString().TrimEnd(Array.Empty<char>())
      };
    }

    private static void SetColumnParameters(ColumnInfo ci, long? size, int? scale) {
      switch (ci.DataType) {
        case "INTEGER": break;
        case "SMALLINT": break;
        case "BIGINT": break;
        case "TIMESTMP": break;
        case "DATE": break;
        case "TIME": break;
        case "VARG": break;
        case "DECFLOAT": break;
        case "FLOAT": break;
        case "ROWID": break;
        case "VARBIN": break;
        case "XML": break;
        case "DECIMAL":
        case "NUMERIC":
          if (size > 0) {
            ci.Precision = (int)size.Value;
          }
          if (scale > 0) {
            ci.Scale = scale;
          }
          break;
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
        case "VARGRAPHIC":
          ci.Length = size;
          break;
        default:
          throw new NotImplementedException("unknown data type: " + ci.DataType);
      }
    }

    private static string GetLibraryList(DataConnection dataConnection) {
      var csb = new DbConnectionStringBuilder {
        ConnectionString = dataConnection.ConnectionString
      };
      var libList = csb["DefaultLibraries"].ToString() ?? csb["DBQ"].ToString() ?? csb["LibraryList"].ToString() ?? string.Empty;
      return string.Join("', '", libList.Split(new char[1]
      {
      ','
      }));
    }
  }

}
