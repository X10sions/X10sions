using System;

namespace Common.Enums {
  public enum AuditType {
    Delete,
    Insert,
    Update
  }

  public static class AuditTypeExtensions {
    public static string Name(this AuditType auditType) {
      switch (auditType) {
        case AuditType.Insert: return nameof(AuditType.Insert);
        case AuditType.Delete: return nameof(AuditType.Delete);
        case AuditType.Update: return nameof(AuditType.Update);
        default:
          throw new ArgumentOutOfRangeException(nameof(auditType));
      }
    }

    public static string Code(this AuditType auditType) => auditType.Name().Substring(0, 1);
  }
}
