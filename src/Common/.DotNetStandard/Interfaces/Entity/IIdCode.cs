namespace Common.Interfaces.Entity {
  public interface IIdCode<TId> : IId<TId> {
    string Code { get; set; }
  }

  public interface IIdCode : IIdCode<int> {      }

}
