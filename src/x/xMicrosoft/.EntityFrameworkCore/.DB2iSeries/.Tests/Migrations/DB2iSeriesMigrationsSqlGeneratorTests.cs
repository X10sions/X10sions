using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Moq;
using System.Reflection.Emit;
using System.Text;
using xMicrosoft.EntityFrameworkCore.DB2iSeries.Migrations;
using Xunit;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Tests.Migrations;

public class DB2iSeriesMigrationsSqlGeneratorTests {
  private readonly TestDB2iSeriesMigrationsSqlGenerator _sqlGenerator;
  private readonly Mock<ISqlGenerationHelper> _sqlHelperMock;

  public DB2iSeriesMigrationsSqlGeneratorTests() {
    _sqlHelperMock = new Mock<ISqlGenerationHelper>();
    _sqlHelperMock.Setup(x => x.StatementTerminator).Returns(";");
    _sqlHelperMock.Setup(x => x.DelimitIdentifier(It.IsAny<string>()))
        .Returns<string>(s => $"\"{s}\"");
    _sqlHelperMock.Setup(x => x.DelimitIdentifier(It.IsAny<string>(), It.IsAny<string>()))
        .Returns<string, string>((name, schema) =>
            string.IsNullOrEmpty(schema) ? $"\"{name}\"" : $"\"{schema}\".\"{name}\"");

    var typeMappingSourceMock = new Mock<IRelationalTypeMappingSource>();
    var commandBuilderFactoryMock = new Mock<IRelationalCommandBuilderFactory>();
    var updateSqlGeneratorMock = new Mock<IUpdateSqlGenerator>();
    var currentDbContextMock = new Mock<ICurrentDbContext>();
    var modificationCommandFactoryMock = new Mock<IModificationCommandFactory>();
    var loggingOptionsMock = new Mock<ILoggingOptions>();
    var commandLoggerMock = new Mock<IRelationalCommandDiagnosticsLogger>();
    var migrationsLoggerMock = new Mock<IDiagnosticsLogger<DbLoggerCategory.Migrations>>();

    var dependencies = new MigrationsSqlGeneratorDependencies(
        commandBuilderFactoryMock.Object,
        updateSqlGeneratorMock.Object,
        _sqlHelperMock.Object,
        typeMappingSourceMock.Object,
        currentDbContextMock.Object,
        modificationCommandFactoryMock.Object,
        loggingOptionsMock.Object,
        commandLoggerMock.Object,
        migrationsLoggerMock.Object);

    _sqlGenerator = new TestDB2iSeriesMigrationsSqlGenerator(dependencies);
  }

  [Fact]
  public void Generate_DropTableOperation_GeneratesCorrectSql() {
    // Arrange
    var operation = new DropTableOperation {
      Name = "TestTable",
      Schema = null
    };

    var commandBuilderMock = new Mock<IRelationalCommandBuilder>();
    var commandStringBuilder = new StringBuilder();

    commandBuilderMock.Setup(x => x.Append(It.IsAny<string>()))
        .Returns<string>(s => {
          commandStringBuilder.Append(s);
          return commandBuilderMock.Object;
        });

    commandBuilderMock.Setup(x => x.AppendLine(It.IsAny<string>()))
        .Returns<string>(s => {
          commandStringBuilder.AppendLine(s);
          return commandBuilderMock.Object;
        });

    commandBuilderMock.Setup(x => x.Build())
        .Returns(() => Mock.Of<IRelationalCommand>());

    var commandBuilderFactoryMock = new Mock<IRelationalCommandBuilderFactory>();
    commandBuilderFactoryMock.Setup(x => x.Create())
        .Returns(commandBuilderMock.Object);

    var builder = new MigrationCommandListBuilder(
        new MigrationsSqlGeneratorDependencies(
            commandBuilderFactoryMock.Object,
            Mock.Of<IUpdateSqlGenerator>(),
            _sqlHelperMock.Object,
            Mock.Of<IRelationalTypeMappingSource>(),
            Mock.Of<ICurrentDbContext>(),
            Mock.Of<IModificationCommandFactory>(),
            Mock.Of<ILoggingOptions>(),
            Mock.Of<IRelationalCommandDiagnosticsLogger>(),
            Mock.Of<IDiagnosticsLogger<DbLoggerCategory.Migrations>>()));
    // Act
    _sqlGenerator.GenerateDropTableOperation(operation, null, builder);

    // Assert
    var sql = commandStringBuilder.ToString();
    Assert.Contains("DROP TABLE", sql);
    Assert.Contains("\"TestTable\"", sql);
  }

  [Fact]
  public void Generate_AddColumnOperation_GeneratesCorrectSql() {
    // Arrange
    var operation = new AddColumnOperation {
      Table = "TestTable",
      Name = "NewColumn",
      ClrType = typeof(string),
      ColumnType = "VARCHAR(100)",
      IsNullable = false
    };

    var commandBuilderMock = new Mock<IRelationalCommandBuilder>();
    var commandStringBuilder = new StringBuilder();

    commandBuilderMock.Setup(x => x.Append(It.IsAny<string>()))
        .Returns<string>(s => {
          commandStringBuilder.Append(s);
          return commandBuilderMock.Object;
        });

    commandBuilderMock.Setup(x => x.AppendLine(It.IsAny<string>()))
        .Returns<string>(s => {
          commandStringBuilder.AppendLine(s);
          return commandBuilderMock.Object;
        });

    commandBuilderMock.Setup(x => x.Build())
        .Returns(() => Mock.Of<IRelationalCommand>());

    var commandBuilderFactoryMock = new Mock<IRelationalCommandBuilderFactory>();
    commandBuilderFactoryMock.Setup(x => x.Create())
        .Returns(commandBuilderMock.Object);

    var builder = new MigrationCommandListBuilder(
        new MigrationsSqlGeneratorDependencies(
            commandBuilderFactoryMock.Object,
            Mock.Of<IUpdateSqlGenerator>(),
            _sqlHelperMock.Object,
            Mock.Of<IRelationalTypeMappingSource>(),
            Mock.Of<ICurrentDbContext>(),
            Mock.Of<IModificationCommandFactory>(),
            Mock.Of<ILoggingOptions>(),
            Mock.Of<IRelationalCommandDiagnosticsLogger>(),
            Mock.Of<IDiagnosticsLogger<DbLoggerCategory.Migrations>>()));

    // Act
    _sqlGenerator.GenerateAddColumnOperation(operation, null, builder);

    // Assert
    var sql = commandStringBuilder.ToString();
    Assert.Contains("ALTER TABLE", sql);
    Assert.Contains("ADD COLUMN", sql);
    Assert.Contains("\"NewColumn\"", sql);
    Assert.Contains("VARCHAR(100)", sql);
    Assert.Contains("NOT NULL", sql);
  }

}

public class TestDB2iSeriesMigrationsSqlGenerator : DB2iSeriesMigrationsSqlGenerator {
  public TestDB2iSeriesMigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies) : base(dependencies) { }

  public void GenerateDropTableOperation(DropTableOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true) {
    Generate(operation, model, builder, terminate);
  }

  public void GenerateAddColumnOperation(AddColumnOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true) {
    Generate(operation, model, builder, terminate);
  }

}