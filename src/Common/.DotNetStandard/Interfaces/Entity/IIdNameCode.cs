namespace Common.Interfaces.Entity {
  public interface IIdNameCode<TId> : IIdName<TId> {
    string Code { get; set; }
  }

  public interface IIdNameCode : IIdNameCode<int> { }
}
