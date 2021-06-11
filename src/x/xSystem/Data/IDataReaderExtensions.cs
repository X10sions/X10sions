using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Data {
  public static class IDataReaderExtensions {

    public static string GetStringTrim(this IDataReader reader, int fieldIndex) => reader.GetString(fieldIndex).Trim();
    public static string GetStringTrimEnd(this IDataReader reader, int fieldIndex) => reader.GetString(fieldIndex).TrimEnd();
    public static string GetStringTrimStart(this IDataReader reader, int fieldIndex) => reader.GetString(fieldIndex).TrimStart();

    public static List<T> DataReaderToObjectList<T>(this IDataReader reader, string propertiesToSkip = null, Dictionary<string, PropertyInfo> piList = null)
         where T : new() {
      var list = new List<T>();
      using (reader) {
        // Get a list of PropertyInfo objects we can cache for looping
        if (piList == null) {
          piList = new Dictionary<string, PropertyInfo>();
          foreach (var prop in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            piList.Add(prop.Name.ToLower(), prop);
        }
        while (reader.Read()) {
          var inst = new T();
          reader.DataReaderToObject(inst, propertiesToSkip, piList);
          list.Add(inst);
        }
      }
      return list;
    }

    public static IEnumerable<T> DataReaderToIEnumerable<T>(this IDataReader reader, string propertiesToSkip = null, Dictionary<string, PropertyInfo> piList = null)
          where T : new() {
      if (reader != null) {
        using (reader) {
          // Get a list of PropertyInfo objects we can cache for looping
          if (piList == null) {
            piList = new Dictionary<string, PropertyInfo>();
            foreach (var prop in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
              piList.Add(prop.Name.ToLower(), prop);
          }
          while (reader.Read()) {
            var inst = new T();
            reader.DataReaderToObject(inst, propertiesToSkip, piList);
            yield return inst;
          }
        }
      }
    }

    public static List<T> DataReaderToList<T>(this IDataReader reader, string propertiesToSkip = null, Dictionary<string, PropertyInfo> piList = null)
       where T : new() {
      var list = new List<T>();

      if (reader != null) {
        using (reader) {
          // Get a list of PropertyInfo objects we can cache for looping
          if (piList == null) {
            piList = new Dictionary<string, PropertyInfo>();
            foreach (var prop in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
              piList.Add(prop.Name.ToLower(), prop);
          }
          while (reader.Read()) {
            var inst = new T();
            reader.DataReaderToObject(inst, propertiesToSkip, piList);
            list.Add(inst);
          }
        }
      }
      return list;
    }

    public static void DataReaderToObject(this IDataReader reader, object instance,
                                             string propertiesToSkip = null,
                                             Dictionary<string, PropertyInfo> piList = null) {
      if (reader.IsClosed)
        throw new InvalidOperationException("Resources.DataReaderPassedToDataReaderToObjectCannot");

      if (string.IsNullOrEmpty(propertiesToSkip))
        propertiesToSkip = string.Empty;
      else
        propertiesToSkip = "," + propertiesToSkip + ",";

      propertiesToSkip = propertiesToSkip.ToLower();

      // create a dictionary of properties to look up
      // we can pass this in so we can cache the list once
      // for a list operation
      if (piList == null || piList.Count < 1) {
        if (piList == null)
          piList = new Dictionary<string, PropertyInfo>();
        foreach (var prop in instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
          piList.Add(prop.Name.ToLower(), prop);
      }

      for (var index = 0; index < reader.FieldCount; index++) {
        var name = reader.GetName(index).ToLower();
        if (piList.ContainsKey(name)) {
          var prop = piList[name];
          // skip fields in skip list
          if (!string.IsNullOrEmpty(propertiesToSkip) && propertiesToSkip.Contains("," + name + ","))
            continue;
          // find writable properties and assign
          if ((prop != null) && prop.CanWrite) {
            var val = reader.GetValue(index);
            if (val == DBNull.Value)
              val = null;
            // deal with data drivers return bit values as int64 or in
            else if (prop.PropertyType == typeof(bool) && (val is long || val is int))
              val = (long)val == 1 ? true : false;
            // int conversions when the value is not different type of number
            else if (prop.PropertyType == typeof(int) && (val is long || val is decimal))
              val = Convert.ToInt32(val);

            prop.SetValue(instance, val, null);
          }
        }
      }
    }

    public static DataTable LoadDataTable(this IDataReader dr) {
      var dt = new DataTable();
      dt.Load(dr);
      return dt;
    }

    public static string ToCsv(this IDataReader reader) {
      var sb = new StringBuilder();
      var columnNames = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
      sb.AppendLine(string.Join(",", columnNames));
      while (reader.Read()) {
        for (var i = 0; i < reader.FieldCount; i++) {
          var value = reader[i].ToString();
          if (value.Contains(","))
            value = "\"" + value + "\"";
          sb.Append(value.Replace(Environment.NewLine, " ") + ",");
        }
        sb.Length--; // Remove the last comma
        sb.AppendLine();
      }
      return sb.ToString();
    }

    public static string ToHtmlTable(this IDataReader reader, string tableCssClass = "table") {
      var sb = new StringBuilder($"<table class=\"{ tableCssClass }\"><thead><tr>");
      var schemaTable = reader.GetSchemaTable();
      foreach (DataRow row in schemaTable.Rows) {
        var dataType = row[nameof(DataColumn.DataType)];
        var allowDBNull = row[nameof(DataColumn.AllowDBNull)].As(false);
        var columnName = row[nameof(DataColumn.ColumnName)];
        sb.Append($"<th title=\"{dataType}{(allowDBNull ? "?" : string.Empty)} \">{columnName}</th>");
        //foreach (DataColumn col in schemaTable.Columns) {
        //  sb.Append($"<th title=\"{col.DataType}{(col.AllowDBNull ? "?" : string.Empty)} \">{col.ColumnName}</th>");
        //}
      }
      sb.AppendLine("</tr></thead><tbody>");
      while (reader.Read()) {
        sb.AppendLine("<tr>");
        for (var i = 0; i < reader.FieldCount; i++) {
          sb.Append($"<td>{reader[i]}</td>");
        }
        sb.Append("</tr>");
      }
      sb.AppendLine("</tbody></table>");
      return sb.ToString();
    }

  }
}