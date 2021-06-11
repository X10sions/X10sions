using Tests.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualBasic {
  public static class VisualBasicCommon {

    public static IEnumerable<Parent> ParamenterName(ITestDataContext db) {
      int id;
      id = 1;
      return from p in db.Parent
             where p.ParentID == id
             select p;
    }

    public static IEnumerable<LinqDataTypes> SearchCondition1(ITestDataContext db) {
      return from t in db.Types
             where !t.BoolValue & (t.SmallIntValue == 5 | t.SmallIntValue == 7 | (t.SmallIntValue | 2) == 10)
             select t;
    }

    public static IEnumerable<string> SearchCondition2(NorthwindDB db) {
      return from cust in db.Customer
             where cust.Orders.Count > 0 & cust.CompanyName.StartsWith("H")
             select cust.CustomerID;
    }

    public static IEnumerable<int> SearchCondition3(NorthwindDB db) {
      // #11/14/1997#
      var query = from order in db.Order
                  where order.OrderDate == new DateTime(1997, 11, 14)
                  select order.OrderID;

      return query;
    }

    public static IEnumerable<int> SearchCondition4(NorthwindDB db) {
      var query = from order in db.Order
                  where new DateTime(1997, 11, 14) == order.OrderDate
                  select order.OrderID;
      return query;
    }

  }

}
