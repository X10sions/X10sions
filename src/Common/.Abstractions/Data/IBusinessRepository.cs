﻿using Common.Collections.Paged;
using Common.Data.Entities;
using System.Linq.Expressions;

namespace Common.Data;

public interface IBusinessRepository<T, TKey>
  where T : class, IEntityWithId<TKey>
  //where TKey : IEquatable<TKey> 
  {
  void Add(T entity);
  int Count();
  int Count(Expression<Func<T, bool>> predicate);
  void Delete(T entity);
  T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertiesToLoad);
  List<T> GetAll(params Expression<Func<T, object>>[] navigationPropertiesToLoad);
  IPagedList<T> GetAllPaged(string orderBy, int startRowIndex = 0, int maxRows = 10, params Expression<Func<T, object>>[] navigationPropertiesToLoad);
  List<T> GetAllFiltered(Expression<Func<T, bool>> prediacte, params Expression<Func<T, object>>[] navigationPropertiesToLoad);
  IPagedList<T> GetAllFilteredPaged(Expression<Func<T, bool>> predicate, string orderBy, int startRowIndex = 0, int maxRows = 10, params Expression<Func<T, object>>[] navigationPropertiesToLooad);
  T GetById(TKey Id);
  void Update(T entity);
}
