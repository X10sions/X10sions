﻿using System.Reflection;

namespace System.Data;
public static class DataRowExtensionsX {

  //public static T? FieldEnum<T>(this DataRow dataRow, string columnName) where T : struct {
  //  var value = dataRow[columnName];
  //  return value.ToEnum<T>();
  //  //var type = typeof(T);
  //  //T? typedValue = value switch {
  //  //  null => null,
  //  //  T t => t,
  //  //  string s => s.ToEnum<T>(),
  //  //  object o => Enum.IsDefined(type, o) ? (T)Enum.ToObject(type, o) : throw new NotImplementedException($"{value}: {value.GetType()}")
  //  //};
  //  //return typedValue;
  //}

  public static void MapToObject(this DataRow dataRow, object obj) {
    foreach (var prop in obj.GetType().GetProperties()) {
      if (dataRow.Table.Columns.Contains(prop.Name)) {
        var value = dataRow[dataRow.Table.Columns[prop.Name]];
        if (!(value is DBNull))
          prop.SetValue(obj, Convert.ChangeType(value, prop.PropertyType), null);
      }
    }
  }

  public const BindingFlags MemberAccess = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase;

  public const BindingFlags MemberPublicInstanceAccess = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

  public static bool CopyDataRow(this DataRow source, DataRow target) {
    var columns = target.Table.Columns;
    for (var x = 0; x < columns.Count; x++) {
      var fieldname = columns[x].ColumnName;
      try {
        target[x] = source[fieldname];
      } catch {; }  // skip any errors
    }
    return true;
  }

  public static void CopyObjectFromDataRow(this DataRow row, object? targetObject, MemberInfo[]? cachedMemberInfo = null) {
    if (targetObject != null && cachedMemberInfo == null) {
      cachedMemberInfo = targetObject.GetType().FindMembers(MemberTypes.Field | MemberTypes.Property, MemberAccess, null, null);
    }
    if (cachedMemberInfo == null) return;
    foreach (var field in cachedMemberInfo) {
      var Name = field.Name;
      if (!row.Table.Columns.Contains(Name))
        continue;

      var value = row[Name];
      if (value == DBNull.Value)
        value = null;

      if (field.MemberType == MemberTypes.Field) {
        ((FieldInfo)field).SetValue(targetObject, value);
      } else if (field.MemberType == MemberTypes.Property) {
        ((PropertyInfo)field).SetValue(targetObject, value, null);
      }
    }
  }

  public static bool CopyObjectToDataRow(this DataRow row, object target) {
    var result = true;
    foreach (var Field in target.GetType().FindMembers(MemberTypes.Field | MemberTypes.Property, MemberAccess, null, null)) {
      var name = Field.Name;
      if (!row.Table.Columns.Contains(name))
        continue;

      try {
        if (Field.MemberType == MemberTypes.Field) {
          row[name] = ((FieldInfo)Field).GetValue(target) ?? DBNull.Value;
        } else if (Field.MemberType == MemberTypes.Property) {
          row[name] = ((PropertyInfo)Field).GetValue(target, null) ?? DBNull.Value;
        }
      } catch { result = false; }
    }

    return result;
  }

  public static DateTime MinimumSqlDate = new DateTime(1900, 1, 1);

  public static void InitializeDataRowWithBlanks(this DataRow row) {
    var loColumns = row.Table.Columns;
    for (var x = 0; x < loColumns.Count; x++) {
      if (!row.IsNull(x))
        continue;
      var lcRowType = loColumns[x].DataType.Name;
      if (lcRowType == "String")
        row[x] = string.Empty;
      else if (lcRowType.StartsWith("Int", StringComparison.Ordinal))
        row[x] = 0;
      else if (lcRowType == "Byte")
        row[x] = 0;
      else if (lcRowType == "Decimal")
        row[x] = 0.00M;
      else if (lcRowType == "Double")
        row[x] = 0.00;
      else if (lcRowType == "Boolean")
        row[x] = false;
      else if (lcRowType == "DateTime")
        row[x] = MinimumSqlDate;

      // Everything else isn't handled explicitly and left alone
      // Byte[] most specifically

    }
  }

  #region mtg.NetFramework.UDrive

  public static string ToHtmlSelectOption(this DataRow @this, string valueField, string textField) => HtmlForOption((@this[valueField]), (@this[textField]), "");

  public static string ToHtmlSelectOption(this DataRow @this, string valueField, string textField, int selectedValue) => HtmlForOption((@this[valueField]), (@this[textField]), selectedValue);

  private static string HtmlForOption(object dataValue, object? dataText = null, object? selectedValue = null) {
    selectedValue = selectedValue ?? string.Empty;
    return $"<option value=\"{(dataText == null ? dataValue.ToString() : dataText.ToString())}\"{dataValue}>{(selectedValue.ToString().Equals(dataValue.ToString(), StringComparison.OrdinalIgnoreCase) ? " selected=\"selected\" " : "")}</option>";
  }

  #endregion
}
