using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CommonOrm {
  public interface IOrmDiscriminator<T, TValue> {
    Expression<Func<T, TValue>> Column { get; set; }
    Dictionary<TValue, Type> Values { get; }
    Type? DefaultType { get; set; }
  }

  public static class IOrmDiscriminatorExtensions {

    public static IOrmDiscriminator<T, TValue> SetDefaultType<T, TValue>(this IOrmDiscriminator<T, TValue> ormDiscriminator, Type defaultType) {
      ormDiscriminator.DefaultType = defaultType;
      return ormDiscriminator;
    }

    public static IOrmDiscriminator<T, TValue> AddValue<T, TValue, TOtherEntity>(this IOrmDiscriminator<T, TValue> ormDiscriminator, TValue value) {
      ormDiscriminator.Values[value] = typeof(TOtherEntity);
      return ormDiscriminator;
    }

  }

  public class OrmDiscriminator<T, TValue> : IOrmDiscriminator<T, TValue> {
    public OrmDiscriminator(Expression<Func<T, TValue>> column, Type? defaultType = null, Dictionary<TValue, Type>? values = null) {
      Column = column;
      if (values != null) {
        Values = values;
      }
      DefaultType = defaultType;
    }

    public Expression<Func<T, TValue>> Column { get; set; }
    public Dictionary<TValue, Type> Values { get; } = new Dictionary<TValue, Type>();
    public Type? DefaultType { get; set; }


    //public List<OrmDiscriminatorValue<TValue>> ValueList  => new List<OrmDiscriminatorValue<TValue>>();

  }

  //public class OrmDiscriminatorValue<TValue> {
  //  public Type Type{ get; set; }
  //  public TValue Value { get; set; }
  //  public bool IsDefault { get; set; }
  //}

}