namespace Common.Interfaces.Entity {
  public interface IIdNameCodeSort<TId> : IIdNameCode<TId> {
    int Sort { get; set; }
  }

  public interface IIdNameCodeSort : IIdNameCodeSort<int> {     }
}
