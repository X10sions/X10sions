using System.Collections.Generic;

namespace System {
  [Obsolete("Use default(type) instead.")]
  public class DefaultValueDictionary : Dictionary<Type, object> {

    public static DefaultValueDictionary Instance => new DefaultValueDictionary();

    public DefaultValueDictionary() {
      this[typeof(decimal)] = default(decimal);
      this[typeof(int)] = default(int);
      this[typeof(Guid)] = default(Guid);
      this[typeof(DateTime)] = default(DateTime);
      this[typeof(DateTimeOffset)] = default(DateTimeOffset);
      this[typeof(long)] = default(long);
      this[typeof(bool)] = default(bool);
      this[typeof(double)] = default(double);
      this[typeof(short)] = default(short);
      this[typeof(float)] = default(float);
      this[typeof(byte)] = default(byte);
      this[typeof(char)] = default(char);
      this[typeof(uint)] = default(uint);
      this[typeof(ushort)] = default(ushort);
      this[typeof(ulong)] = default(ulong);
      this[typeof(sbyte)] = default(sbyte);
    }

  }
}