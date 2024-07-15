namespace Common.ValueObjects;
  public readonly record struct IdName<TId>(TId Id , string Name) : IIdName<TId> {
  }

public interface IIdName<TId> : IId<TId> {
  string Name { get;  }
}

public interface IIdName : IIdName<int> { }