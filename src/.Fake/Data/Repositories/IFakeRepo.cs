using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using X10sions.Fake.Data.Enums;
using X10sions.Fake.Data.Models;

namespace X10sions.Fake.Data.Repositories {
  public interface IFakeRepo {
    IDbConnection DbConnection { get; }
    IQueryable<T> GetQueryable<T>() where T : class;
    void CreateTable<T>() where T : class;
    void DropTable<T>() where T : class;

    long Delete<T>(IEnumerable<T> rows) where T : class;
    long Insert<T>(IEnumerable<T> rows) where T : class;
    T InsertWithIdentity<T>(T row) where T : class;
    long Update<T>(IEnumerable<T> rows) where T : class;

    //List<T> Select<T>() where T : class;
    //IEnumerable<T> Select<T>() where T : class;
    //IEnumerable<T> SelectMany<T>(Expression<Func<T, bool>> where) where T : class;
    //T? SelectOne<T>(Expression<Func<T, bool>> where) where T : class;


  }

  public static class IFakeRepoExtensions {
    public static IQueryable<T> GetQueryable<T>(this IFakeRepo fakeRepo, Expression<Func<T, bool>> where) where T : class => fakeRepo.GetQueryable<T>().Where(where);
    public static void CreateTable<T>(this IFakeRepo fakeRepo, bool overwrite) where T : class {
      if (overwrite) {
        fakeRepo.DropTable<T>();
      }
      fakeRepo.CreateTable<T>();
    }
    public static long Insert<T>(this IFakeRepo fakeRepo, T obj) where T : class => fakeRepo.Insert(new[] { obj });
    public static long Delete<T>(this IFakeRepo fakeRepo, T obj) where T : class => fakeRepo.Delete(new[] { obj });
    public static long Update<T>(this IFakeRepo fakeRepo, T obj) where T : class => fakeRepo.Update(new[] { obj });
    public static T? Select<T>(this IFakeRepo fakeRepo, Expression<Func<T, bool>> where) where T : class => fakeRepo.GetQueryable(where).FirstOrDefault();
    public static List<T> SelectList<T>(this IFakeRepo fakeRepo) where T : class => fakeRepo.SelectList<T>(x => true);
    public static List<T> SelectList<T>(this IFakeRepo fakeRepo, Expression<Func<T, bool>> where) where T : class => fakeRepo.GetQueryable(where).ToList();

    public static IDbConnection EnsureOpen(this IDbConnection connection) {
      if (connection.State != ConnectionState.Open) {
        connection.Open();
      }
      return connection;
    }

    public static void SimpleExample(this IFakeRepo fakeRepo, bool doDropTable, bool doCreateTable) {
      //OrmLiteConfig.DialectProvider = SqliteDialect.Provider;
      using (IDbConnection db = fakeRepo.DbConnection.EnsureOpen()) {
        if (doDropTable) {
          fakeRepo.DropFakeTables();
        }
        if (doCreateTable) {
          fakeRepo.CreateFakeTables();
        }
        fakeRepo.Insert(new FakeIdName { Id = 1, Name = "Hello, World!" });
        var rows = fakeRepo.SelectList<FakeIdName>();
        //   Assert.That(rows, Has.Count(1));
        //   Assert.That(rows[0].Id, Is.EqualTo(1));
      }
    }

    public static void DropFakeTables(this IFakeRepo fakeRepo) {
      fakeRepo.DropTable<FakeIdName>();
      fakeRepo.DropTable<FakeOrderDetail>();
      fakeRepo.DropTable<FakeOrder>();
      fakeRepo.DropTable<FakeCustomer>();
      fakeRepo.DropTable<FakeProduct>();
      fakeRepo.DropTable<FakeEmployee>();
    }
    public static void CreateFakeTables(this IFakeRepo fakeRepo) {
      fakeRepo.CreateTable<FakeIdName>(false);
      fakeRepo.CreateTable<FakeEmployee>();
      fakeRepo.CreateTable<FakeProduct>();
      fakeRepo.CreateTable<FakeCustomer>();
      fakeRepo.CreateTable<FakeOrder>();
      fakeRepo.CreateTable<FakeOrderDetail>();
    }

    public static void CustomerOrdersExample(this IFakeRepo fakeRepo, bool doDropTable, bool doCreateTable, string connString, ConnectionStringName name) {
      using (IDbConnection db = fakeRepo.DbConnection.EnsureOpen()) {
        if (doDropTable) {
          fakeRepo.DropFakeTables();
        }
        if (doCreateTable) {
          fakeRepo.CreateFakeTables();
        }
      }

      fakeRepo.Insert(new FakeEmployee { Id = 1, Name = "Employee 1" });
      fakeRepo.Insert(new FakeEmployee { Id = 2, Name = "Employee 2" });
      var product1 = new FakeProduct { Id = 1, Name = "Product 1", UnitPrice = 10 };
      var product2 = new FakeProduct { Id = 2, Name = "Product 2", UnitPrice = 20 };
      fakeRepo.Insert(new[] { product1, product2 });

      product2.Name = name + "-updated";
      fakeRepo.Update(new[] { product2 });

      var customer = new FakeCustomer {
        FirstName = "Fake",
        LastName = "Orm",
        Email = "orm@fake.com",
        PhoneNumbers = {
          { FakePhoneType.Home, "555-1234" },
          { FakePhoneType.Work, "1-800-1234" },
          { FakePhoneType.Mobile, "818-123-4567" },
        },
        Addresses = {
          { FakeAddressType.Work, new FakeAddress {
            Line1 = "1 Street", Country = "US", State = "NY", City = "New York", ZipCode = "10101" }
          },
        },
        CreatedAt = DateTime.UtcNow,
      };

      var newCustomer = fakeRepo.InsertWithIdentity(customer); //Get Auto Inserted Id

      customer = fakeRepo.Select<FakeCustomer>(x => x.Email == customer.Email);

      var order = new FakeOrder {
        CustomerId = customer.Id,
        EmployeeId = 1,
        OrderDate = DateTime.UtcNow,
        Freight = 10.50m,
        ShippingAddress = new FakeAddress {
          Line1 = "3 Street",
          Country = "US",
          State = "NY",
          City = "New York",
          ZipCode = "12121"
        },
      };
      fakeRepo.Insert(order); //Inserts 1st time

      var orderDetails = new[] {
        new FakeOrderDetail {
          OrderId = order.Id,
          ProductId = product1.Id,
          Quantity = 2,
          UnitPrice = product1.UnitPrice,
        },
        new FakeOrderDetail {
          OrderId = order.Id,
          ProductId = product2.Id,
          Quantity = 2,
          UnitPrice = product2.UnitPrice,
          Discount = .15m,
        }
      };
      fakeRepo.Insert(orderDetails);
      order.Total = orderDetails.Sum(x => x.UnitPrice * x.Quantity * x.Discount) + order.Freight;
      fakeRepo.Update(order); //Updates 2nd Time
    }
  }

  public abstract class FakeRepo : IFakeRepo {
    public FakeRepo(DbConnection dbConnection) {
      DbConnection = dbConnection;
    }

    public FakeRepo(ConnectionStringName name, IConfiguration configuration) : this(name.GetDbConnection(configuration) ?? throw new ArgumentNullException(nameof(DbConnection))) { }

    public IDbConnection DbConnection { get; }
    public abstract IQueryable<T> GetQueryable<T>() where T : class;
    public abstract void CreateTable<T>() where T : class;
    public abstract void DropTable<T>() where T : class;
    public abstract long Delete<T>(IEnumerable<T> rows) where T : class;
    public abstract long Insert<T>(IEnumerable<T> rows) where T : class;
    public abstract T InsertWithIdentity<T>(T row) where T : class;
    public abstract long Update<T>(IEnumerable<T> rows) where T : class;
    //public abstract IEnumerable<T> SelectMany<T>(Expression<Func<T, bool>> where) where T : class;
    //public abstract T? SelectOne<T>(Expression<Func<T, bool>> where) where T :  class;
  }
}