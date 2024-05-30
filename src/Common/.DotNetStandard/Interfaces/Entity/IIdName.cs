namespace Common.Interfaces.Entity {
  public interface IIdName<TId> : IId<TId> {
    string Name { get; set; }
  }

  public interface IIdName : IIdName<int> {     }
}

