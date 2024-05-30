namespace Common.Interfaces.Entity {
  public interface IId<TId> {
    TId Id { get; set; }
  }

  public interface IId : IId<int> { }

}

