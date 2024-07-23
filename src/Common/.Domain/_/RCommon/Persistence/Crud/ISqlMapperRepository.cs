using RCommon.Entities;

namespace RCommon.Persistence.Crud;

public interface ISqlMapperRepository<TEntity> : IReadOnlyRepository<TEntity>, IWriteOnlyRepository<TEntity> where TEntity : IBusinessEntity {
  public string TableName { get; set; }
}
