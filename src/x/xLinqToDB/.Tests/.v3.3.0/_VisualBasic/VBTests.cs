using System;
using System.Linq;
using System.Collections.Generic;
using LinqToDB;
using LinqToDB.Mapping;
using Tests.Model;

namespace VisualBasic {
  public static class VBTests {
    [Table("activity649")]
    public class Activity649 {
      [Column]
      [Identity]
      [PrimaryKey]
      public int activityid { get; set; }
      [Column]
      [NotNull]
      public int personid { get; set; }
      [Column]
      [NotNull]
      public DateTime added { get; set; }

      [Association(ThisKey = nameof(personid), OtherKey = nameof(Person649.personid), CanBeNull = false)]
      public Person649 Person { get; set; }
    }

    [Table("person649")]
    public class Person649 {
      [Column]
      [Identity]
      [PrimaryKey]
      public int personid { get; set; }
      [Column]
      [NotNull]
      public string personname { get; set; }
      [Association(ThisKey = nameof(personid), OtherKey = nameof(Activity649.personid), CanBeNull = false)]
      public List<Activity649> Activties { get; set; }
    }


    public static IEnumerable<object> Issue649Test1(IDataContext db) {
      return (from p in db.GetTable<Activity649>()
              where p.added >= new DateTime(2017, 1, 1)
              group p by new { p.Person.personid, p.Person.personname } into pp
              select new {
                PersonId = pp.Key.personid,
                PersonName = pp.Key.personname,
                LastAdded = pp.Max(f => f.added)
              }).ToList();
    }

    public static IEnumerable<object> Issue649Test2(IDataContext db) {
      return (from p in db.GetTable<Activity649>()
              where p.added >= new DateTime(2017, 1, 1)
              group p by new { p.Person.personid, p.Person.personname, } into pp
              let LastAdded = pp.Max(x => x.added)
              select new {
                PersonId = pp.Key.personid,
                PersonName = pp.Key.personname,
                LastAdded = LastAdded
              }).ToList();
    }

    public static IEnumerable<object> Issue649Test3(IDataContext db) {
      return db.GetTable<Activity649>().Where(f => f.added >= new DateTime(2017, 1, 1)).GroupBy(f => new {
        personid = f.Person.personid,
        personname = f.Person.personname
      }).Select(f => new {
        personid = f.Key.personid,
        personname = f.Key.personname,
        LastAdded = f.Max(g => g.added)
      }).ToList();
    }

    public static IEnumerable<GrandChild1> Issue2746Test(IDataContext db, string SelectedValue) {
      return db.GetTable<GrandChild1>().Where(w => w.ChildID.HasValue && w.ChildID.Value == System.Convert.ToInt32(SelectedValue)).ToList();
    }
  }

}
