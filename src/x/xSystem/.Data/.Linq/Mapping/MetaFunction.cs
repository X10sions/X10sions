﻿using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Data.Linq.Mapping {
  public abstract class MetaFunction {
    public abstract MetaModel Model { get; }
    public abstract MethodInfo Method { get; }
    public abstract string Name { get; }
    public abstract string MappedName { get; }
    public abstract bool IsComposable { get; }
    public abstract ReadOnlyCollection<MetaParameter> Parameters { get; }
    public abstract MetaParameter ReturnParameter { get; }
    public abstract bool HasMultipleResults { get; }
    public abstract ReadOnlyCollection<MetaType> ResultRowTypes { get; }
  }
}