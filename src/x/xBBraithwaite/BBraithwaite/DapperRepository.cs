using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace BBaithwaite {
  public abstract class DapperRepository<T, TId> : IRepository<T, TId> where T : EntityBase, IAggregateRoot<TId> {
    private readonly string TableName;
    public IDbConnection Connection { get; }

    public DapperRepository(string tableName, IDbConnection connection) {
      TableName = tableName;
      Connection = connection;
    }

    internal virtual object Mapping(T item) {
      return item;
    }

    public virtual TId Add(T item) {
      using (var cn = Connection) {
        var parameters = Mapping(item);
        cn.Open();
        item.Id = cn.DynamicQueryInsert<TId>(TableName, parameters);
      }
      return item.Id;
    }

    public virtual void Update(T item) {
      using (IDbConnection cn = Connection) {
        var parameters = Mapping(item);
        cn.Open();
        cn.DynamicQueryUpdate(TableName, parameters);
      }
    }

    public virtual void Remove(T item) {
      using (IDbConnection cn = Connection) {
        cn.Open();
        cn.Execute("DELETE FROM " + TableName + " WHERE ID=@ID", new {
          ID = item.Id
        });
      }
    }

    public virtual T FindById(TId id) {
      T item = null;
      using (IDbConnection cn = Connection) {
        cn.Open();
        item = cn.Query<T>("SELECT * FROM " + TableName + " WHERE ID=@ID", new {
          ID = id
        }).SingleOrDefault();
      }
      return item;
    }

    public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate) {
      IEnumerable<T> items = null;
      var result = DynamicQuery.GetDynamicQuery(TableName, predicate);
      using (var cn = Connection) {
        cn.Open();
        items = cn.Query<T>(result.Sql, result.Param);
      }
      return items;
    }

    public virtual IEnumerable<T> FindAll() {
      IEnumerable<T> items = null;
      using (var cn = Connection) {
        cn.Open();
        items = cn.Query<T>("SELECT * FROM " + TableName);
      }
      return items;
    }
  }
}
