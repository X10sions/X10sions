using RCommon.Entities;
namespace RCommon.Persistence.Crud;

public interface IGraphRepository<T> : ILinqRepository<T>, IEagerLoadableQueryable<T> where T : class, IBusinessEntity {
  public bool Tracking { get; set; }
}
