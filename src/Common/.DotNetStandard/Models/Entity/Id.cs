using Common.Interfaces.Entity;

namespace Common.Models.Entity {
  public class _Id<TId> : IId<TId> {
    public TId Id { get; set; }
  }
}
