using X10sions.Fake.Data.Enums;
using X10sions.Fake.Data.Models;
using ServiceStack;
using ServiceStack.OrmLite;
using System.Data;
using X10sions.Fake.Data.Repositories;

namespace X10sions.Fake.Pages.Testing.Orm {
  public class OrmLite {

    public void CustomerOrdersExample(string connString, ConnectionStringName name) {
      // Setup SQL Server Connection Factory
      var dbFactory = new OrmLiteConnectionFactory(connString, name.GetOrmLiteDialectProvider());

      // Use in-memory Sqlite DB instead
      //var dbFactory = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider, false);

      //Non-intrusive: All extension methods hang off System.Data.* interfaces
      using var db = connString.OpenDbConnection();

      //Re-Create all table schemas:
      db.DropTable<FakeOrderDetail>();
      db.DropTable<FakeOrder>();
      db.DropTable<FakeCustomer>();
      db.DropTable<FakeProduct>();
      db.DropTable<FakeEmployee>();

      db.CreateTable<FakeEmployee>();
      db.CreateTable<FakeProduct>();
      db.CreateTable<FakeCustomer>();
      db.CreateTable<FakeOrder>();
      db.CreateTable<FakeOrderDetail>();

      db.Insert(new FakeEmployee { Id = 1, Name = "Employee 1" });
      db.Insert(new FakeEmployee { Id = 2, Name = "Employee 2" });
      var product1 = new FakeProduct { Id = 1, Name = "Product 1", UnitPrice = 10 };
      var product2 = new FakeProduct { Id = 2, Name = "Product 2", UnitPrice = 20 };
      db.Save(product1, product2);

      var customer = new FakeCustomer {
        FirstName = "Orm",
        LastName = "Lite",
        Email = "ormlite@servicestack.net",
        PhoneNumbers =          {
          { FakePhoneType.Home, "555-1234" },
          { FakePhoneType.Work, "1-800-1234" },
          { FakePhoneType.Mobile, "818-123-4567" },
        },
        Addresses =        {
          { FakeAddressType.Work, new FakeAddress {
            Line1 = "1 Street", Country = "US", State = "NY", City = "New York", ZipCode = "10101" }
          },
        },
        CreatedAt = DateTime.UtcNow,
      };

      var customerId = db.Insert(customer, selectIdentity: true); //Get Auto Inserted Id
      customer = db.Single<FakeCustomer>(new { customer.Email }); //Query

      //Direct access to System.Data.Transactions:
      using (IDbTransaction trans = db.OpenTransaction(IsolationLevel.ReadCommitted)) {
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
        db.Save(order); //Inserts 1st time

        //order.Id populated on Save().

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

        db.Save(orderDetails);

        order.Total = orderDetails.Sum(x => x.UnitPrice * x.Quantity * x.Discount) + order.Freight;

        db.Save(order); //Updates 2nd Time

        trans.Commit();
      }
    }

    public async Task CustomerOrdersExampleAsync(IDbConnection db) {

      await db.InsertAsync(new FakeEmployee { Id = 1, Name = "Employee 1" });
      await db.InsertAsync(new FakeEmployee { Id = 2, Name = "Employee 2" });
      var product1 = new FakeProduct { Id = 1, Name = "Product 1", UnitPrice = 10 };
      var product2 = new FakeProduct { Id = 2, Name = "Product 2", UnitPrice = 20 };
      await db.SaveAsync(product1, product2);

      var customer = new FakeCustomer {
        FirstName = "Orm",
        LastName = "Lite",
        Email = "ormlite@servicestack.net",
        PhoneNumbers =          {
          { FakePhoneType.Home, "555-1234" },
          { FakePhoneType.Work, "1-800-1234" },
          { FakePhoneType.Mobile, "818-123-4567" },
        },
        Addresses =          {
          { FakeAddressType.Work, new FakeAddress {
            Line1 = "1 Street", Country = "US", State = "NY", City = "New York", ZipCode = "10101" }
          },
        },
        CreatedAt = DateTime.UtcNow,
      };

      var customerId = await db.InsertAsync(customer, selectIdentity: true); //Get Auto Inserted Id
      customer = await db.SingleAsync<FakeCustomer>(new { customer.Email }); //Query

      //Direct access to System.Data.Transactions:
      using var trans = db.OpenTransaction(IsolationLevel.ReadCommitted);
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
      await db.SaveAsync(order); //Inserts 1st time

      //order.Id populated on Save().
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

      await db.SaveAsync(orderDetails);

      order.Total = orderDetails.Sum(x => x.UnitPrice * x.Quantity * x.Discount) + order.Freight;

      await db.SaveAsync(order); //Updates 2nd Time

      trans.Commit();

    }

    public void SimpleExmaple() {

      OrmLiteConfig.DialectProvider = SqliteDialect.Provider;

      using (IDbConnection db = "/path/to/db.sqlite".OpenDbConnection()) {
        db.CreateTable<FakeIdName>(true);
        db.Insert(new FakeIdName { Id = 1, Name = "Hello, World!" });
        var rows = db.Select<FakeIdName>();

        //   Assert.That(rows, Has.Count(1));
        //   Assert.That(rows[0].Id, Is.EqualTo(1));
      }
    }

    public void MultiDbExample(string connString, string shardId) {
      const int NoOfShards = 10;
      const int NoOfRobots = 1000;

      var dbFactory = new OrmLiteConnectionFactory(connString, SqlServerDialect.Provider);

      dbFactory.OpenDbConnection().Run(db => db.CreateTable<FakeMasterRecord>(overwrite: false));

      NoOfShards.Times(i => {
        var namedShard = "robots-shard" + i;
        dbFactory.RegisterConnection(namedShard, $"~/App_Data/{shardId}.sqlite".MapAbsolutePath(), SqliteDialect.Provider);

        dbFactory.OpenDbConnection(namedShard).Run(db => db.CreateTable<FakeRobot>(overwrite: false));
      });

      var newRobots = NoOfRobots.Times(i => //Create 1000 Robots
          new FakeRobot { Id = i, Name = "R2D" + i, CreatedDate = DateTime.UtcNow, CellCount = DateTime.Now.Millisecond });

      foreach (var newRobot in newRobots) {
        using (var db = dbFactory.OpenDbConnection()) {
          db.Insert(new FakeMasterRecord { Id = Guid.NewGuid(), RobotId = newRobot.Id, RobotName = newRobot.Name });
          using (IDbConnection robotShard = dbFactory.OpenDbConnection("robots-shard" + newRobot.Id % NoOfShards)) {
            robotShard.Insert(newRobot);
          }
        }
      }

    }

  }

}
