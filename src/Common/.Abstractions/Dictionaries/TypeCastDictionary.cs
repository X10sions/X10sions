using Common.Constants;
using System;
using System.Collections.Generic;

namespace Common.Dictionaries {
  public class TypeCastDictionary : Dictionary<Type, Type[]> {

    public static TypeCastDictionary Instance => new TypeCastDictionary();

    public TypeCastDictionary() {
      this[typeof(decimal)] = TypeConstants.Decimal;
      this[typeof(double)] = TypeConstants.Double;
      this[typeof(float)] = TypeConstants.Float;
      this[typeof(int)] = TypeConstants.Int;
      this[typeof(long)] = TypeConstants.Long;
      this[typeof(short)] = TypeConstants.Short;
      this[typeof(uint)] = TypeConstants.UInt;
      this[typeof(ulong)] = TypeConstants.ULong;
      this[typeof(ushort)] = TypeConstants.UShort;
    }

  }
}