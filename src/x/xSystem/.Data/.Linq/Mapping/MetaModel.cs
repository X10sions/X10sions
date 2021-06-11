using System.Collections.Generic;
using System.Reflection;

namespace System.Data.Linq.Mapping {
  public abstract class MetaModel {
    public abstract MappingSource MappingSource { get; }
    public abstract Type ContextType { get; }
    public abstract string DatabaseName { get; }
    public abstract Type ProviderType { get; }
    internal object Identity { get; } = new object();
    public abstract MetaTable GetTable(Type rowType);
    public abstract MetaFunction GetFunction(MethodInfo method);
    public abstract IEnumerable<MetaTable> GetTables();
    public abstract IEnumerable<MetaFunction> GetFunctions();
    public abstract MetaType GetMetaType(Type type);
  }
}