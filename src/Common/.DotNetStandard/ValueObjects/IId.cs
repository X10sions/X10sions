namespace Common.ValueObjects;
public interface IId<out TId> {
  TId Id { get;  }
}

public interface IId : IId<int> { }
