using Xunit;
using Microsoft.EntityFrameworkCore.Update;
using Moq;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using xMicrosoft.EntityFrameworkCore.DB2iSeries.Update;
using System.Security.Cryptography.X509Certificates;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Tests.Update;

public class DB2iSeriesUpdateSqlGeneratorTests {
  private readonly TestDB2iSeriesUpdateSqlGenerator _sqlGenerator;
  private readonly Mock<ISqlGenerationHelper> _sqlHelperMock;

  public DB2iSeriesUpdateSqlGeneratorTests() {
    _sqlHelperMock = new Mock<ISqlGenerationHelper>();
    _sqlHelperMock.Setup(x => x.StatementTerminator).Returns(";");
    _sqlHelperMock.Setup(x => x.DelimitIdentifier(It.IsAny<string>()))
        .Returns<string>(s => $"\"{s}\"");
    _sqlHelperMock.Setup(x => x.DelimitIdentifier(It.IsAny<string>(), It.IsAny<string>()))
        .Returns<string, string>((name, schema) =>
            string.IsNullOrEmpty(schema) ? $"\"{name}\"" : $"\"{schema}\".\"{name}\"");

    var dependencies = new UpdateSqlGeneratorDependencies(
        _sqlHelperMock.Object,
        Mock.Of<IRelationalTypeMappingSource>());

    _sqlGenerator = new TestDB2iSeriesUpdateSqlGenerator(dependencies);
  }

  [Fact]
  public void AppendSelectAffectedCountCommand_GeneratesCorrectSql() {
    // Arrange
    var builder = new StringBuilder();
    var commandStringBuilder = new StringBuilder();
    // Act
    var result = _sqlGenerator.TestAppendSelectAffectedCountCommand(commandStringBuilder, "TestTable", null, 0);
    // Assert
    Assert.Contains("SELECT ROW_COUNT()", commandStringBuilder.ToString());
    Assert.Contains("FROM SYSIBM.SYSDUMMY1", commandStringBuilder.ToString());
    Assert.Equal(ResultSetMapping.LastInResultSet, result);
  }
}

public class TestDB2iSeriesUpdateSqlGenerator : DB2iSeriesUpdateSqlGenerator {
  public TestDB2iSeriesUpdateSqlGenerator(UpdateSqlGeneratorDependencies dependencies) : base(dependencies) { }


  public ResultSetMapping TestAppendSelectAffectedCountCommand(StringBuilder commandStringBuilder, string name, string schema, int commandPosition) {
    return AppendSelectAffectedCountCommand(commandStringBuilder, name, schema, commandPosition);
  }

}

