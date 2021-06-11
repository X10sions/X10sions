namespace NHibernate_v5_2.Linq {
  /// <summary>
  /// Interface to access the entity name of a NhQueryable instance.
  /// </summary>
  interface IEntityNameProvider {
    string EntityName { get; }
  }
}