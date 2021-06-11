using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace System.Data.Linq.Mapping {
  internal class AttributedMetaModel : MetaModel {
    private ReaderWriterLock @lock = new ReaderWriterLock();
    private MappingSource mappingSource;
    private Type contextType;
    private Type providerType;
    private Dictionary<Type, MetaType> metaTypes;
    private Dictionary<Type, MetaTable> metaTables;
    private ReadOnlyCollection<MetaTable> staticTables;
    private Dictionary<MetaPosition, MetaFunction> metaFunctions;
    private string dbName;
    private bool initStaticTables;
    private bool initFunctions;
    public override MappingSource MappingSource => mappingSource;

    public override Type ContextType => contextType;

    public override string DatabaseName => dbName;

    public override Type ProviderType => providerType;

    internal AttributedMetaModel(MappingSource mappingSource, Type contextType) {
      this.mappingSource = mappingSource;
      this.contextType = contextType;
      metaTypes = new Dictionary<Type, MetaType>();
      metaTables = new Dictionary<Type, MetaTable>();
      metaFunctions = new Dictionary<MetaPosition, MetaFunction>();
      ProviderAttribute[] array = (ProviderAttribute[])this.contextType.GetCustomAttributes(typeof(ProviderAttribute), true);
      if (array != null && array.Length == 1) {
        providerType = array[0].Type;
      } else {
        providerType = typeof(SqlProvider);
      }
      DatabaseAttribute[] array2 = (DatabaseAttribute[])this.contextType.GetCustomAttributes(typeof(DatabaseAttribute), false);
      dbName = ((array2 != null && array2.Length != 0) ? array2[0].Name : this.contextType.Name);
    }

    public override IEnumerable<MetaTable> GetTables() {
      InitStaticTables();
      if (staticTables.Count <= 0) {
        @lock.AcquireReaderLock(-1);
        try {
          return (from x in metaTables.Values
                  where x != null
                  select x).Distinct();
        } finally {
          @lock.ReleaseReaderLock();
        }
      }
      return staticTables;
    }

    private void InitStaticTables() {
      if (!initStaticTables) {
        @lock.AcquireWriterLock(-1);
        try {
          if (!initStaticTables) {
            HashSet<MetaTable> hashSet = new HashSet<MetaTable>();
            Type baseType = contextType;
            while (baseType != typeof(DataContext)) {
              FieldInfo[] fields = baseType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
              FieldInfo[] array = fields;
              foreach (FieldInfo fieldInfo in array) {
                Type fieldType = fieldInfo.FieldType;
                if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Table<>)) {
                  Type rowType = fieldType.GetGenericArguments()[0];
                  hashSet.Add(GetTableNoLocks(rowType));
                }
              }
              PropertyInfo[] properties = baseType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
              PropertyInfo[] array2 = properties;
              foreach (PropertyInfo propertyInfo in array2) {
                Type propertyType = propertyInfo.PropertyType;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Table<>)) {
                  Type rowType2 = propertyType.GetGenericArguments()[0];
                  hashSet.Add(GetTableNoLocks(rowType2));
                }
              }
              baseType = baseType.BaseType;
            }
            staticTables = new List<MetaTable>(hashSet).AsReadOnly();
            initStaticTables = true;
          }
        } finally {
          @lock.ReleaseWriterLock();
        }
      }
    }

    private void InitFunctions() {
      if (!initFunctions) {
        @lock.AcquireWriterLock(-1);
        try {
          if (!initFunctions) {
            if (contextType != typeof(DataContext)) {
              Type baseType = contextType;
              while (baseType != typeof(DataContext)) {
                MethodInfo[] methods = baseType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (MethodInfo methodInfo in methods) {
                  if (IsUserFunction(methodInfo)) {
                    if (methodInfo.IsGenericMethodDefinition) {
                      throw System.Data.Linq.Mapping.Error.InvalidUseOfGenericMethodAsMappedFunction(methodInfo.Name);
                    }
                    MetaPosition key = new MetaPosition(methodInfo);
                    if (!metaFunctions.ContainsKey(key)) {
                      MetaFunction metaFunction = new AttributedMetaFunction(this, methodInfo);
                      metaFunctions.Add(key, metaFunction);
                      foreach (MetaType resultRowType in metaFunction.ResultRowTypes) {
                        foreach (MetaType inheritanceType in resultRowType.InheritanceTypes) {
                          if (!metaTypes.ContainsKey(inheritanceType.Type)) {
                            metaTypes.Add(inheritanceType.Type, inheritanceType);
                          }
                        }
                      }
                    }
                  }
                }
                baseType = baseType.BaseType;
              }
            }
            initFunctions = true;
          }
        } finally {
          @lock.ReleaseWriterLock();
        }
      }
    }

    private static bool IsUserFunction(MethodInfo mi) {
      return Attribute.GetCustomAttribute(mi, typeof(FunctionAttribute), false) != null;
    }

    public override MetaTable GetTable(Type rowType) {
      if (!(rowType == (Type)null)) {
        @lock.AcquireReaderLock(-1);
        MetaTable value;
        try {
          if (metaTables.TryGetValue(rowType, out value)) {
            return value;
          }
        } finally {
          @lock.ReleaseReaderLock();
        }
        @lock.AcquireWriterLock(-1);
        try {
          value = GetTableNoLocks(rowType);
          return value;
        } finally {
          @lock.ReleaseWriterLock();
        }
      }
      throw System.Data.Linq.Mapping.Error.ArgumentNull("rowType");
    }

    internal MetaTable GetTableNoLocks(Type rowType) {
      if (!metaTables.TryGetValue(rowType, out MetaTable value)) {
        Type type = GetRoot(rowType) ?? rowType;
        TableAttribute[] array = (TableAttribute[])type.GetCustomAttributes(typeof(TableAttribute), true);
        if (array.Length == 0) {
          metaTables.Add(rowType, null);
        } else {
          if (!metaTables.TryGetValue(type, out value)) {
            value = new AttributedMetaTable(this, array[0], type);
            foreach (MetaType inheritanceType in value.RowType.InheritanceTypes) {
              metaTables.Add(inheritanceType.Type, value);
            }
          }
          if (value.RowType.GetInheritanceType(rowType) == null) {
            metaTables.Add(rowType, null);
            return null;
          }
        }
      }
      return value;
    }

    private static Type GetRoot(Type derivedType) {
      while (derivedType != (Type)null && derivedType != typeof(object)) {
        TableAttribute[] array = (TableAttribute[])derivedType.GetCustomAttributes(typeof(TableAttribute), false);
        if (array.Length != 0) {
          return derivedType;
        }
        derivedType = derivedType.BaseType;
      }
      return null;
    }

    public override MetaType GetMetaType(Type type) {
      if (type == (Type)null) {
        throw System.Data.Linq.Mapping.Error.ArgumentNull("type");
      }
      MetaType value = null;
      @lock.AcquireReaderLock(-1);
      try {
        if (metaTypes.TryGetValue(type, out value)) {
          return value;
        }
      } finally {
        @lock.ReleaseReaderLock();
      }
      MetaTable table = GetTable(type);
      if (table == null) {
        InitFunctions();
        @lock.AcquireWriterLock(-1);
        try {
          if (metaTypes.TryGetValue(type, out value)) {
            return value;
          }
          value = new UnmappedType(this, type);
          metaTypes.Add(type, value);
          return value;
        } finally {
          @lock.ReleaseWriterLock();
        }
      }
      return table.RowType.GetInheritanceType(type);
    }

    public override MetaFunction GetFunction(MethodInfo method) {
      if (method == (MethodInfo)null) {
        throw System.Data.Linq.Mapping.Error.ArgumentNull("method");
      }
      InitFunctions();
      MetaFunction value = null;
      metaFunctions.TryGetValue(new MetaPosition(method), out value);
      return value;
    }

    public override IEnumerable<MetaFunction> GetFunctions() {
      InitFunctions();
      return metaFunctions.Values.ToList().AsReadOnly();
    }
  }


}