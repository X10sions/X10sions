namespace Common.Domain.ValueObjects;
/// <summary>
/// Learn more: https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/implement-value-objects
/// </summary>
public abstract class BaseValueObject : IEquatable<BaseValueObject> {
  protected static bool EqualOperator(BaseValueObject left, BaseValueObject right) => !(left is null ^ right is null) && left?.Equals(right!) != false;
  protected static bool NotEqualOperator(BaseValueObject left, BaseValueObject right) => !EqualOperator(left, right);
  protected abstract IEnumerable<object> GetEqualityComponents();
  public override bool Equals(object? obj) => obj is not null && obj.GetType() == GetType() && GetEqualityComponents().SequenceEqual(((BaseValueObject)obj).GetEqualityComponents());
  public override int GetHashCode() => GetEqualityComponents().Select(x => x is not null ? x.GetHashCode() : 0).Aggregate((x, y) => x ^ y);
  abstract public override string ToString();
  // IEquatable
  public bool Equals(BaseValueObject? other) => Equals(other as object);

  public static bool operator ==(BaseValueObject one, BaseValueObject two) => EqualOperator(one, two);
  public static bool operator !=(BaseValueObject one, BaseValueObject two) => NotEqualOperator(one, two);

}
