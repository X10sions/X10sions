using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using xMicrosoft.EntityFrameworkCore.DB2iSeries.Storage;
using Xunit;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Tests.Storage;

public class DB2iSeriesSqlGenerationHelperTests {
  private readonly DB2iSeriesSqlGenerationHelper _sqlHelper;

  public DB2iSeriesSqlGenerationHelperTests() {
    var dependencies = new RelationalSqlGenerationHelperDependencies();
    _sqlHelper = new DB2iSeriesSqlGenerationHelper(dependencies);
  }

  [Theory]
  [InlineData("TableName", "\"TableName\"")]
  [InlineData("Column_Name", "\"Column_Name\"")]
  [InlineData("Test123", "\"Test123\"")]
  public void DelimitIdentifier_DelimitsCorrectly(string identifier, string expected) {
    // Act
    var result = _sqlHelper.DelimitIdentifier(identifier);

    // Assert
    Assert.Equal(expected, result);
  }

  [Fact]
  public void DelimitIdentifier_WithStringBuilder_AppendsCorrectly() {
    // Arrange
    var builder = new StringBuilder();
    var identifier = "TestTable";

    // Act
    _sqlHelper.DelimitIdentifier(builder, identifier);

    // Assert
    Assert.Equal("\"TestTable\"", builder.ToString());
  }

  [Theory]
  [InlineData("Test\"Name", "Test\"\"Name")]
  [InlineData("Normal", "Normal")]
  [InlineData("Multiple\"Quotes\"Here", "Multiple\"\"Quotes\"\"Here")]
  public void EscapeIdentifier_EscapesQuotesCorrectly(string identifier, string expected) {
    // Act
    var result = _sqlHelper.EscapeIdentifier(identifier);

    // Assert
    Assert.Equal(expected, result);
  }

  [Fact]
  public void EscapeIdentifier_WithStringBuilder_EscapesCorrectly() {
    // Arrange
    var builder = new StringBuilder();
    var identifier = "Test\"Name";

    // Act
    _sqlHelper.EscapeIdentifier(builder, identifier);

    // Assert
    Assert.Equal("Test\"\"Name", builder.ToString());
  }

  [Fact]
  public void StatementTerminator_ReturnsSemicolon() {
    // Assert
    Assert.Equal(";", _sqlHelper.StatementTerminator);
  }

}
