using Common.Models;

namespace Common.ValueObjects;

public readonly record struct Difference(string PropertyName, object ValueBefore, object ValueAfter) {

  public static List<Difference> ListFields<T>(T obj1, T obj2, params string[] excludeFieldNames) {
    var differences = new List<Difference>();
    var fields = typeof(T).GetFields();
    foreach (var field in fields) {
      if (excludeFieldNames.Contains(field.Name)) {
        continue;
      }
      var valueBefore = field.GetValue(obj1);
      var valueAfter = field.GetValue(obj2);
      if (!Equals(valueBefore, valueAfter)) {
        differences.Add(new Difference(field.Name, valueBefore, valueAfter));
      }
    }
    return differences;
  }

  public static List<Difference> ListProperties<T>(T obj1, T obj2, params string[] excludePropertyNames) {
    var differences = new List<Difference>();
    var properties = typeof(T).GetProperties();
    foreach (var property in properties) {
      if (excludePropertyNames.Contains(property.Name)) {
        continue;
      }
      var valueBefore = property.GetValue(obj1);
      var valueAfter = property.GetValue(obj2);
      if (!Equals(valueBefore, valueAfter)) {
        differences.Add(new Difference(property.Name, valueBefore, valueAfter));
      }
    }
    return differences;
  }

}

public static class DifferenceExtensions {
  public static List<Difference> GetFieldDifferenceList<T>(this T obj1, T obj2, params string[] excludePropertyNames) => Difference.ListFields(obj1, obj2, excludePropertyNames);
  public static List<Difference> GetPropertyDifferenceList<T>(this T obj1, T obj2, params string[] excludePropertyNames) => Difference.ListProperties(obj1, obj2, excludePropertyNames);
}

//public interface IAuditBeforeAfter<T, TAudit> : IAuditBefore<T, TAudit>, IAuditAfter<T, TAudit> { }
//public interface IAuditAfter<T, TAudit> {
//  Expression<Func<T, TAudit>> AuditAfterSetter { get; }
//}

//public interface IAuditBefore<T, TAudit> {
//  Expression<Func<T, TAudit>> AuditBeforeSetter { get; }
//}
//public interface IAuditUserProfile {
//  IAuditUserProfile UserProfile { get; }
//}

//public interface IAuditWorkstation {
//  string Workstation { get; set; }
//}

