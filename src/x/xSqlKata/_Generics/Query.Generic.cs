using System;
using System.Linq.Expressions;

namespace SqlKata {
  // https://github.com/sqlkata/querybuilder/pull/183/commits/a32ef55bc6b9f99470b62a3de96a5ab5eaa266b8

  // https://github.com/sqlkata/querybuilder/pull/183/commits/4965e7d6edf05c721d914f5287eaa0c873b71524
  public partial class Query<T> : Query where T : class {
    public Query() : base(typeof(T).Name) {
    }

    public new Query<T> From(string table) {
      base.From(table);
      return this;
    }

    public Query<T> GroupBy(Expression<Func<T, object>> expression) {
      GroupBy(expression.GetMemberName());
      return this;
    }

    public Query<T> Having(Expression<Func<T, object>> expression) {
      var exp = expression.GetMemberName().Split();
      Having(exp[0], exp[1], exp[2]);
      return this;
    }

    public Query<T> HavingNot(Expression<Func<T, object>> expression) {
      var exp = expression.GetMemberName().Split();
      HavingNot(exp[0], exp[1], exp[2]);
      return this;
    }

    public Query<T> OrHaving(Expression<Func<T, object>> expression) {
      var exp = expression.GetMemberName().Split();
      OrHaving(exp[0], exp[1], exp[2]);
      return this;
    }

    public Query<T> OrHavingNot(Expression<Func<T, object>> expression) {
      var exp = expression.GetMemberName().Split();
      OrHavingNot(exp[0], exp[1], exp[2]);
      return this;
    }

    public Query<T> Join<JTable>(Expression<Func<T, JTable, object>> expression) where JTable : class {
      Join(null, expression);
      return this;
    }

    public Query<T> Join<JTable>(string tableName, Expression<Func<T, JTable, object>> expression) where JTable : class {
      var joinColumns = expression.GetMemberNames();
      if (tableName != null && tableName.Length > 0) {
        Join(tableName, joinColumns[0], joinColumns[1]);
      } else {
        Join(typeof(JTable).Name, joinColumns[0], joinColumns[1]);
      }
      return this;
    }

    public Query<T> LeftJoin<JTable>(Expression<Func<T, JTable, object>> expression) where JTable : class {
      LeftJoin(null, expression);
      return this;
    }

    public Query<T> LeftJoin<JTable>(string tableName, Expression<Func<T, JTable, object>> expression) where JTable : class {
      var joinColumns = expression.GetMemberNames();
      if (tableName != null && tableName.Length > 0) {
        LeftJoin(tableName, joinColumns[0], joinColumns[1]);
      } else {
        LeftJoin(typeof(JTable).Name, joinColumns[0], joinColumns[1]);
      }
      return this;
    }

    public Query<T> RightJoin<JTable>(Expression<Func<T, JTable, object>> expression) where JTable : class {
      RightJoin(null, expression);
      return this;
    }

    public Query<T> RightJoin<JTable>(string tableName, Expression<Func<T, JTable, object>> expression) where JTable : class {
      var joinColumns = expression.GetMemberNames();
      if (tableName != null && tableName.Length > 0) {
        RightJoin(tableName, joinColumns[0], joinColumns[1]);
      } else {
        RightJoin(typeof(JTable).Name, joinColumns[0], joinColumns[1]);
      }
      return this;
    }

    public Query<T> CrossJoin<JTable>() where JTable : class {
      CrossJoin<JTable>(null);
      return this;
    }

    public Query<T> CrossJoin<JTable>(string tableName) where JTable : class {
      if (tableName != null && tableName.Length > 0) {
        CrossJoin(tableName);
      } else {
        CrossJoin(typeof(JTable).Name);
      }
      return this;
    }

    public Query<T> OrderBy(Expression<Func<T, object>> columns) {
      OrderBy(columns.GetMemberNames());
      return this;
    }

    public Query<T> OrderByDesc(Expression<Func<T, object>> columns) {
      OrderByDesc(columns.GetMemberNames());
      return this;
    }

    public Query<T> Select(Expression<Func<T, object>> columns) {
      Select(columns.GetMemberNames());
      return this;
    }

    public Query<T> Where(Expression<Func<T, object>> expression) {
      var exp = expression.GetMemberName().Split();
      Where(exp[0], exp[1], exp[2]);
      return this;
    }

    public Query<T> WhereNot(Expression<Func<T, object>> expression) {
      var exp = expression.GetMemberName().Split();
      WhereNot(exp[0], exp[1], exp[2]);
      return this;
    }

    public Query<T> OrWhere(Expression<Func<T, object>> expression) {
      var exp = expression.GetMemberName().Split();
      OrWhere(exp[0], exp[1], exp[2]);
      return this;
    }

    public Query<T> OrWhereNot(Expression<Func<T, object>> expression) {
      var exp = expression.GetMemberName().Split();
      OrWhereNot(exp[0], exp[1], exp[2]);
      return this;
    }

    public Query<T> OrWhereColumns(Expression<Func<T, object>> expression) {
      var exp = expression.GetMemberName().Split();
      OrWhereColumns(exp[0], exp[1], exp[2]);
      return this;
    }

    public Query<T> WhereNull(Expression<Func<T, object>> expression) {
      WhereNull(expression.GetMemberName());
      return this;
    }

    public Query<T> WhereNotNull(Expression<Func<T, object>> expression) {
      WhereNotNull(expression.GetMemberName());
      return this;
    }

    public Query<T> OrWhereNull(Expression<Func<T, object>> expression) {
      OrWhereNull(expression.GetMemberName());
      return this;
    }

    public Query<T> OrWhereNotNull(Expression<Func<T, object>> expression) {
      OrWhereNotNull(expression.GetMemberName());
      return this;
    }

    public Query<T> WhereTrue(Expression<Func<T, object>> expression) {
      WhereTrue(expression.GetMemberName());
      return this;
    }

    public Query<T> OrWhereTrue(Expression<Func<T, object>> expression) {
      OrWhereTrue(expression.GetMemberName());
      return this;
    }

    public Query<T> WhereFalse(Expression<Func<T, object>> expression) {
      WhereFalse(expression.GetMemberName());
      return this;
    }

    public Query<T> OrWhereFalse(Expression<Func<T, object>> expression) {
      OrWhereFalse(expression.GetMemberName());
      return this;
    }

  }
}
