using System.Reflection;

namespace System.Data.Linq.Mapping {
  public abstract class MetaParameter {
    public abstract ParameterInfo Parameter { get; }
    public abstract string Name { get; }
    public abstract string MappedName { get; }
    public abstract Type ParameterType { get; }
    public abstract string DbType { get; }
  }
}