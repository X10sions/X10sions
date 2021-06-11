using Common;
using IBM.Data.DB2.iSeries;
using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.SchemaProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
//using ext = global::System.Data.DataTableExtensions;
using System.Linq;

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9.AS400 {
  [IsCustom(IsCustomReason.ThirdPartyExtension)]
  public class AS400SchemaProvider : SchemaProviderBase {

    readonly HashSet<string> _systemSchemas =GetHashSet(new[] {
      "SYSCAT",
      "SYSFUN",
      "SYSIBM",
      "SYSIBMADM",
      "SYSPROC",
      "SYSPUBLIC",
      "SYSSTAT",
      "SYSTOOLS"
    }, StringComparer.OrdinalIgnoreCase);

    protected string CurrentSchema { get; private set; }

    protected override List<DataTypeInfo> GetDataTypes(DataConnection dataConnection) {
      DataTypesSchema = ((DbConnection)dataConnection.Connection).GetSchema("DataTypes");

      return DataTypesSchema.AsEnumerable()
        .Select(t => new DataTypeInfo {
          TypeName = t.Field<string>("SQL_TYPE_NAME"),
          DataType = t.Field<string>("FRAMEWORK_TYPE"),
          CreateParameters = t.Field<string>("CREATE_PARAMS"),
        })
        .Union(
        new[] {
          new DataTypeInfo {
            TypeName = "CHARACTER",
            CreateParameters = "LENGTH",
            DataType = "System.String"
          }
        }).ToList();
    }

    protected override List<TableInfo> GetTables(DataConnection dataConnection, GetSchemaOptions options) {
      LoadCurrentSchema(dataConnection);
      var tables = ((DbConnection)dataConnection.Connection).GetSchema("Tables");
      return (
        from t in tables.AsEnumerable()
        where
          new[] { "TABLE", "VIEW" }.Contains(t.Field<string>("TABLE_TYPE"))
        let catalog = dataConnection.Connection.Database
        let schema = t.Field<string>("TABLE_SCHEMA")
        let name = t.Field<string>("TABLE_NAME")
        let system = t.Field<string>("TABLE_TYPE") == "SYSTEM TABLE"
        where IncludedSchemas.Count != 0 || ExcludedSchemas.Count != 0 || schema == CurrentSchema
        select new TableInfo {
          CatalogName = catalog,
          Description = t.Field<string>("REMARKS"),
          IsDefaultSchema = string.IsNullOrEmpty(schema),
          IsProviderSpecific = system || _systemSchemas.Contains(schema),
          IsView = t.Field<string>("TABLE_TYPE") == "VIEW",
          SchemaName = schema,
          TableID = catalog + '.' + schema + '.' + name,
          TableName = name
        }
      ).ToList();
    }

    protected void LoadCurrentSchema(DataConnection dataConnection) {
      if (CurrentSchema == null) {
        CurrentSchema = dataConnection.Execute<string>($"select current_schema from { iDB2Constants.DummyTableName()}");
      }
    }

    protected override IReadOnlyCollection<PrimaryKeyInfo> GetPrimaryKeys(DataConnection dataConnection, IEnumerable<TableSchema> tables, GetSchemaOptions options) => (
        from pk in dataConnection.Query(
          rd => new {
            id = dataConnection.Connection.Database + "." + rd.GetStringTrim(0) + "." + rd.GetStringTrim(1),
            name = rd.GetStringTrim(2),
            cols = rd.GetStringTrim(3).Split('+').Skip(1).ToArray(),
          }, @"
          SELECT
            TABSCHEMA,
            TABNAME,
            INDNAME,
            COLNAMES
          FROM
            SYSCAT.INDEXES
          WHERE
            UNIQUERULE = 'P' AND " + GetSchemaFilter("TABSCHEMA"))
        from col in pk.cols.Select((c, i) => new { c, i })
        select new PrimaryKeyInfo {
          TableID = pk.id,
          PrimaryKeyName = pk.name,
          ColumnName = col.c,
          Ordinal = col.i
        }
      ).ToList();

    List<ColumnInfo> _columns;

    protected override List<ColumnInfo> GetColumns(DataConnection dataConnection, GetSchemaOptions options) {
      var sql = @"
        SELECT
          TABSCHEMA,
          TABNAME,
          COLNAME,
          LENGTH,
          SCALE,
          NULLS,
          IDENTITY,
          COLNO,
          TYPENAME,
          REMARKS,
          CODEPAGE
        FROM
          SYSCAT.COLUMNS
        WHERE
          " + GetSchemaFilter("TABSCHEMA");

      return _columns = dataConnection.Query(rd => {
        var typeName = rd.GetStringTrim(8);
        var cp = Converter.ChangeTypeTo<int>(rd[10]);

        if (typeName == "CHARACTER" && cp == 0)
          typeName = "CHAR () FOR BIT DATA";
        else if (typeName == "VARCHAR" && cp == 0)
          typeName = "VARCHAR () FOR BIT DATA";

        var ci = new ColumnInfo {
          TableID = dataConnection.Connection.Database + "." + rd.GetStringTrim(0) + "." + rd.GetStringTrim(1),
          Name = rd.GetStringTrim(2),
          IsNullable = rd.GetStringTrim(5) == "Y",
          IsIdentity = rd.GetStringTrim(6) == "Y",
          Ordinal = Converter.ChangeTypeTo<int>(rd[7]),
          DataType = typeName,
          Description = rd.GetStringTrim(9),
        };
        SetColumnParameters(ci, Converter.ChangeTypeTo<long?>(rd[3]), Converter.ChangeTypeTo<int?>(rd[4]));
        return ci;
      },
        sql).ToList();
    }

    static void SetColumnParameters(ColumnInfo ci, long? size, int? scale) {
      switch (ci.DataType) {
        case "DECIMAL":
        case "DECFLOAT":
          if ((size ?? 0) > 0)
            ci.Precision = (int?)size.Value;
          if ((scale ?? 0) > 0)
            ci.Scale = scale;
          break;
        case "DBCLOB":
        case "CLOB":
        case "BLOB":
        case "LONG VARGRAPHIC":
        case "VARGRAPHIC":
        case "GRAPHIC":
        case "LONG VARCHAR FOR BIT DATA":
        case "VARCHAR () FOR BIT DATA":
        case "VARBIN":
        case "BINARY":
        case "CHAR () FOR BIT DATA":
        case "LONG VARCHAR":
        case "CHARACTER":
        case "CHAR":
        case "VARCHAR":
          ci.Length = size;
          break;
      }
    }

    protected override IReadOnlyCollection<ForeignKeyInfo> GetForeignKeys(DataConnection dataConnection, IEnumerable<TableSchema> tables, GetSchemaOptions options) => dataConnection.Query(rd => new {
      name = rd.GetStringTrim(0),
      thisTable = dataConnection.Connection.Database + "." + rd.GetStringTrim(1) + "." + rd.GetStringTrim(2),
      thisColumns = rd.GetStringTrim(3),
      otherTable = dataConnection.Connection.Database + "." + rd.GetStringTrim(4) + "." + rd.GetStringTrim(5),
      otherColumns = rd.GetStringTrim(6),
    }, @"
          SELECT
            CONSTNAME,
            TABSCHEMA,
            TABNAME,
            FK_COLNAMES,
            REFTABSCHEMA,
            REFTABNAME,
            PK_COLNAMES
          FROM
            SYSCAT.REFERENCES
          WHERE
            " + GetSchemaFilter("TABSCHEMA"))
        .SelectMany(fk => {
          var thisTable = _columns.Where(c => c.TableID == fk.thisTable).OrderByDescending(c => c.Length).ToList();
          var otherTable = _columns.Where(c => c.TableID == fk.otherTable).OrderByDescending(c => c.Length).ToList();
          var thisColumns = fk.thisColumns.Trim();
          var otherColumns = fk.otherColumns.Trim();
          var list = new List<ForeignKeyInfo>();
          for (var i = 0; thisColumns.Length > 0; i++) {
            var thisColumn = thisTable.FirstOrDefault(c => thisColumns.StartsWith(c.Name, StringComparison.Ordinal));
            if (thisColumn == null) {
              continue;
            }
            var otherColumn = otherTable.FirstOrDefault(c => otherColumns.StartsWith(c.Name, StringComparison.Ordinal));
            if (otherColumn == null) {
              continue;
            }
            list.Add(new ForeignKeyInfo {
              Name = fk.name,
              ThisTableID = fk.thisTable,
              OtherTableID = fk.otherTable,
              Ordinal = i,
              ThisColumn = thisColumn.Name,
              OtherColumn = otherColumn.Name,
            });
            thisColumns = thisColumns.Substring(thisColumn.Name.Length).Trim();
            otherColumns = otherColumns.Substring(otherColumn.Name.Length).Trim();
          }
          return list;
        }).ToList();

    protected override string GetDbType(GetSchemaOptions options, string columnType, DataTypeInfo dataType, long? length, int? prec, int? scale, string udtCatalog, string udtSchema, string udtName) {
      var type = DataTypes.FirstOrDefault(dt => dt.TypeName == columnType);
      if (type != null) {
        if (type.CreateParameters == null)
          length = prec = scale = 0;
        else {
          if (type.CreateParameters == "LENGTH")
            prec = scale = 0;
          else
            length = 0;
          if (type.CreateFormat == null) {
            if (type.TypeName.IndexOf("()", StringComparison.Ordinal) >= 0) {
              type.CreateFormat = type.TypeName.Replace("()", "({0})");
            } else {
              var format = string.Join(",",
                type.CreateParameters
                  .Split(',')
                  .Select((p, i) => "{" + i + "}")
                  .ToArray());
              type.CreateFormat = type.TypeName + "(" + format + ")";
            }
          }
        }
      }
      return base.GetDbType(options, columnType, dataType, length, prec, scale, udtCatalog, udtSchema, udtName);
    }

    protected override DataType GetDataType(string dataType, string columnType, long? length, int? prec, int? scale) {
      switch (dataType) {
        case "BIGINT":
          return DataType.Int64;     // BigInt          System.Int64
        case "BINARY":
          return DataType.Binary;    // Binary          System.Byte[]
        case "BLOB":
          return DataType.Blob;      // Blob            System.Byte[]
        case "CHAR () FOR BIT DATA":
          return DataType.Binary;    // Binary          System.Byte[]
        case "CHAR FOR BIT DATA":
          return DataType.Binary;
        case "CHAR":
          return DataType.Char;      // Char            System.String
        case "CHARACTER":
          return DataType.Char;      // Char            System.String
        case "CLOB":
          return DataType.Text;      // Clob            System.String
        case "DATALINK":
          return DataType.Undefined;
        case "DATE":
          return DataType.Date;      // Date            System.DateTime
        case "DBCLOB":
          return DataType.Text;      // DbClob          System.String
        case "DECFLOAT":
          return DataType.Decimal;   // DecimalFloat    System.Decimal
        case "DECIMAL":
          return DataType.Decimal;   // Decimal         System.Decimal
        case "DOUBLE":
          return DataType.Double;    // Double          System.Double
        case "GRAPHIC":
          return DataType.Text;      // Graphic         System.String
        case "INTEGER":
          return DataType.Int32;     // Integer         System.Int32
        case "LONG VARCHAR FOR BIT DATA":
          return DataType.VarBinary; // LongVarBinary   System.Byte[]
        case "LONG VARCHAR":
          return DataType.VarChar;   // LongVarChar     System.String
        case "LONG VARGRAPHIC":
          return DataType.Text;      // LongVarGraphic  System.String
        case "NUMERIC":
          return DataType.Decimal;
        case "REAL":
          return DataType.Single;    // Real            System.Single
        case "ROWID":
          return DataType.Undefined; // RowID           System.Byte[]
        case "SMALLINT":
          return DataType.Int16;     // SmallInt        System.Int16
        case "TIME":
          return DataType.Time;      // Time            System.TimeSpan
        case "TIMESTAMP":
          return DataType.Timestamp; // Timestamp       System.DateTime
        case "TIMESTMP":
          return DataType.Timestamp; // Timestamp       System.DateTime
        case "VARBIN":
          return DataType.VarBinary; // VarBinary       System.Byte[]
        case "VARBINARY":
          return DataType.VarBinary;
        case "VARCHAR () FOR BIT DATA":
          return DataType.VarBinary; // VarBinary       System.Byte[]
        case "VARCHAR FOR BIT DATA":
          return DataType.VarBinary;
        case "VARCHAR":
          return DataType.VarChar;   // VarChar         System.String
        case "VARGRAPHIC":
          return DataType.Text;      // VarGraphic      System.String
        case "XML":
          return DataType.Xml;       // Xml             System.String
      }
      throw new NotImplementedException($"{nameof(GetDataType)} unknown: {dataType}");
    }

    protected override string GetProviderSpecificTypeNamespace() => typeof(iDB2Connection).Namespace;

    protected override string GetProviderSpecificType(string dataType) {
      switch (dataType) {
        case "XML":
          return nameof(iDB2DbType.iDB2Xml);
        case "DECFLOAT":
          return nameof(iDB2DbType.iDB2DecFloat16);
        case "DBCLOB":
          return nameof(iDB2DbType.iDB2DbClob);
        case "CLOB":
          return nameof(iDB2DbType.iDB2Clob);
        case "BLOB":
          return nameof(iDB2DbType.iDB2Blob);
        case "BIGINT":
          return nameof(iDB2DbType.iDB2BigInt);
        case "LONG VARCHAR FOR BIT DATA":
          return nameof(iDB2DbType.iDB2VarCharBitData);
        case "VARCHAR () FOR BIT DATA":
          return nameof(iDB2DbType.iDB2Binary);
        case "VARBIN":
          return nameof(iDB2DbType.iDB2VarBinary);
        case "BINARY":
          return nameof(iDB2DbType.iDB2Binary);
        case "CHAR () FOR BIT DATA":
          return nameof(iDB2DbType.iDB2CharBitData);
        case "LONG VARGRAPHIC":
          return nameof(iDB2DbType.iDB2VarGraphic);
        case "VARGRAPHIC":
          return nameof(iDB2DbType.iDB2VarGraphic);
        case "GRAPHIC":
          return nameof(iDB2DbType.iDB2Graphic);
        case "LONG VARCHAR":
          return nameof(iDB2DbType.iDB2VarChar);
        case "CHARACTER":
          return nameof(iDB2DbType.iDB2Char);
        case "VARCHAR":
          return nameof(iDB2DbType.iDB2VarChar);
        case "CHAR":
          return nameof(iDB2DbType.iDB2Char);
        case "DECIMAL":
          return nameof(iDB2DbType.iDB2Decimal);
        case "INTEGER":
          return nameof(iDB2DbType.iDB2Integer);
        case "SMALLINT":
          return nameof(iDB2DbType.iDB2SmallInt);
        case "REAL":
          return nameof(iDB2DbType.iDB2Real);
        case "DOUBLE":
          return nameof(iDB2DbType.iDB2Double);
        case "DATE":
          return nameof(iDB2DbType.iDB2Date);
        case "TIME":
          return nameof(iDB2DbType.iDB2Time);
        case "TIMESTMP":
          return nameof(iDB2DbType.iDB2TimeStamp);
        case "TIMESTAMP":
          return nameof(iDB2DbType.iDB2TimeStamp);
        case "ROWID":
          return nameof(iDB2DbType.iDB2Rowid);
      }
      throw new NotImplementedException($"{nameof(GetProviderSpecificType)} unknown: {dataType}");
      //  return base.GetProviderSpecificType(dataType);
    }

    protected override List<ProcedureInfo> GetProcedures(DataConnection dataConnection, GetSchemaOptions options) {
      LoadCurrentSchema(dataConnection);
      var sql = $" SELECT PROCSCHEMA, PROCNAME FROM SYSCAT.PROCEDURES WHERE	{GetSchemaFilter("PROCSCHEMA")}";
      if (IncludedSchemas.Count == 0) {
        sql += " AND PROCSCHEMA NOT IN ('SYSPROC', 'SYSIBMADM', 'SQLJ', 'ADMINISTRATOR', 'SYSIBM')";
      }
      return dataConnection.Query(rd => {
        var schema = rd.GetStringTrim(0);
        var name = rd.GetStringTrim(1);
        return new ProcedureInfo {
          ProcedureID = dataConnection.Connection.Database + "." + schema + "." + name,
          CatalogName = dataConnection.Connection.Database,
          SchemaName = schema,
          ProcedureName = name,
        };
      }, sql)
        .Where(p => IncludedSchemas.Count != 0 || ExcludedSchemas.Count != 0 || p.SchemaName == CurrentSchema)
        .ToList();
    }

    protected override List<ProcedureParameterInfo> GetProcedureParameters(DataConnection dataConnection, IEnumerable<ProcedureInfo> procedures, GetSchemaOptions options) => dataConnection.Query(rd => {
      var schema = rd.GetStringTrim(0);
      var procname = rd.GetStringTrim(1);
      var length = ConvertTo<long?>.From(rd["LENGTH"]);
      var scale = ConvertTo<int?>.From(rd["SCALE"]);
      var mode = ConvertTo<string>.From(rd[4]);
      var ppi = new ProcedureParameterInfo {
        ProcedureID = dataConnection.Connection.Database + "." + schema + "." + procname,
        ParameterName = rd.GetStringTrim(2),
        DataType = rd.GetStringTrim(3),
        Ordinal = ConvertTo<int>.From(rd["ORDINAL"]),
        IsIn = mode.Contains("IN"),
        IsOut = mode.Contains("OUT"),
        IsResult = false
      };
      var ci = new ColumnInfo { DataType = ppi.DataType };
      SetColumnParameters(ci, length, scale);
      ppi.Length = ci.Length;
      ppi.Precision = ci.Precision;
      ppi.Scale = ci.Scale;

      return ppi;
    }, @"
          SELECT
            PROCSCHEMA,
            PROCNAME,
            PARMNAME,
            TYPENAME,
            PARM_MODE,
            ORDINAL,
            LENGTH,
            SCALE
          FROM
            SYSCAT.PROCPARMS
          WHERE
            " + GetSchemaFilter("PROCSCHEMA"))
        .ToList();

    protected string GetSchemaFilter(string schemaNameField) {
      if (IncludedSchemas.Count != 0 || ExcludedSchemas.Count != 0) {
        var sql = schemaNameField;
        if (IncludedSchemas.Count != 0) {
          sql += string.Format(" IN ({0})", IncludedSchemas.Select(n => '\'' + n + '\'').Aggregate((s1, s2) => s1 + ',' + s2));
          if (ExcludedSchemas.Count != 0) {
            sql += " AND " + schemaNameField;
          }
        }
        if (ExcludedSchemas.Count != 0) {
          sql += string.Format(" NOT IN ({0})", ExcludedSchemas.Select(n => '\'' + n + '\'').Aggregate((s1, s2) => s1 + ',' + s2));
        }
        return sql;
      }
      return string.Format("{0} = '{1}'", schemaNameField, CurrentSchema);
    }

  }

}