using System;

namespace IQToolkit.Data.Mapping {
  /// <summary>
  /// Describes the mapping between at database table and an entity type.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public class TableAttribute : TableBaseAttribute {
  }
}
