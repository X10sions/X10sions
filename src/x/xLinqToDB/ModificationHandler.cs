using LinqToDB.Linq;
using System.Linq.Expressions;

namespace LinqToDB;
public class ModificationHandler<T> : IModificationHandler<T> where T : class {
  public Type EntityType => typeof(T);
  public IQueryable<T> Queryable { get; set; }
  public IValueInsertable<T> Insertable { get; set; }
  public IUpdatable<T> Updatable { get; set; }

  public ModificationHandler(IDataContext dataContext, Expression<Func<T, bool>> predicate) : this(dataContext.GetTable<T>(), predicate) { }

  public ModificationHandler(ITable<T> table, Expression<Func<T, bool>> predicate) {
    //Table = table;
    Queryable = table.Where(predicate);
    //Deletable = table.Where(wherePredicate);
    Updatable = table.Where(predicate).AsUpdatable();
    Insertable = table.AsValueInsertable();//  table.DataContext.Into(table);
  }

}