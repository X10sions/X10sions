using RepoDb.Interfaces;
using RepoDb.Types;
using System;

namespace RepoDb.Resolvers {
  public class DB2iSeriesDbTypeNameToClientTypeResolver : IResolver<string, Type> {
    public Type Resolve(string dbTypeName) {
      if (dbTypeName == null) {
        throw new NullReferenceException("The DB Type name must not be null.");
      }
      switch (dbTypeName.ToLower()) {
        case "attribute": return typeof(byte[]);
        case "bigint": return typeof(long);
        case "binary": return typeof(byte[]);
        case "bit": return typeof(bool);
        case "blob": return typeof(byte[]);
        case "boolean": return typeof(bool);
        case "char": return typeof(string);
        case "date": return typeof(DateTime);
        case "datetime": return typeof(DateTime);
        case "datetime2": return typeof(DateTime);
        case "datetimeoffset": return typeof(DateTimeOffset);
        case "decimal": return typeof(decimal);
        case "double": return typeof(double);
        case "filestream": return typeof(byte[]);
        case "float": return typeof(double);
        case "image": return typeof(byte[]);
        case "int": return typeof(int);
        case "integer": return typeof(long);
        case "money": return typeof(decimal);
        case "nchar": return typeof(string);
        case "none": return typeof(object);
        case "ntext": return typeof(string);
        case "numeric": return typeof(decimal);
        case "nvarchar": return typeof(string);
        //case "real": return typeof(double);
        //case "real": return typeof(float);
        case "rowversion": return typeof(byte[]);
        case "smalldatetime": return typeof(DateTime);
        case "smallint": return typeof(short);
        case "smallmoney": return typeof(decimal);
        case "sql_variant": return typeof(SqlVariant);
        case "string": return typeof(string);
        case "text": return typeof(string);
        case "time": return typeof(TimeSpan);
        case "timestamp": return typeof(byte[]);
        case "tinyint": return typeof(byte);
        case "uniqueidentifier": return typeof(Guid);
        case "varbinary": return typeof(byte[]);
        case "varbinary(max)": return typeof(byte[]);
        case "varchar": return typeof(string);
        case "xml": return typeof(string);
        default: return typeof(object);
      }
    }
  }

}
