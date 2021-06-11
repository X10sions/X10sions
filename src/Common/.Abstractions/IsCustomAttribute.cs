using System;

namespace Common {
  [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
  public class IsCustomAttribute : Attribute {
    public IsCustomAttribute() {
    }

    public IsCustomAttribute(IsCustomReason reason) {
      Reason = reason;
    }

    //    public string Create{ get; set; }
    //    public bool IsSealed { get; set; }
    //    public bool IsPrivate { get; set; }
    public IsCustomReason Reason { get; set; }

  }
}
