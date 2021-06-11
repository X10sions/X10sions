using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System {
  public static class TExtensions {

    public static object GetTypeFieldValueAs<T>(this T obj, string fieldName) => obj.GetTypeFieldValueAs<T, object>(fieldName);
    public static object GetTypePropertyValueAs<T>(this T obj, string propertyName) => obj.GetTypePropertyValueAs<T, object>(propertyName);

    public static TField GetTypeFieldValueAs<T, TField>(this T obj, string fieldName) => obj.GetType().GetFieldValueAs<T, TField>(fieldName, obj);
    public static TProperty GetTypePropertyValueAs<T, TProperty>(this T obj, string propertyName) => obj.GetType().GetPropertyValueAs<T, TProperty>(propertyName, obj);

    // Internal Field/Property helper
    //    public static TField GetTypeFieldValueAs<T, TField>(this T obj, string fieldName) => typeof(T).GetFieldValueAs<T, TField>(fieldName, obj);
    //    public static TProperty GetTypePropertyValueAs<T, TProperty>(this T obj, string propertyName) => typeof(T).GetPropertyValueAs<T, TProperty>(propertyName, obj);

    public static string ToCsv<T>(this IEnumerable<T> objectlist, List<string> excludedPropertyNames = null, bool quoteEveryField = false, bool includeFieldNamesAsFirstRow = true) {
      if (excludedPropertyNames == null) { excludedPropertyNames = new List<string>(); }
      var separator = ",";
      var t = typeof(T);
      var props = t.GetProperties();
      var arrPropNames = props.Where(p => !excludedPropertyNames.Contains(p.Name)).Select(f => f.Name).ToArray();
      var csvBuilder = new StringBuilder();
      if (includeFieldNamesAsFirstRow) {
        if (quoteEveryField) {
          for (var i = 0; i <= arrPropNames.Length - 1; i++) {
            if (i > 0) { csvBuilder.Append(separator); }
            csvBuilder.Append("\"");
            csvBuilder.Append(arrPropNames[i]);
            csvBuilder.Append("\"");
          }
          csvBuilder.Append(Environment.NewLine);

        } else {
          var header = string.Join(separator, arrPropNames);
          csvBuilder.AppendLine(header);
        }
      }
      foreach (var o in objectlist) {
        csvBuilder.AppendCsvRow(excludedPropertyNames, separator, quoteEveryField, props, o);
        csvBuilder.Append(Environment.NewLine);
      }
      return csvBuilder.ToString();
    }

    public static T Set<T>(this T input, Action<T> updater) {
      // https://robvolk.com/linq-select-an-object-but-change-some-properties-without-creating-a-new-object-af4072738e33
      // select some monkeys and modify a property in the select statement instead of creating a new monkey and manually setting all 
      // example:  var list = from monkey in monkeys select monkey.Set(monkey1 => {  monkey1.FavoriteFood += " and banannas"; });
      updater(input);
      return input;
    }

    public static T SetAndReturn<T>(this T newValue, ref T setThis) => setThis = newValue;

    public static T SetAndReturnIfNull<T>(this Func<T> newValueFunc, ref T setThis) {
      if (setThis == null) {
        setThis = newValueFunc();
      }
      return setThis;
    }

    public static T SetAndReturnIfNull<T>(this Func<T> newValueFunc, ref T setThis, Action actionIfNull) {
      if (setThis == null) {
        setThis = newValueFunc();
        actionIfNull();
      }
      return setThis;
    }

    public static string WrapIfNotNull<T>(this T value, string prefix = "", string suffix = "", string defaultIfNull = "") => (value == null) ? defaultIfNull : $"{prefix}{value}{suffix}";

  }
}