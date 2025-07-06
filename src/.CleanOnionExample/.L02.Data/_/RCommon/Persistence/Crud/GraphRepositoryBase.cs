using Microsoft.Extensions.Options;
using RCommon.Entities;

namespace RCommon.Persistence.Crud;

public abstract class GraphRepositoryBase<T, TDataStore, TTable, TLoadTable> : LinqRepositoryBase<T, TDataStore, TTable, TLoadTable>, IGraphRepository<T>
  where T : class, IBusinessEntity
  where TDataStore: IDataStore
  where TTable : IQueryable<T>
  where TLoadTable : IQueryable<T> {

  public GraphRepositoryBase(IDataStoreFactory dataStoreFactory, IEntityEventTracker eventTracker, IOptions<DefaultDataStoreOptions> defaultDataStoreOptions) : base(dataStoreFactory, eventTracker, defaultDataStoreOptions) { }

  public bool Tracking { get; set; }
}
