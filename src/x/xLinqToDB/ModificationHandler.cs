using LinqToDB.Linq;
using System.Linq.Expressions;

namespace LinqToDB;
public class ModificationHandler<T> : IModificationHandler<T> where T : class {
  public Type EntityType => typeof(T);
  //public ITable<T> Table { get; }
  public IQueryable<T> Queryable { get; set; }
  //public IQueryable<T> Deletable { get; }
  public IValueInsertable<T> Insertable { get; set; }
  public IUpdatable<T> Updatable { get; set; }
  //public Expression<Func<T, bool>> Predicate { get; } = x => true;

  public ModificationHandler(IDataContext dataContext, Expression<Func<T, bool>> wherePredicate) : this(dataContext.GetTable<T>(), wherePredicate) { }

  public ModificationHandler(ITable<T> table, Expression<Func<T, bool>> wherePredicate) {
    //Table = table;
    Queryable = table.Where(wherePredicate);
    //Deletable = table.Where(wherePredicate);
    Updatable = table.Where(wherePredicate).AsUpdatable();
    Insertable = table.AsValueInsertable();//  table.DataContext.Into(table);
  }

}