namespace Common.Interfaces.Entity {
  public interface IIdNameSort<TId> : IIdName<TId> {
    int Sort { get; set; }
  }

  public interface IIdNameSort : IIdNameSort<int> {    }
}
