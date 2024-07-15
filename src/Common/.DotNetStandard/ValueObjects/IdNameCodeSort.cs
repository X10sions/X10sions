namespace Common.ValueObjects;
public readonly record struct IdNameCodeSort<TId>(TId Id, string Name , string Code, int Sort) : IIdNameCodeSort<TId> {  }

public interface IIdNameCodeSort<TId> : IIdNameCode<TId> {
  int Sort { get;  }
}

public interface IIdNameCodeSort : IIdNameCodeSort<int> { }

