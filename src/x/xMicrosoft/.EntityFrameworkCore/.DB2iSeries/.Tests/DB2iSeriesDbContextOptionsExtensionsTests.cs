using Microsoft.EntityFrameworkCore;
using Xunit;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Tests;

public class DB2iSeriesDbContextOptionsExtensionsTests {
  [Fact]
  public void UseISeries_ConfiguresOptions_Successfully() {
    // Arrange
    var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
    var connectionString = "DataSource=localhost;UserID=test;Password=test;DefaultCollection=testlib";
    // Act
    optionsBuilder.UseDB2iSeries(connectionString);
    // Assert
    Assert.NotNull(optionsBuilder.Options);
    var extension = optionsBuilder.Options.FindExtension<DB2iSeriesOptionsExtension>();
    Assert.NotNull(extension);
    Assert.Equal(connectionString, extension.ConnectionString);
  }

  [Fact]
  public void UseISeries_WithOptionsAction_ExecutesAction() {
    // Arrange
    var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
    var connectionString = "DataSource=localhost;UserID=test;Password=test";
    var actionExecuted = false;
    // Act
    optionsBuilder.UseDB2iSeries(connectionString, options => {
      actionExecuted = true;
    });
    // Assert
    Assert.True(actionExecuted);
  }

  [Fact]
  public void UseISeries_WithNullConnectionString_ThrowsException() {
    // Arrange
    var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
    // Act & Assert
    Assert.Throws<ArgumentNullException>(() => optionsBuilder.UseDB2iSeries(null!));
  }
}
