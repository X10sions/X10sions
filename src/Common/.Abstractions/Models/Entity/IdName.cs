using Common.Interfaces.Entity;

namespace Common.Models.Entity {
  public class IdName<TId> : _Id<TId>, IIdName<TId> {
    public string Name { get; set; }
  }
}
