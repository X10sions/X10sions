using System.Linq.Expressions;

namespace LinqToDB;

public static class IDataContextExtensions {
  const string Declare_Global_Temporary_Table = "DECLARE GLOBAL TEMPORARY TABLE ";
  const string With_Replace = " WITH REPLACE";
  const string With_Data = " WITH DATA";
  public static string DeclareTempTableHeader(string tableName) => Declare_Global_Temporary_Table + tableName;
  public static string DeclareTempTableFooter(bool withData, bool withReplace) => $"{(withData ? With_Data : string.Empty)} {(withReplace ? With_Replace : string.Empty)}";

  [Obsolete("Use Async")]
  public static ITable<T> DeclareTempTable<T>(this IDataContext dataContext, string? tableName, bool withReplace, IQueryable<T>? queryable = null) {
    var tempTable = dataContext.CreateTable<T>(tableName, null, null, DeclareTempTableHeader("{0}"), DeclareTempTableFooter(false, withReplace), tableOptions: TableOptions.IsGlobalTemporaryStructure);
    if (queryable is not null) {
      queryable.Into(tempTable).Insert();
    }
    return tempTable;
  }

  public static ITable<T> DeclareTempTable<T>(this IDataContext dc, string tableName, bool withReplace, IEnumerable<T> data) where T : notnull {
    //var footer = (withReplace ? " WITH REPLACE" : "");
    //var tempTable = dc.CreateTable<T>(tableName, null, null, TempTableStatementHeaderFormat, footer);
    var tempTable = dc.CreateTable<T>(tableName, null, null, DeclareTempTableHeader("{0}"), DeclareTempTableFooter(false, withReplace), tableOptions: TableOptions.IsGlobalTemporaryStructure);
    if (data != null) {
      foreach (var row in data) {
        dc.Insert(row, tableName);
      }
    }
    return tempTable;
  }


  public async static Task<ITable<T>> DeclareTempTableAsync<T>(this IDataContext dataContext, string? tableName, bool withReplace, IQueryable<T>? queryable = null, CancellationToken cancellationToken = default) where T : notnull {
    var tempTable = await dataContext.CreateTableAsync<T>(tableName, null, null, DeclareTempTableHeader("{0}"), DeclareTempTableFooter(false, withReplace), tableOptions: TableOptions.IsGlobalTemporaryStructure, token: cancellationToken);
    if (queryable is not null) {
      await queryable.Into(tempTable).InsertAsync(cancellationToken);
    }
    return tempTable;
  }

  public static IModificationHandler<T> GetModificationHandler<T>(this IDataContext dataContext, Expression<Func<T, bool>> wherePredicate) where T : class => new ModificationHandler<T>(dataContext, wherePredicate);
  public static IQueryable<T> GetQueryable<T>(this IDataContext dataContext, Expression<Func<T, bool>> where) where T : class => dataContext.GetTable<T>().Where(where);
}