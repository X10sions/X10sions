using IQToolkit;
using System;
using System.Linq;

namespace Test {
  public abstract class NorthwindCUDTests : NorthwindTestBase {
    public override void RunTest(Action testAction) {
      CleaupDatabase();
      base.RunTest(testAction);
    }

    public override void Setup(string[] args) => base.Setup(args);

    public override void Teardown() {
      CleaupDatabase();
      base.Teardown();
    }

    private void CleaupDatabase() {
      ExecSilent("DELETE FROM Orders WHERE CustomerID LIKE 'XX%'");
      ExecSilent("DELETE FROM Customers WHERE CustomerID LIKE 'XX%'");
    }

    public void TestInsertCustomerNoResult() {
      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Seattle",
        Country = "USA"
      };
      var result = db.Customers.Insert(cust);
      Assert.Equal(1, result);  // returns 1 for success
    }

    public void TestInsertCustomerWithResult() {
      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Seattle",
        Country = "USA"
      };
      var result = db.Customers.Insert(cust, c => c.City);
      Assert.Equal(result, "Seattle");  // should be value we asked for
    }

    public void TestBatchInsertCustomersNoResult() {
      var n = 10;
      var custs = Enumerable.Range(1, n).Select(
          i => new Customer {
            CustomerID = "XX" + i,
            CompanyName = "Company" + i,
            ContactName = "Contact" + i,
            City = "Seattle",
            Country = "USA"
          });
      var results = db.Customers.Batch(custs, (u, c) => u.Insert(c));
      Assert.Equal(n, results.Count());
      Assert.Equal(true, results.All(r => object.Equals(r, 1)));
    }

    public void TestBatchInsertCustomersWithResult() {
      var n = 10;
      var custs = Enumerable.Range(1, n).Select(
          i => new Customer {
            CustomerID = "XX" + i,
            CompanyName = "Company" + i,
            ContactName = "Contact" + i,
            City = "Seattle",
            Country = "USA"
          });
      var results = db.Customers.Batch(custs, (u, c) => u.Insert(c, d => d.City));
      Assert.Equal(n, results.Count());
      Assert.Equal(true, results.All(r => object.Equals(r, "Seattle")));
    }

    [ExcludeProvider("Access")] // problem with generating OrderID
    public void TestInsertOrderWithNoResult() {
      TestInsertCustomerNoResult(); // create customer "XX1"
      var order = new Order {
        CustomerID = "XX1",
        OrderDate = DateTime.Today,
      };
      var result = db.Orders.Insert(order);
      Assert.Equal(1, result);
    }

    [ExcludeProvider("Access")] // problem with generating OrderID
    public void TestInsertOrderWithGeneratedIDResult() {
      TestInsertCustomerNoResult(); // create customer "XX1"
      var order = new Order {
        CustomerID = "XX1",
        OrderDate = DateTime.Today,
      };
      var result = db.Orders.Insert(order, o => o.OrderID);
      Assert.NotEqual(1, result);
    }

    public void TestUpdateCustomerNoResult() {
      TestInsertCustomerNoResult(); // create customer "XX1"

      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Portland", // moved to Portland!
        Country = "USA"
      };

      var result = db.Customers.Update(cust);
      Assert.Equal(1, result);
    }

    public void TestUpdateCustomerWithResult() {
      TestInsertCustomerNoResult(); // create customer "XX1"

      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Portland", // moved to Portland!
        Country = "USA"
      };

      var result = db.Customers.Update(cust, null, c => c.City);
      Assert.Equal("Portland", result);
    }

    public void TestUpdateCustomerWithUpdateCheckThatDoesNotSucceed() {
      TestInsertCustomerNoResult(); // create customer "XX1"

      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Portland", // moved to Portland!
        Country = "USA"
      };

      var result = db.Customers.Update(cust, d => d.City == "Detroit");
      Assert.Equal(0, result); // 0 for failure
    }

    public void TestUpdateCustomerWithUpdateCheckThatSucceeds() {
      TestInsertCustomerNoResult(); // create customer "XX1"

      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Portland", // moved to Portland!
        Country = "USA"
      };

      var result = db.Customers.Update(cust, d => d.City == "Seattle");
      Assert.Equal(1, result);
    }

    public void TestBatchUpdateCustomer() {
      TestBatchInsertCustomersNoResult();

      var n = 10;
      var custs = Enumerable.Range(1, n).Select(
          i => new Customer {
            CustomerID = "XX" + i,
            CompanyName = "Company" + i,
            ContactName = "Contact" + i,
            City = "Seattle",
            Country = "USA"
          });

      var results = db.Customers.Batch(custs, (u, c) => u.Update(c));
      Assert.Equal(n, results.Count());
      Assert.Equal(true, results.All(r => object.Equals(r, 1)));
    }

    public void TestBatchUpdateCustomerWithUpdateCheck() {
      TestBatchInsertCustomersNoResult();

      var n = 10;
      var pairs = Enumerable.Range(1, n).Select(
          i => new {
            original = new Customer {
              CustomerID = "XX" + i,
              CompanyName = "Company" + i,
              ContactName = "Contact" + i,
              City = "Seattle",
              Country = "USA"
            },
            current = new Customer {
              CustomerID = "XX" + i,
              CompanyName = "Company" + i,
              ContactName = "Contact" + i,
              City = "Portland",
              Country = "USA"
            }
          });

      var results = db.Customers.Batch(pairs, (u, x) => u.Update(x.current, d => d.City == x.original.City));
      Assert.Equal(n, results.Count());
      Assert.Equal(true, results.All(r => object.Equals(r, 1)));
    }

    public void TestBatchUpdateCustomerWithResult() {
      TestBatchInsertCustomersNoResult();

      var n = 10;
      var custs = Enumerable.Range(1, n).Select(
          i => new Customer {
            CustomerID = "XX" + i,
            CompanyName = "Company" + i,
            ContactName = "Contact" + i,
            City = "Portland",
            Country = "USA"
          });

      var results = db.Customers.Batch(custs, (u, c) => u.Update(c, null, d => d.City));
      Assert.Equal(n, results.Count());
      Assert.Equal(true, results.All(r => object.Equals(r, "Portland")));
    }

    public void TestBatchUpdateCustomerWithUpdateCheckAndResult() {
      TestBatchInsertCustomersNoResult();

      var n = 10;
      var pairs = Enumerable.Range(1, n).Select(
          i => new {
            original = new Customer {
              CustomerID = "XX" + i,
              CompanyName = "Company" + i,
              ContactName = "Contact" + i,
              City = "Seattle",
              Country = "USA"
            },
            current = new Customer {
              CustomerID = "XX" + i,
              CompanyName = "Company" + i,
              ContactName = "Contact" + i,
              City = "Portland",
              Country = "USA"
            }
          });

      var results = db.Customers.Batch(pairs, (u, x) => u.Update(x.current, d => d.City == x.original.City, d => d.City));
      Assert.Equal(n, results.Count());
      Assert.Equal(true, results.All(r => object.Equals(r, "Portland")));
    }

    public void TestUpsertNewCustomerNoResult() {
      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Seattle", // moved to Portland!
        Country = "USA"
      };

      var result = db.Customers.InsertOrUpdate(cust);
      Assert.Equal(1, result);
    }

    public void TestUpsertExistingCustomerNoResult() {
      TestInsertCustomerNoResult();

      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Portland", // moved to Portland!
        Country = "USA"
      };

      var result = db.Customers.InsertOrUpdate(cust);
      Assert.Equal(1, result);
    }

    public void TestUpsertNewCustomerWithResult() {
      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Seattle", // moved to Portland!
        Country = "USA"
      };

      var result = db.Customers.InsertOrUpdate(cust, null, d => d.City);
      Assert.Equal("Seattle", result);
    }

    public void TestUpsertExistingCustomerWithResult() {
      TestInsertCustomerNoResult();

      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Portland", // moved to Portland!
        Country = "USA"
      };

      var result = db.Customers.InsertOrUpdate(cust, null, d => d.City);
      Assert.Equal("Portland", result);
    }

    public void TestUpsertNewCustomerWithUpdateCheck() {
      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Portland", // moved to Portland!
        Country = "USA"
      };

      var result = db.Customers.InsertOrUpdate(cust, d => d.City == "Portland");
      Assert.Equal(1, result);
    }

    public void TestUpsertExistingCustomerWithUpdateCheck() {
      TestInsertCustomerNoResult();

      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Portland", // moved to Portland!
        Country = "USA"
      };

      var result = db.Customers.InsertOrUpdate(cust, d => d.City == "Seattle");
      Assert.Equal(1, result);
    }

    public void TestBatchUpsertNewCustomersNoResult() {
      var n = 10;
      var custs = Enumerable.Range(1, n).Select(
          i => new Customer {
            CustomerID = "XX" + i,
            CompanyName = "Company" + i,
            ContactName = "Contact" + i,
            City = "Portland",
            Country = "USA"
          });

      var results = db.Customers.Batch(custs, (u, c) => u.InsertOrUpdate(c));
      Assert.Equal(n, results.Count());
      Assert.Equal(true, results.All(r => object.Equals(r, 1)));
    }

    public void TestBatchUpsertExistingCustomersNoResult() {
      TestBatchInsertCustomersNoResult();

      var n = 10;
      var custs = Enumerable.Range(1, n).Select(
          i => new Customer {
            CustomerID = "XX" + i,
            CompanyName = "Company" + i,
            ContactName = "Contact" + i,
            City = "Portland",
            Country = "USA"
          });

      var results = db.Customers.Batch(custs, (u, c) => u.InsertOrUpdate(c));
      Assert.Equal(n, results.Count());
      Assert.Equal(true, results.All(r => object.Equals(r, 1)));
    }

    public void TestBatchUpsertNewCustomersWithResult() {
      var n = 10;
      var custs = Enumerable.Range(1, n).Select(
          i => new Customer {
            CustomerID = "XX" + i,
            CompanyName = "Company" + i,
            ContactName = "Contact" + i,
            City = "Portland",
            Country = "USA"
          });

      var results = db.Customers.Batch(custs, (u, c) => u.InsertOrUpdate(c, null, d => d.City));
      Assert.Equal(n, results.Count());
      Assert.Equal(true, results.All(r => object.Equals(r, "Portland")));
    }

    public void TestBatchUpsertExistingCustomersWithResult() {
      TestBatchInsertCustomersNoResult();

      var n = 10;
      var custs = Enumerable.Range(1, n).Select(
          i => new Customer {
            CustomerID = "XX" + i,
            CompanyName = "Company" + i,
            ContactName = "Contact" + i,
            City = "Portland",
            Country = "USA"
          });

      var results = db.Customers.Batch(custs, (u, c) => u.InsertOrUpdate(c, null, d => d.City));
      Assert.Equal(n, results.Count());
      Assert.Equal(true, results.All(r => object.Equals(r, "Portland")));
    }

    public void TestBatchUpsertNewCustomersWithUpdateCheck() {
      var n = 10;
      var pairs = Enumerable.Range(1, n).Select(
          i => new {
            original = new Customer {
              CustomerID = "XX" + i,
              CompanyName = "Company" + i,
              ContactName = "Contact" + i,
              City = "Seattle",
              Country = "USA"
            },
            current = new Customer {
              CustomerID = "XX" + i,
              CompanyName = "Company" + i,
              ContactName = "Contact" + i,
              City = "Portland",
              Country = "USA"
            }
          });

      var results = db.Customers.Batch(pairs, (u, x) => u.InsertOrUpdate(x.current, d => d.City == x.original.City));
      Assert.Equal(n, results.Count());
      Assert.Equal(true, results.All(r => object.Equals(r, 1)));
    }

    public void TestBatchUpsertExistingCustomersWithUpdateCheck() {
      TestBatchInsertCustomersNoResult();

      var n = 10;
      var pairs = Enumerable.Range(1, n).Select(
          i => new {
            original = new Customer {
              CustomerID = "XX" + i,
              CompanyName = "Company" + i,
              ContactName = "Contact" + i,
              City = "Seattle",
              Country = "USA"
            },
            current = new Customer {
              CustomerID = "XX" + i,
              CompanyName = "Company" + i,
              ContactName = "Contact" + i,
              City = "Portland",
              Country = "USA"
            }
          });

      var results = db.Customers.Batch(pairs, (u, x) => u.InsertOrUpdate(x.current, d => d.City == x.original.City));
      Assert.Equal(n, results.Count());
      Assert.Equal(true, results.All(r => object.Equals(r, 1)));
    }

    public void TestDeleteCustomer() {
      TestInsertCustomerNoResult();

      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Seattle",
        Country = "USA"
      };

      var result = db.Customers.Delete(cust);
      Assert.Equal(1, result);
    }

    public void TestDeleteCustomerForNonExistingCustomer() {
      TestInsertCustomerNoResult();

      var cust = new Customer {
        CustomerID = "XX2",
        CompanyName = "Company2",
        ContactName = "Contact2",
        City = "Seattle",
        Country = "USA"
      };

      var result = db.Customers.Delete(cust);
      Assert.Equal(0, result);
    }

    public void TestDeleteCustomerWithDeleteCheckThatSucceeds() {
      TestInsertCustomerNoResult();

      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Seattle",
        Country = "USA"
      };

      var result = db.Customers.Delete(cust, d => d.City == "Seattle");
      Assert.Equal(1, result);
    }

    public void TestDeleteCustomerWithDeleteCheckThatDoesNotSucceed() {
      TestInsertCustomerNoResult();

      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Seattle",
        Country = "USA"
      };

      var result = db.Customers.Delete(cust, d => d.City == "Portland");
      Assert.Equal(0, result);
    }

    public void TestBatchDeleteCustomers() {
      TestBatchInsertCustomersNoResult();

      var n = 10;
      var custs = Enumerable.Range(1, n).Select(
          i => new Customer {
            CustomerID = "XX" + i,
            CompanyName = "Company" + i,
            ContactName = "Contact" + i,
            City = "Seattle",
            Country = "USA"
          });

      var results = db.Customers.Batch(custs, (u, c) => u.Delete(c));
      Assert.Equal(n, results.Count());
      Assert.Equal(true, results.All(r => object.Equals(r, 1)));
    }

    public void TestBatchDeleteCustomersWithDeleteCheck() {
      TestBatchInsertCustomersNoResult();

      var n = 10;
      var custs = Enumerable.Range(1, n).Select(
          i => new Customer {
            CustomerID = "XX" + i,
            CompanyName = "Company" + i,
            ContactName = "Contact" + i,
            City = "Seattle",
            Country = "USA"
          });

      var results = db.Customers.Batch(custs, (u, c) => u.Delete(c, d => d.City == c.City));
      Assert.Equal(n, results.Count());
      Assert.Equal(true, results.All(r => object.Equals(r, 1)));
    }

    public void TestBatchDeleteCustomersWithDeleteCheckThatDoesNotSucceed() {
      TestBatchInsertCustomersNoResult();

      var n = 10;
      var custs = Enumerable.Range(1, n).Select(
          i => new Customer {
            CustomerID = "XX" + i,
            CompanyName = "Company" + i,
            ContactName = "Contact" + i,
            City = "Portland",
            Country = "USA"
          });

      var results = db.Customers.Batch(custs, (u, c) => u.Delete(c, d => d.City == c.City));
      Assert.Equal(n, results.Count());
      Assert.Equal(true, results.All(r => object.Equals(r, 0)));
    }

    public void TestDeleteWhere() {
      TestBatchInsertCustomersNoResult();

      var result = db.Customers.Delete(c => c.CustomerID.StartsWith("XX"));
      Assert.Equal(10, result);
    }

    public void TestSessionIdentityCache() {
      var ns = new NorthwindSession(GetProvider());

      // both objects should be the same instance
      var cust = ns.Customers.Single(c => c.CustomerID == "ALFKI");
      var cust2 = ns.Customers.Single(c => c.CustomerID == "ALFKI");

      Assert.NotEqual(null, cust);
      Assert.NotEqual(null, cust2);
      Assert.Equal(cust, cust2);
    }

    public void TestSessionProviderNotIdentityCached() {
      var ns = new NorthwindSession(GetProvider());
      var db2 = new Northwind(ns.Session.Provider);

      // both objects should be different instances
      var cust = ns.Customers.Single(c => c.CustomerID == "ALFKI");
      var cust2 = ns.Customers.Table.Single(c => c.CustomerID == "ALFKI");

      Assert.NotEqual(null, cust);
      Assert.NotEqual(null, cust2);
      Assert.Equal(cust.CustomerID, cust2.CustomerID);
      Assert.NotEqual(cust, cust2);
    }

    public void TestSessionSubmitActionOnModify() {
      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Seattle",
        Country = "USA"
      };

      db.Customers.Insert(cust);

      var ns = new NorthwindSession(GetProvider());
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));

      // fetch the previously inserted customer
      cust = ns.Customers.Single(c => c.CustomerID == "XX1");
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));

      cust.ContactName = "Contact Modified";
      Assert.Equal(SubmitAction.Update, ns.Customers.GetSubmitAction(cust));

      ns.SubmitChanges();
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));

      // prove actually modified by fetching through provider
      var cust2 = db.Customers.Single(c => c.CustomerID == "XX1");
      Assert.Equal("Contact Modified", cust2.ContactName);

      // ready to be submitted again!
      cust.City = "SeattleX";
      Assert.Equal(SubmitAction.Update, ns.Customers.GetSubmitAction(cust));
    }

    public void TestSessionSubmitActionOnInsert() {
      var ns = new NorthwindSession(GetProvider());
      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Seattle",
        Country = "USA"
      };
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));

      ns.Customers.InsertOnSubmit(cust);
      Assert.Equal(SubmitAction.Insert, ns.Customers.GetSubmitAction(cust));

      ns.SubmitChanges();
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));

      cust.City = "SeattleX";
      Assert.Equal(SubmitAction.Update, ns.Customers.GetSubmitAction(cust));
    }

    public void TestSessionSubmitActionOnInsertOrUpdate() {
      var ns = new NorthwindSession(GetProvider());
      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Seattle",
        Country = "USA"
      };
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));

      ns.Customers.InsertOrUpdateOnSubmit(cust);
      Assert.Equal(SubmitAction.InsertOrUpdate, ns.Customers.GetSubmitAction(cust));

      ns.SubmitChanges();
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));

      cust.City = "SeattleX";
      Assert.Equal(SubmitAction.Update, ns.Customers.GetSubmitAction(cust));
    }

    public void TestSessionSubmitActionOnUpdate() {
      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Seattle",
        Country = "USA"
      };
      db.Customers.Insert(cust);

      var ns = new NorthwindSession(GetProvider());
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));

      ns.Customers.UpdateOnSubmit(cust);
      Assert.Equal(SubmitAction.Update, ns.Customers.GetSubmitAction(cust));

      ns.SubmitChanges();
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));

      cust.City = "SeattleX";
      Assert.Equal(SubmitAction.Update, ns.Customers.GetSubmitAction(cust));
    }

    public void TestSessionSubmitActionOnDelete() {
      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Seattle",
        Country = "USA"
      };
      db.Customers.Insert(cust);

      var ns = new NorthwindSession(GetProvider());
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));

      ns.Customers.DeleteOnSubmit(cust);
      Assert.Equal(SubmitAction.Delete, ns.Customers.GetSubmitAction(cust));

      ns.SubmitChanges();
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));

      // modifications after delete don't trigger updates
      cust.City = "SeattleX";
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));
    }

    public void TestDeleteThenInsertSamePK() {
      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Seattle",
        Country = "USA"
      };

      var cust2 = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company2",
        ContactName = "Contact2",
        City = "Chicago",
        Country = "USA"
      };

      db.Customers.Insert(cust);

      var ns = new NorthwindSession(GetProvider());
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust2));

      ns.Customers.DeleteOnSubmit(cust);
      Assert.Equal(SubmitAction.Delete, ns.Customers.GetSubmitAction(cust));
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust2));

      ns.Customers.InsertOnSubmit(cust2);
      Assert.Equal(SubmitAction.Delete, ns.Customers.GetSubmitAction(cust));
      Assert.Equal(SubmitAction.Insert, ns.Customers.GetSubmitAction(cust2));

      ns.SubmitChanges();
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust2));

      // modifications after delete don't trigger updates
      cust.City = "SeattleX";
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));

      // modifications after insert do trigger updates
      cust2.City = "ChicagoX";
      Assert.Equal(SubmitAction.Update, ns.Customers.GetSubmitAction(cust2));
    }

    public void TestInsertThenDeleteSamePK() {
      var cust = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company1",
        ContactName = "Contact1",
        City = "Seattle",
        Country = "USA"
      };

      var cust2 = new Customer {
        CustomerID = "XX1",
        CompanyName = "Company2",
        ContactName = "Contact2",
        City = "Chicago",
        Country = "USA"
      };

      db.Customers.Insert(cust);

      var ns = new NorthwindSession(GetProvider());
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust2));

      ns.Customers.InsertOnSubmit(cust2);
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));
      Assert.Equal(SubmitAction.Insert, ns.Customers.GetSubmitAction(cust2));

      ns.Customers.DeleteOnSubmit(cust);
      Assert.Equal(SubmitAction.Delete, ns.Customers.GetSubmitAction(cust));
      Assert.Equal(SubmitAction.Insert, ns.Customers.GetSubmitAction(cust2));

      ns.SubmitChanges();
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust2));

      // modifications after delete don't trigger updates
      cust.City = "SeattleX";
      Assert.Equal(SubmitAction.None, ns.Customers.GetSubmitAction(cust));

      // modifications after insert do trigger updates
      cust2.City = "ChicagoX";
      Assert.Equal(SubmitAction.Update, ns.Customers.GetSubmitAction(cust2));
    }

    [ExcludeProvider("Access")] // Access does not auto generate the OrderID
    public void TestSessionGeneratedId() {
      TestInsertCustomerNoResult(); // create customer "XX1"

      var ns = new NorthwindSession(GetProvider());

      var order = new Order {
        CustomerID = "XX1",
        OrderDate = DateTime.Today,
      };

      ns.Orders.InsertOnSubmit(order);

      Assert.Equal(0, order.OrderID);
      ns.SubmitChanges();

      Assert.NotEqual(0, order.OrderID);
    }
  }
}
