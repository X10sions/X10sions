using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Tests.Helpers;

public static class TestHelpers {
  public static DbContextOptions CreateOptions() => new DbContextOptionsBuilder().UseDB2iSeries("DataSource=test;UserID=test;Password=test").Options;

  public static DbContextOptions<T> CreateGenericOptions<T>() where T : DbContext {
    var builder = new DbContextOptionsBuilder<T>();
    builder.UseDB2iSeries("DataSource=test;UserID=test;Password=test");
    return builder.Options;
  }

  public static Mock<IModel> CreateMockModel() {
    var modelMock = new Mock<IModel>();
    return modelMock;
  }

  public static Mock<IRelationalTypeMappingSource> CreateMockTypeMappingSource() {
    var mappingSourceMock = new Mock<IRelationalTypeMappingSource>();
    // Setup common type mappings
    mappingSourceMock.Setup(x => x.FindMapping(It.IsAny<Type>()))
        .Returns<Type>(type => {
          if (type == typeof(string))
            return new StringTypeMapping("VARCHAR(255)", System.Data.DbType.String);
          if (type == typeof(int))
            return new IntTypeMapping("INTEGER");
          if (type == typeof(decimal))
            return new DecimalTypeMapping("DECIMAL(18,2)");

          return null;
        });

    return mappingSourceMock;
  }
}