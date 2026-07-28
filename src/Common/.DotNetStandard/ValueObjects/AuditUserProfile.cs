namespace Common.ValueObjects;

public readonly record struct AuditUserProfile(string Value) : IValueObject<string> {
  //    public static implicit operator AuditUserProfile(string value) => new(value);
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

