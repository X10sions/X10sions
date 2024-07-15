namespace Common.ValueObjects;
public readonly record struct IdNameCode<TId>(TId Id, string Name, string Code) : IIdNameCode<TId> {  }

public interface IIdNameCode<TId> : IIdName<TId> {
  string Code { get;  }
}

public interface IIdNameCode : IIdNameCode<int> { }

