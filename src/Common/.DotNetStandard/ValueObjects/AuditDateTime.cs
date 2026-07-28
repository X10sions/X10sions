using Common.Models;
namespace Common.ValueObjects;


public readonly record struct AuditDateTime(DateTime Value) : IValueObject<DateTime> {
  public static AuditDateTime Now => new(DateTime.Now);
  //    public static implicit operator AuditDateTime(DateTime value) => new(value);
  //    public static implicit operator DateTime(AuditDateTime value) => value;
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

