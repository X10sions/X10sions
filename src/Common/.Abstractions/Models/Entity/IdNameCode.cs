using Common.Interfaces.Entity;

namespace Common.Models.Entity {
  public class IdNameCode<TId> : IdName<TId>, IIdNameCode<TId> {
    public string Code { get; set; }
  }
}
