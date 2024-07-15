namespace Common.ValueObjects;

public readonly record struct IdNameSort<TId>(TId Id, string Name,  int Sort) : IIdNameSort<TId> { }

public interface IIdNameSort<TId> : IIdName<TId> {
  int Sort { get; }
}

public interface IIdNameSort : IIdNameSort<int> { }
