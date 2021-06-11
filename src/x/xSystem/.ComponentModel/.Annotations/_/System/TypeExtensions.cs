using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace System {
  public static class TypeExtensions {

    public static string GetColumneName(this Type type) => type.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault()?.Name ?? type.Name;

    public static string GetTableName(this Type type, string schemaQualifier = ".") => type.GetCustomAttributes(false).OfType<TableAttribute>().FirstOrDefault()?.QualifiedTableName(schemaQualifier) ?? type.Name;

  }
}
