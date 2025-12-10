using Microsoft.EntityFrameworkCore;
using Xunit;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Tests.Query;

public sealed class SelectPaginationTests {
  private DbContextOptions<TestDbContext> Options() {
    var b = new DbContextOptionsBuilder<TestDbContext>();
    b.UseDB2iSeries("Driver={IBM i Access ODBC Driver};System=TESTSYS;Uid=U;Pwd=P;Naming=1;DefaultLibraries=TESTLIB")
     .EnableSensitiveDataLogging();
    return b.Options;
  }

  [Fact]
  public void Take_translates_to_fetch_first() {
    using var ctx = new TestDbContext(Options());
    var q = ctx.TestCustomers.OrderBy(c => c.Name).Take(10);
    var sql = q.ToQueryString();
    Assert.Contains("FETCH FIRST 10 ROWS ONLY", sql);
    Assert.DoesNotContain("OFFSET", sql);
  }

  [Fact]
  public void Skip_throws_not_supported() {
    using var ctx = new TestDbContext(Options());
    var q = ctx.TestCustomers.OrderBy(c => c.Name).Skip(5);
    Assert.ThrowsAny<System.NotSupportedException>(() => q.ToList());
  }

  [Fact]
  public void Skip_Take_translates_to_row_number() {
    using var ctx = new TestDbContext(Options());
    var q = ctx.TestCustomers.OrderBy(c => c.Name).Skip(5).Take(10);
    var sql = q.ToQueryString();
    Assert.Contains("ROW_NUMBER() OVER", sql);
    Assert.Contains("RN", sql);
    Assert.Contains("BETWEEN", sql); // depending on your predicate formatting
  }

}
