//using LinqToDB.Tests;
//using System;
//using Xunit;

//namespace LinqToDB.Linq {
//  public class IModificationHandlerTests : IClassFixture<zDataConnectionFixture> {
//    public IModificationHandlerTests(zDataConnectionFixture dataConnectionFixture) {
//      dataConnection = dataConnectionFixture;
//    }
//    zDataConnectionFixture dataConnection;

//    public class TestDataItem {
//      public TestDataItem(int? id, int personId, string name, bool doDelete, SqlAction expectedSqlAction) {
//        Id = id;
//        PersonId = personId;
//        Name = name;
//        DoDelete = doDelete;
//        ExpectedSqlAction = expectedSqlAction;
//      }
//      public int? Id { get; set; }
//      public int PersonId { get; set; }
//      public string Name { get; set; }
//      public bool DoDelete { get; set; }
//      public SqlAction ExpectedSqlAction { get; set; }
//      //public string ExpectedString => ExpectedSqlAction.ToString();

//    }

//    public enum SqlAction {
//      Insert,
//      Update,
//      Delete
//    }

//    public static TheoryData<TestDataItem> TestDataList => new TheoryData<TestDataItem> {
//        new TestDataItem(null,23,"TestName",false,SqlAction.Insert) ,
//        new TestDataItem(0,23,"TestName",false, SqlAction.Insert) ,
//        new TestDataItem(1,23,"TestName",false, SqlAction.Update) ,
//        new TestDataItem(1,23,"TestName",false, SqlAction.Delete)
//    };

//    //public bool IsSqlActionDelete(TestDataItem data) => data.DoDelete ;
//    //public bool IsSqlActionInsert(TestDataItem data) => !data.DoDelete && data?.Id > 0;
//    //public bool IsSqlActionUpdate(TestDataItem data) => !data.DoDelete && !(data?.Id > 0);

//    //[Theory, MemberData(nameof(TestDataList), MemberType = typeof(IModificationHandlerTests))] public void AllPersons_IsSqlActionDelete_WithTheoryData_FromDataGenerator(TestDataItem x) => Assert.True(IsSqlActionDelete(x));
//    //[Theory, MemberData(nameof(TestDataList), MemberType = typeof(IModificationHandlerTests))] public void AllPersons_IsSqlActionInsert_WithTheoryData_FromDataGenerator(TestDataItem x) => Assert.True(IsSqlActionInsert(x));
//    //[Theory, MemberData(nameof(TestDataList), MemberType = typeof(IModificationHandlerTests))] public void AllPersons_IsSqlActionUpdate_WithTheoryData_FromDataGenerator(TestDataItem x) => Assert.True(IsSqlActionUpdate(x));

//    [Theory, MemberData(nameof(TestDataList), MemberType = typeof(IModificationHandlerTests))]
//    public void TestGetModificationHandler(TestDataItem data) {
//      var mh = dataConnection.TestTable.GetModificationHandler(x => x.Id == data.Id, x => x.Name, data.Name);
//      if (data.DoDelete) {
//        // Delete
//        var i = mh.Delete();
//        // below could also be used:
//        //    mh.Deletable.Delete();

//        Assert.Equal(SqlAction.Delete, data.ExpectedSqlAction);

//      } else {
//        mh.SetValue(x => x.Name, data.Name)
//          .SetValue(x => x.LastModifiedById, data.PersonId);
//        if (data?.Id > 0) {
//          // Update
//          mh.SetValue(x => x.ModifyCount, x => x.ModifyCount + 1)
//            .SetValue(x => x.LastModified, DateTime.Now)
//            .Update();
//          // below could also be used:
//          //   mh.Updatable.Update();
//          Assert.Equal(SqlAction.Update, data.ExpectedSqlAction);
//        } else {
//          // Insert
//          mh.SetValue(x => x.CreatedById, data.PersonId)
//            .Insert();
//          // below could also be used:
//          //   id = mh.Insertable.InsertWithInt32Identity();
//          Assert.Equal(SqlAction.Insert, data.ExpectedSqlAction);
//        }
//      }



//    }

//  }

//}
