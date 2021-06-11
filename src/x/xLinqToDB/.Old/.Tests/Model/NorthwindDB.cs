using LinqToDB.Data;
using LinqToDB.DataProvider.SqlServer;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqToDB.Tests.Model {
  public class NorthwindDB : DataConnection {
    public NorthwindDB(string configurationString) : base(configurationString) { }

    public ITable<Northwind.Category> Category => GetTable<Northwind.Category>();
    public ITable<Northwind.Customer> Customer => GetTable<Northwind.Customer>();
    public ITable<Northwind.Employee> Employee => GetTable<Northwind.Employee>();
    public ITable<Northwind.EmployeeTerritory> EmployeeTerritory => GetTable<Northwind.EmployeeTerritory>();
    public ITable<Northwind.OrderDetail> OrderDetail => GetTable<Northwind.OrderDetail>();
    public ITable<Northwind.Order> Order => GetTable<Northwind.Order>();
    public ITable<Northwind.Product> Product => GetTable<Northwind.Product>();
    public ITable<Northwind.ActiveProduct> ActiveProduct => GetTable<Northwind.ActiveProduct>();
    public ITable<Northwind.DiscontinuedProduct> DiscontinuedProduct => GetTable<Northwind.DiscontinuedProduct>();
    public ITable<Northwind.Region> Region => GetTable<Northwind.Region>();
    public ITable<Northwind.Shipper> Shipper => GetTable<Northwind.Shipper>();
    public ITable<Northwind.Supplier> Supplier => GetTable<Northwind.Supplier>();
    public ITable<Northwind.Territory> Territory => GetTable<Northwind.Territory>();

    public IQueryable<SqlServerExtensions.FreeTextKey<TKey>> FreeTextTable<TTable, TKey>(
      ITable<TTable> table,
      Expression<Func<TTable, object?>> columns,
      string search) => Sql.Ext.SqlServer().FreeTextTable<TTable, TKey>(table, columns, search);

    [Sql.TableExpression("{0} {1} WITH (UPDLOCK)")]
    public ITable<T> WithUpdateLock<T>() where T : class {
      var methodInfo = typeof(NorthwindDB).GetMethod("WithUpdateLock")!.MakeGenericMethod(typeof(T));
      return GetTable<T>(this, methodInfo);
    }
  }
}