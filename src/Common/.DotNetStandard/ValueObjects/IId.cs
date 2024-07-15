namespace Common.ValueObjects;
public interface IId<TId> {
  TId Id { get;  }
}

public interface IId : IId<int> { }
