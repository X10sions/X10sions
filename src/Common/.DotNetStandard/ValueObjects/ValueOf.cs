using System.Linq.Expressions;
using System.Reflection;

namespace Common.ValueObjects;

/// <summary>
/// https://github.com/mcintyre321/ValueOf/blob/master/ValueOf/ValueOf.cs
/// </summary>
public class ValueOf<TValue, TThis> where TThis : ValueOf<TValue, TThis>, new() {
  static ValueOf() {
    var ctor = typeof(TThis).GetTypeInfo().DeclaredConstructors.First();
    var argsExp = new Expression[0];
    var newExp = Expression.New(ctor, argsExp);
    var lambda = Expression.Lambda(typeof(Func<TThis>), newExp);
    Factory = (Func<TThis>)lambda.Compile();
  }

  protected virtual void Validate() { }
  protected virtual bool TryValidate() => true;

  public TValue Value { get; protected set; }

  protected virtual bool Equals(ValueOf<TValue, TThis> other) => EqualityComparer<TValue>.Default.Equals(Value, other.Value);

  public override bool Equals(object obj) => obj is not null && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((ValueOf<TValue, TThis>)obj));

  public override int GetHashCode() => EqualityComparer<TValue>.Default.GetHashCode(Value);

  public override string ToString() => Value.ToString();

  public static TThis From(TValue item) {
    var x = Factory();
    x.Value = item;
    x.Validate();
    return x;
  }

  public static bool TryFrom(TValue item, out TThis thisValue) {
    var x = Factory();
    x.Value = item;
    thisValue = x.TryValidate() ? x : null;
    return thisValue != null;
  }

  private static readonly Func<TThis> Factory;
  public static bool operator ==(ValueOf<TValue, TThis> a, ValueOf<TValue, TThis> b) => a is null && b is null ? true : a is null || b is null ? false : a.Equals(b);
  public static bool operator !=(ValueOf<TValue, TThis> a, ValueOf<TValue, TThis> b) => !(a == b);

}