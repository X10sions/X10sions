using LinqToDB.Linq;
using LinqToDB.Mapping;
using System.Diagnostics;
using System.Linq.Expressions;

namespace LinqToDB;

public class ModificationHandler<T> : IModificationHandler<T> where T : class {
  public Type EntityType { get; } = typeof(T);
  //public IDictionary<LambdaExpression, object> ExpressionValues { get; } = new Dictionary<LambdaExpression, object>();
  public Expression<Func<T, bool>> Predicate { get; set; }
  public ITable<T> Table { get; private set; }
  public IQueryable<T> TableWherePredicate => Table.Where(Predicate);
  public IValueInsertable<T> ValueInsertable { get; set; }
  public IUpdatable<T> Updatable { get; set; }
  public IDataContext DataContext { get; }
  public ITable<T> TableHint([SqlQueryDependent] string hint) {
    Table = Table.TableHint(hint);
    // Explicitly rebuild downstream dependencies with the new table tree
    ValueInsertable = Table.AsValueInsertable();
    Updatable = Table.Where(Predicate).AsUpdatable();
    return Table;
  }

  public ModificationHandler(IDataContext dataContext, Expression<Func<T, bool>> predicate) : this(dataContext.GetTable<T>(), predicate) { }
  public ModificationHandler(IDataContext dataContext, Expression<Func<T, bool>> predicate, [SqlQueryDependent] string hint) : this(dataContext.GetTable<T>(), predicate, hint) { }

  public ModificationHandler(ITable<T> table, Expression<Func<T, bool>> predicate) {
    DataContext = table.DataContext;
    Table = table;
    Predicate = predicate;
    ValueInsertable = table.AsValueInsertable();
    Updatable = table.Where(predicate).AsUpdatable();
  }

  public ModificationHandler(ITable<T> table, Expression<Func<T, bool>> predicate, [SqlQueryDependent] string hint) {
    Table = table.TableHint(hint);
    DataContext = table.DataContext;
    Predicate = predicate;
    ValueInsertable = table.AsValueInsertable();
    Updatable = table.Where(predicate).AsUpdatable();
  }

}
