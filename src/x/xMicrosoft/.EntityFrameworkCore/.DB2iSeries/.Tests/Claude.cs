//// File: EntityFrameworkCore.ISeries.Tests.csproj
///*
//<Project Sdk="Microsoft.NET.Sdk">

//  <PropertyGroup>
//    <TargetFramework>net9.0</TargetFramework>
//    <ImplicitUsings>enable</ImplicitUsings>
//    <Nullable>enable</Nullable>
//    <IsPackable>false</IsPackable>
//  </PropertyGroup>

//  <ItemGroup>
//    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.0" />
//    <PackageReference Include="xUnit" Version="2.9.0" />
//    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2">
//      <PrivateAssets>all</PrivateAssets>
//      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
//    </PackageReference>
//    <PackageReference Include="Moq" Version="4.20.70" />
//    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.0" />
//    <PackageReference Include="Microsoft.EntityFrameworkCore.Relational" Version="10.0.0" />
//    <PackageReference Include="IBM.Data.DB2.Core" Version="3.1.0.400" />
//  </ItemGroup>

//  <ItemGroup>
//    <ProjectReference Include="..\EntityFrameworkCore.ISeries\EntityFrameworkCore.ISeries.csproj" />
//  </ItemGroup>

//</Project>
//*/

//// File: ISeriesDbContextOptionsExtensionsTests.cs
//using EntityFrameworkCore.ISeries;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata;
//using Microsoft.EntityFrameworkCore.Migrations;
//using Microsoft.EntityFrameworkCore.Migrations.Operations;
//using Microsoft.EntityFrameworkCore.Storage;
//using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
//using Microsoft.EntityFrameworkCore.Update;
//using Moq;
//using System.Text;
//using Xunit;

//namespace EntityFrameworkCore.ISeries.Tests {
//  public class ISeriesDbContextOptionsExtensionsTests {
//    [Fact]
//    public void UseISeries_ConfiguresOptions_Successfully() {
//      // Arrange
//      var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
//      var connectionString = "DataSource=localhost;UserID=test;Password=test;DefaultCollection=testlib";

//      // Act
//      optionsBuilder.UseISeries(connectionString);

//      // Assert
//      Assert.NotNull(optionsBuilder.Options);
//      var extension = optionsBuilder.Options.FindExtension<ISeriesOptionsExtension>();
//      Assert.NotNull(extension);
//      Assert.Equal(connectionString, extension.ConnectionString);
//    }

//    [Fact]
//    public void UseISeries_WithOptionsAction_ExecutesAction() {
//      // Arrange
//      var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
//      var connectionString = "DataSource=localhost;UserID=test;Password=test";
//      var actionExecuted = false;

//      // Act
//      optionsBuilder.UseISeries(connectionString, options =>
//      {
//        actionExecuted = true;
//      });

//      // Assert
//      Assert.True(actionExecuted);
//    }

//    [Fact]
//    public void UseISeries_WithNullConnectionString_ThrowsException() {
//      // Arrange
//      var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();

//      // Act & Assert
//      Assert.Throws<ArgumentNullException>(() =>
//          optionsBuilder.UseISeries(null!));
//    }
//  }

//  public class TestDbContext : DbContext {
//    public TestDbContext() { }
//    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

//    public DbSet<TestEntity> TestEntities { get; set; }
//  }

//  public class TestEntity {
//    public int Id { get; set; }
//    public string Name { get; set; } = string.Empty;
//    public decimal Price { get; set; }
//    public DateTime CreatedDate { get; set; }
//  }
//}

//// File: ISeriesTypeMappingSourceTests.cs
//using Xunit;
//using Microsoft.EntityFrameworkCore.Storage;
//using EntityFrameworkCore.ISeries.Storage;
//using Moq;

//namespace EntityFrameworkCore.ISeries.Tests.Storage {
//  public class ISeriesTypeMappingSourceTests {
//    private readonly ISeriesTypeMappingSource _typeMappingSource;

//    public ISeriesTypeMappingSourceTests() {
//      var dependencies = new TypeMappingSourceDependencies(
//          new ValueConverterSelector(new ValueConverterSelectorDependencies()),
//          Array.Empty<ITypeMappingSourcePlugin>());

//      var relationalDependencies = new RelationalTypeMappingSourceDependencies(
//          Array.Empty<IRelationalTypeMappingSourcePlugin>());

//      _typeMappingSource = new ISeriesTypeMappingSource(dependencies, relationalDependencies);
//    }

//    [Theory]
//    [InlineData(typeof(int), "INTEGER")]
//    [InlineData(typeof(long), "BIGINT")]
//    [InlineData(typeof(short), "SMALLINT")]
//    [InlineData(typeof(decimal), "DECIMAL(18,2)")]
//    [InlineData(typeof(double), "DOUBLE")]
//    [InlineData(typeof(float), "REAL")]
//    [InlineData(typeof(bool), "SMALLINT")]
//    [InlineData(typeof(DateTime), "TIMESTAMP")]
//    public void FindMapping_ForClrType_ReturnsCorrectStoreType(Type clrType, string expectedStoreType) {
//      // Arrange & Act
//      var mapping = _typeMappingSource.FindMapping(clrType);

//      // Assert
//      Assert.NotNull(mapping);
//      Assert.Equal(expectedStoreType, mapping.StoreType);
//    }

//    [Fact]
//    public void FindMapping_ForString_ReturnsVarchar() {
//      // Arrange & Act
//      var mapping = _typeMappingSource.FindMapping(typeof(string));

//      // Assert
//      Assert.NotNull(mapping);
//      Assert.StartsWith("VARCHAR", mapping.StoreType);
//    }

//    [Fact]
//    public void FindMapping_ForStringWithSize_ReturnsVarcharWithSize() {
//      // Arrange & Act
//      var mapping = _typeMappingSource.FindMapping(typeof(string), null, null, false, 100);

//      // Assert
//      Assert.NotNull(mapping);
//      Assert.Equal("VARCHAR(100)", mapping.StoreType);
//    }

//    [Fact]
//    public void FindMapping_ForFixedLengthString_ReturnsChar() {
//      // Arrange & Act
//      var mapping = _typeMappingSource.FindMapping(typeof(string), null, null, true, 10);

//      // Assert
//      Assert.NotNull(mapping);
//      Assert.Equal("CHAR(10)", mapping.StoreType);
//    }

//    [Fact]
//    public void FindMapping_ForByteArray_ReturnsBlob() {
//      // Arrange & Act
//      var mapping = _typeMappingSource.FindMapping(typeof(byte[]));

//      // Assert
//      Assert.NotNull(mapping);
//      Assert.Equal("BLOB", mapping.StoreType);
//    }

//    [Theory]
//    [InlineData("INTEGER", typeof(int))]
//    [InlineData("VARCHAR(100)", typeof(string))]
//    [InlineData("DECIMAL(10,2)", typeof(decimal))]
//    public void FindMapping_ByStoreTypeName_ReturnsCorrectMapping(string storeTypeName, Type expectedClrType) {
//      // Arrange & Act
//      var mapping = _typeMappingSource.FindMapping(storeTypeName);

//      // Assert
//      Assert.NotNull(mapping);
//      Assert.True(mapping.ClrType == expectedClrType ||
//                 (expectedClrType == typeof(string) && mapping.ClrType == typeof(string)));
//    }
//  }
//}

//// File: ISeriesSqlGenerationHelperTests.cs
//using Xunit;
//using Microsoft.EntityFrameworkCore.Storage;
//using EntityFrameworkCore.ISeries.Storage;
//using System.Text;

//namespace EntityFrameworkCore.ISeries.Tests.Storage {
//  public class ISeriesSqlGenerationHelperTests {
//    private readonly ISeriesSqlGenerationHelper _sqlHelper;

//    public ISeriesSqlGenerationHelperTests() {
//      var dependencies = new RelationalSqlGenerationHelperDependencies();
//      _sqlHelper = new ISeriesSqlGenerationHelper(dependencies);
//    }

//    [Theory]
//    [InlineData("TableName", "\"TableName\"")]
//    [InlineData("Column_Name", "\"Column_Name\"")]
//    [InlineData("Test123", "\"Test123\"")]
//    public void DelimitIdentifier_DelimitsCorrectly(string identifier, string expected) {
//      // Act
//      var result = _sqlHelper.DelimitIdentifier(identifier);

//      // Assert
//      Assert.Equal(expected, result);
//    }

//    [Fact]
//    public void DelimitIdentifier_WithStringBuilder_AppendsCorrectly() {
//      // Arrange
//      var builder = new StringBuilder();
//      var identifier = "TestTable";

//      // Act
//      _sqlHelper.DelimitIdentifier(builder, identifier);

//      // Assert
//      Assert.Equal("\"TestTable\"", builder.ToString());
//    }

//    [Theory]
//    [InlineData("Test\"Name", "Test\"\"Name")]
//    [InlineData("Normal", "Normal")]
//    [InlineData("Multiple\"Quotes\"Here", "Multiple\"\"Quotes\"\"Here")]
//    public void EscapeIdentifier_EscapesQuotesCorrectly(string identifier, string expected) {
//      // Act
//      var result = _sqlHelper.EscapeIdentifier(identifier);

//      // Assert
//      Assert.Equal(expected, result);
//    }

//    [Fact]
//    public void EscapeIdentifier_WithStringBuilder_EscapesCorrectly() {
//      // Arrange
//      var builder = new StringBuilder();
//      var identifier = "Test\"Name";

//      // Act
//      _sqlHelper.EscapeIdentifier(builder, identifier);

//      // Assert
//      Assert.Equal("Test\"\"Name", builder.ToString());
//    }

//    [Fact]
//    public void StatementTerminator_ReturnsSemicolon() {
//      // Assert
//      Assert.Equal(";", _sqlHelper.StatementTerminator);
//    }
//  }
//}

//// File: ISeriesUpdateSqlGeneratorTests.cs
//using Xunit;
//using Microsoft.EntityFrameworkCore.Update;
//using EntityFrameworkCore.ISeries.Update;
//using Moq;
//using System.Text;
//using Microsoft.EntityFrameworkCore.Storage;

//namespace EntityFrameworkCore.ISeries.Tests.Update {
//  public class ISeriesUpdateSqlGeneratorTests {
//    private readonly ISeriesUpdateSqlGenerator _sqlGenerator;
//    private readonly Mock<ISqlGenerationHelper> _sqlHelperMock;

//    public ISeriesUpdateSqlGeneratorTests() {
//      _sqlHelperMock = new Mock<ISqlGenerationHelper>();
//      _sqlHelperMock.Setup(x => x.StatementTerminator).Returns(";");
//      _sqlHelperMock.Setup(x => x.DelimitIdentifier(It.IsAny<string>()))
//          .Returns<string>(s => $"\"{s}\"");
//      _sqlHelperMock.Setup(x => x.DelimitIdentifier(It.IsAny<string>(), It.IsAny<string>()))
//          .Returns<string, string>((name, schema) =>
//              string.IsNullOrEmpty(schema) ? $"\"{name}\"" : $"\"{schema}\".\"{name}\"");

//      var dependencies = new UpdateSqlGeneratorDependencies(
//          _sqlHelperMock.Object,
//          Mock.Of<IRelationalTypeMappingSource>());

//      _sqlGenerator = new ISeriesUpdateSqlGenerator(dependencies);
//    }

//    [Fact]
//    public void AppendSelectAffectedCountCommand_GeneratesCorrectSql() {
//      // Arrange
//      var builder = new StringBuilder();
//      var commandStringBuilder = new StringBuilder();

//      // Act
//      var result = _sqlGenerator.AppendSelectAffectedCountCommand(
//          commandStringBuilder,
//          "TestTable",
//          null,
//          0);

//      // Assert
//      Assert.Contains("SELECT ROW_COUNT()", commandStringBuilder.ToString());
//      Assert.Contains("FROM SYSIBM.SYSDUMMY1", commandStringBuilder.ToString());
//      Assert.Equal(ResultSetMapping.LastInResultSet, result);
//    }
//  }
//}

//// File: ISeriesMigrationsSqlGeneratorTests.cs
//using Xunit;
//using Microsoft.EntityFrameworkCore.Migrations;
//using Microsoft.EntityFrameworkCore.Migrations.Operations;
//using Microsoft.EntityFrameworkCore.Storage;
//using EntityFrameworkCore.ISeries.Migrations;
//using Moq;

//namespace EntityFrameworkCore.ISeries.Tests.Migrations {
//  public class ISeriesMigrationsSqlGeneratorTests {
//    private readonly ISeriesMigrationsSqlGenerator _sqlGenerator;
//    private readonly Mock<ISqlGenerationHelper> _sqlHelperMock;

//    public ISeriesMigrationsSqlGeneratorTests() {
//      _sqlHelperMock = new Mock<ISqlGenerationHelper>();
//      _sqlHelperMock.Setup(x => x.StatementTerminator).Returns(";");
//      _sqlHelperMock.Setup(x => x.DelimitIdentifier(It.IsAny<string>()))
//          .Returns<string>(s => $"\"{s}\"");
//      _sqlHelperMock.Setup(x => x.DelimitIdentifier(It.IsAny<string>(), It.IsAny<string>()))
//          .Returns<string, string>((name, schema) =>
//              string.IsNullOrEmpty(schema) ? $"\"{name}\"" : $"\"{schema}\".\"{name}\"");

//      var typeMappingSourceMock = new Mock<IRelationalTypeMappingSource>();

//      var dependencies = new MigrationsSqlGeneratorDependencies(
//          Mock.Of<IRelationalCommandBuilderFactory>(),
//          _sqlHelperMock.Object,
//          typeMappingSourceMock.Object);

//      var commandBatchPreparer = Mock.Of<ICommandBatchPreparer>();

//      _sqlGenerator = new ISeriesMigrationsSqlGenerator(dependencies, commandBatchPreparer);
//    }

//    [Fact]
//    public void Generate_DropTableOperation_GeneratesCorrectSql() {
//      // Arrange
//      var operation = new DropTableOperation {
//        Name = "TestTable",
//        Schema = null
//      };
//      var builder = new MigrationCommandListBuilder(
//          Mock.Of<IRelationalCommandBuilderFactory>());

//      // Act
//      _sqlGenerator.Generate(operation, null, builder);
//      var commands = builder.GetCommandList();

//      // Assert
//      Assert.NotEmpty(commands);
//      var sql = commands[0].CommandText;
//      Assert.Contains("DROP TABLE", sql);
//      Assert.Contains("\"TestTable\"", sql);
//    }

//    [Fact]
//    public void Generate_AddColumnOperation_GeneratesCorrectSql() {
//      // Arrange
//      var operation = new AddColumnOperation {
//        Table = "TestTable",
//        Name = "NewColumn",
//        ClrType = typeof(string),
//        ColumnType = "VARCHAR(100)",
//        IsNullable = false
//      };
//      var builder = new MigrationCommandListBuilder(
//          Mock.Of<IRelationalCommandBuilderFactory>());

//      // Act
//      _sqlGenerator.Generate(operation, null, builder);
//      var commands = builder.GetCommandList();

//      // Assert
//      Assert.NotEmpty(commands);
//      var sql = commands[0].CommandText;
//      Assert.Contains("ALTER TABLE", sql);
//      Assert.Contains("ADD COLUMN", sql);
//      Assert.Contains("\"NewColumn\"", sql);
//      Assert.Contains("VARCHAR(100)", sql);
//      Assert.Contains("NOT NULL", sql);
//    }
//  }
//}

//// File: IntegrationTests.cs
//using Xunit;
//using Microsoft.EntityFrameworkCore;
//using EntityFrameworkCore.ISeries;

//namespace EntityFrameworkCore.ISeries.Tests.Integration {
//  // Note: These tests require an actual iSeries connection
//  // Mark with [Fact(Skip = "Requires iSeries connection")] if not available
//  public class IntegrationTests : IDisposable {
//    private readonly TestDbContext _context;
//    private const string ConnectionString =
//        "DataSource=localhost;UserID=testuser;Password=testpass;DefaultCollection=TESTLIB";

//    public IntegrationTests() {
//      var options = new DbContextOptionsBuilder<TestDbContext>()
//          .UseISeries(ConnectionString)
//          .Options;

//      _context = new TestDbContext(options);
//    }

//    [Fact(Skip = "Requires iSeries connection")]
//    public void CanConnect_ToDatabase() {
//      // Act & Assert
//      Assert.True(_context.Database.CanConnect());
//    }

//    [Fact(Skip = "Requires iSeries connection")]
//    public async Task CanInsertAndQuery_Entity() {
//      // Arrange
//      var entity = new TestEntity {
//        Name = "Test Product",
//        Price = 99.99m,
//        CreatedDate = DateTime.Now
//      };

//      // Act
//      _context.TestEntities.Add(entity);
//      await _context.SaveChangesAsync();

//      var retrieved = await _context.TestEntities
//          .FirstOrDefaultAsync(e => e.Name == "Test Product");

//      // Assert
//      Assert.NotNull(retrieved);
//      Assert.Equal("Test Product", retrieved.Name);
//      Assert.Equal(99.99m, retrieved.Price);
//    }

//    [Fact(Skip = "Requires iSeries connection")]
//    public async Task CanUpdate_Entity() {
//      // Arrange
//      var entity = new TestEntity {
//        Name = "Original Name",
//        Price = 50.00m,
//        CreatedDate = DateTime.Now
//      };
//      _context.TestEntities.Add(entity);
//      await _context.SaveChangesAsync();

//      // Act
//      entity.Name = "Updated Name";
//      entity.Price = 75.00m;
//      await _context.SaveChangesAsync();

//      var retrieved = await _context.TestEntities.FindAsync(entity.Id);

//      // Assert
//      Assert.NotNull(retrieved);
//      Assert.Equal("Updated Name", retrieved.Name);
//      Assert.Equal(75.00m, retrieved.Price);
//    }

//    [Fact(Skip = "Requires iSeries connection")]
//    public async Task CanDelete_Entity() {
//      // Arrange
//      var entity = new TestEntity {
//        Name = "To Delete",
//        Price = 10.00m,
//        CreatedDate = DateTime.Now
//      };
//      _context.TestEntities.Add(entity);
//      await _context.SaveChangesAsync();
//      var id = entity.Id;

//      // Act
//      _context.TestEntities.Remove(entity);
//      await _context.SaveChangesAsync();

//      var retrieved = await _context.TestEntities.FindAsync(id);

//      // Assert
//      Assert.Null(retrieved);
//    }

//    [Fact(Skip = "Requires iSeries connection")]
//    public async Task CanExecute_ComplexQuery() {
//      // Arrange
//      var entities = new[]
//      {
//                new TestEntity { Name = "Product A", Price = 10.00m, CreatedDate = DateTime.Now },
//                new TestEntity { Name = "Product B", Price = 20.00m, CreatedDate = DateTime.Now },
//                new TestEntity { Name = "Product C", Price = 30.00m, CreatedDate = DateTime.Now }
//            };
//      _context.TestEntities.AddRange(entities);
//      await _context.SaveChangesAsync();

//      // Act
//      var results = await _context.TestEntities
//          .Where(e => e.Price > 15.00m)
//          .OrderByDescending(e => e.Price)
//          .Take(2)
//          .ToListAsync();

//      // Assert
//      Assert.Equal(2, results.Count);
//      Assert.Equal("Product C", results[0].Name);
//      Assert.Equal("Product B", results[1].Name);
//    }

//    public void Dispose() {
//      _context?.Dispose();
//    }
//  }
//}

//// File: TestHelpers.cs
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.EntityFrameworkCore.Metadata;
//using Moq;

//namespace EntityFrameworkCore.ISeries.Tests.Helpers {
//  public static class TestHelpers {
//    public static DbContextOptions<T> CreateOptions<T>() where T : DbContext {
//      return new DbContextOptionsBuilder<T>()
//          .UseISeries("DataSource=test;UserID=test;Password=test")
//          .Options;
//    }

//    public static Mock<IModel> CreateMockModel() {
//      var modelMock = new Mock<IModel>();
//      return modelMock;
//    }

//    public static Mock<IRelationalTypeMappingSource> CreateMockTypeMappingSource() {
//      var mappingSourceMock = new Mock<IRelationalTypeMappingSource>();

//      // Setup common type mappings
//      mappingSourceMock
//          .Setup(x => x.FindMapping(It.IsAny<Type>()))
//          .Returns<Type>(type => {
//            if (type == typeof(string))
//              return new StringTypeMapping("VARCHAR(255)", System.Data.DbType.String);
//            if (type == typeof(int))
//              return new IntTypeMapping("INTEGER");
//            if (type == typeof(decimal))
//              return new DecimalTypeMapping("DECIMAL(18,2)");

//            return null;
//          });

//      return mappingSourceMock;
//    }
//  }
//}

//// File: README.md
///*
//# EntityFrameworkCore.ISeries.Tests

//## Running Tests

//### Unit Tests
//Unit tests can be run without an iSeries connection:

//```bash
//dotnet test --filter "FullyQualifiedName!~Integration"
//```

//### Integration Tests
//Integration tests require an actual iSeries v5r4 connection.

//1. Update the connection string in `IntegrationTests.cs`
//2. Remove the `Skip` attribute from test methods
//3. Run:

//```bash
//dotnet test --filter "FullyQualifiedName~Integration"
//```

//## Test Coverage

//- **DbContext Options**: Tests for UseISeries extension methods
//- **Type Mappings**: Tests for CLR to iSeries type conversions
//- **SQL Generation**: Tests for identifier delimiting and SQL syntax
//- **Migrations**: Tests for DDL statement generation
//- **Updates**: Tests for INSERT/UPDATE/DELETE operations
//- **Integration**: End-to-end tests with real database

//## Mock Setup

//The tests use Moq for mocking EF Core dependencies. Key mocked components:
//- ISqlGenerationHelper
//- IRelationalTypeMappingSource
//- IModel

//## CI/CD Considerations

//For CI/CD pipelines, integration tests should be skipped or run against a test iSeries instance:

//```yaml
//# Example GitHub Actions
//- name: Run Unit Tests
//  run: dotnet test --filter "FullyQualifiedName!~Integration"
//```
//*/