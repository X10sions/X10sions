namespace Common.ValueObjects;

public readonly record struct IdCode<TId>(TId Id,  string Code) : IIdCode<TId> { }

public interface IIdCode<TId> : IId<TId> {
  string Code { get; }
}

public interface IIdCode : IIdCode<int> { }
