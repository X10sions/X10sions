using Microsoft.EntityFrameworkCore;
using Xunit;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Tests.Integration;

// Note: These tests require an actual DB2iSeries connection
// Mark with [Fact(Skip = "Requires DB2iSeries connection")] if not available
public class IntegrationTests : IDisposable {

  private readonly TestDbContext _context;
  private const string ConnectionString = "DataSource=localhost;UserID=testuser;Password=testpass;DefaultCollection=TESTLIB";

  public IntegrationTests() {
    var options = new DbContextOptionsBuilder<TestDbContext>().UseDB2iSeries(ConnectionString).Options;
    _context = new TestDbContext(options);
  }

  [Fact(Skip = "Requires DB2iSeries connection")]
  public void CanConnect_ToDatabase() {
    // Act & Assert
    Assert.True(_context.Database.CanConnect());
  }

  [Fact(Skip = "Requires DB2iSeries connection")]
  public async Task CanInsertAndQuery_Entity() {
    // Arrange
    var entity = new TestEntity {
      Name = "Test Product",
      Price = 99.99m,
      CreatedDate = DateTime.Now
    };
    // Act
    _context.TestEntities.Add(entity);
    await _context.SaveChangesAsync();
    var retrieved = await _context.TestEntities.FirstOrDefaultAsync(e => e.Name == "Test Product");
    // Assert
    Assert.NotNull(retrieved);
    Assert.Equal("Test Product", retrieved.Name);
    Assert.Equal(99.99m, retrieved.Price);
  }

  [Fact(Skip = "Requires DB2iSeries connection")]
  public async Task CanUpdate_Entity() {
    // Arrange
    var entity = new TestEntity {
      Name = "Original Name",
      Price = 50.00m,
      CreatedDate = DateTime.Now
    };
    _context.TestEntities.Add(entity);
    await _context.SaveChangesAsync();
    // Act
    entity.Name = "Updated Name";
    entity.Price = 75.00m;
    await _context.SaveChangesAsync();
    var retrieved = await _context.TestEntities.FindAsync(entity.Id);
    // Assert
    Assert.NotNull(retrieved);
    Assert.Equal("Updated Name", retrieved.Name);
    Assert.Equal(75.00m, retrieved.Price);
  }

  [Fact(Skip = "Requires DB2iSeries connection")]
  public async Task CanDelete_Entity() {
    // Arrange
    var entity = new TestEntity {
      Name = "To Delete",
      Price = 10.00m,
      CreatedDate = DateTime.Now
    };
    _context.TestEntities.Add(entity);
    await _context.SaveChangesAsync();
    var id = entity.Id;

    // Act
    _context.TestEntities.Remove(entity);
    await _context.SaveChangesAsync();

    var retrieved = await _context.TestEntities.FindAsync(id);

    // Assert
    Assert.Null(retrieved);
  }

  [Fact(Skip = "Requires DB2iSeries connection")]
  public async Task CanExecute_ComplexQuery() {
    // Arrange
    var entities = new[] {
      new TestEntity { Name = "Product A", Price = 10.00m, CreatedDate = DateTime.Now },
      new TestEntity { Name = "Product B", Price = 20.00m, CreatedDate = DateTime.Now },
      new TestEntity { Name = "Product C", Price = 30.00m, CreatedDate = DateTime.Now }
    };
    _context.TestEntities.AddRange(entities);
    await _context.SaveChangesAsync();
    // Act
    var results = await _context.TestEntities
        .Where(e => e.Price > 15.00m)
        .OrderByDescending(e => e.Price)
        .Take(2)
        .ToListAsync();

    // Assert
    Assert.Equal(2, results.Count);
    Assert.Equal("Product C", results[0].Name);
    Assert.Equal("Product B", results[1].Name);
  }

  public void Dispose() {
    _context?.Dispose();
  }
}