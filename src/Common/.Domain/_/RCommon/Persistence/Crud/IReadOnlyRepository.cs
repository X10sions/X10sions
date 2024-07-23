using Common.Domain.Entities;
using System.Linq.Expressions;

namespace RCommon.Persistence.Crud;

public interface IReadOnlyRepository<T> : INamedDataSource {
  Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken token = default);
  Task<long> CountAsync(Expression<Func<T, bool>> expression, CancellationToken token = default);
  Task<T?> GetAsync(Expression<Func<T, bool>> expression, CancellationToken token = default);
  Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken token = default);
  Task<T?> GetByPrimaryKeyAsync<TKey>(TKey primaryKey, CancellationToken token = default);
}

public interface IReadOnlyRepository<T, TKey> : IReadOnlyRepository<T>
  where T: IEntityWithId<TKey>
  where TKey : IEquatable<TKey> {
    Task<T?> GetByPrimaryKeyAsync(TKey primaryKey, CancellationToken token = default);
}

public static class IReadOnlyRepositoryExtensions {
  public static async Task<bool> AnyAsync<T>(this IReadOnlyRepository<T> repository, ISpecification<T> specification, CancellationToken token = default) => await repository.AnyAsync(specification.Predicate, token);
  public static async Task<long> CountAsync<T>(this IReadOnlyRepository<T> repository, ISpecification<T> specification, CancellationToken token = default) => await repository.CountAsync(specification.Predicate, token);
  public static async Task<T> GetAsync<T>(this IReadOnlyRepository<T> repository, ISpecification<T> specification, CancellationToken token = default) => await repository.GetAsync(specification.Predicate, token);
  public static async Task<ICollection<T>> GetAllAsync<T>(this IReadOnlyRepository<T> repository, ISpecification<T> specification, CancellationToken token = default) => await repository.GetAllAsync(specification.Predicate, token);
}