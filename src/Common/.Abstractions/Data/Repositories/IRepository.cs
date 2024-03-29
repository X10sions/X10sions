﻿using Common.Data.Entities;

namespace Common.Data.Repositories;

public interface IRepository<T, TId> : ICommandRepository<T, TId>, IQueryRepository<T, TId>
  where T : class, IEntityWithId<TId>
  //where TId : IEquatable<TId> 
  { }

public interface IRepositoryAsync<T> where T : class {
  IQueryable<T> Entities { get; }
  Task<T> GetByIdAsync(int id);
  Task<List<T>> GetAllAsync();
  Task<List<T>> GetPagedReponseAsync(int pageNumber, int pageSize);
  Task<T> AddAsync(T entity);
  Task UpdateAsync(T entity);
  Task DeleteAsync(T entity);
}
