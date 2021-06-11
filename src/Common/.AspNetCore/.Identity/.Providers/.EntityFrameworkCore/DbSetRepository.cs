using Common.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {
  public class DbSetRepository<T> : IDbSet<T>, IDataStoreRepositoryWrite<T> where T : class {
    private readonly DbSet<T> _set;
    public DbSetRepository(DbSet<T> set) {
      _set = set;
    }

    public Type ElementType => ((IQueryable<T>)_set).ElementType;
    public Expression Expression => ((IQueryable<T>)_set).Expression;
    public IQueryProvider Provider => ((IQueryable<T>)_set).Provider;

    public IServiceProvider Instance => throw new NotImplementedException();

    public bool ContainsListCollection => throw new NotImplementedException();

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_set).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)_set).GetEnumerator();

    void IDataStoreRepositoryWrite<T>.Add(T entity) => _set.Add(entity);
    void IDataStoreRepositoryWrite<T>.Update(T entity) => _set.Update(entity);
    void IDataStoreRepositoryWrite<T>.Delete(T entity) => _set.Remove(entity);

    async Task IDataStoreRepositoryWrite<T>.AddAsync(T entity, CancellationToken token) => await _set.AddAsync(entity, token);
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public IList GetList() => throw new NotImplementedException();
  }

  public interface IDataStoreRepositoryWrite<T> {
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task AddAsync(T entity, CancellationToken token);
  }

}
