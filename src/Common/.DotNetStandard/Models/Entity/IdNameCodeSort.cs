using Common.Interfaces.Entity;

namespace Common.Models.Entity {
  public class IdNameCodeSort<TId> : IdNameCode<TId>, IIdNameCodeSort<TId> {
    public int Sort { get; set; }
  }
}
