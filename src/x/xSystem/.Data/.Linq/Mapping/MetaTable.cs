using System.Reflection;

namespace System.Data.Linq.Mapping {
  public abstract class MetaTable {
    public abstract MetaModel Model { get; }
    public abstract string TableName { get; }
    public abstract MetaType RowType { get; }
    public abstract MethodInfo InsertMethod { get; }
    public abstract MethodInfo UpdateMethod { get; }
    public abstract MethodInfo DeleteMethod { get; }
  }

}