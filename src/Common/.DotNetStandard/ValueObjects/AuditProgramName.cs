using Common.Models;
namespace Common.ValueObjects;

public readonly record struct AuditProgramName(string Value) : IValueObject<string> {
  public static AuditProgramName Empty { get; } = new(string.Empty);
  public static AuditProgramName MTGWEB { get; } = new("MTGWEB");
  //    public static implicit operator AuditProgramName(string value) => new(value);
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

