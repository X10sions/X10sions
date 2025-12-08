using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using xMicrosoft.EntityFrameworkCore.DB2iSeries.Storage;
using Xunit;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Tests.Storage;

public class DB2iSeriesTypeMappingSourceTests {
  private readonly DB2iSeriesTypeMappingSource _typeMappingSource;

  public DB2iSeriesTypeMappingSourceTests() {

    var valueConverterSelectorDependencies = new ValueConverterSelectorDependencies();
    var valueConverterSelector = new ValueConverterSelector(valueConverterSelectorDependencies);
    var jsonReaderWriterSourceDependencies = new JsonValueReaderWriterSourceDependencies();
    IJsonValueReaderWriterSource jsonValueReaderWriterSource = new JsonValueReaderWriterSource(jsonReaderWriterSourceDependencies);
    var dependencies = new TypeMappingSourceDependencies(valueConverterSelector, jsonValueReaderWriterSource, Array.Empty<ITypeMappingSourcePlugin>());
    var relationalDependencies = new RelationalTypeMappingSourceDependencies(Array.Empty<IRelationalTypeMappingSourcePlugin>());
    _typeMappingSource = new DB2iSeriesTypeMappingSource(dependencies, relationalDependencies);
  }

  [Theory]
  [InlineData(typeof(int), "INTEGER")]
  [InlineData(typeof(long), "BIGINT")]
  [InlineData(typeof(short), "SMALLINT")]
  [InlineData(typeof(decimal), "DECIMAL(18,2)")]
  [InlineData(typeof(double), "DOUBLE")]
  [InlineData(typeof(float), "REAL")]
  [InlineData(typeof(bool), "SMALLINT")]
  [InlineData(typeof(DateTime), "TIMESTAMP")]
  public void FindMapping_ForClrType_ReturnsCorrectStoreType(Type clrType, string expectedStoreType) {
    // Arrange & Act
    var mapping = _typeMappingSource.FindMapping(clrType);
    // Assert
    Assert.NotNull(mapping);
    Assert.Equal(expectedStoreType, mapping.StoreType);
  }

  [Fact]
  public void FindMapping_ForString_ReturnsVarchar() {
    // Arrange & Act
    var mapping = _typeMappingSource.FindMapping(typeof(string));
    // Assert
    Assert.NotNull(mapping);
    Assert.StartsWith("VARCHAR", mapping.StoreType);
  }

  [Fact]
  public void FindMapping_ForStringWithSize_ReturnsVarcharWithSize() {
    // Arrange & Act
    var mapping = _typeMappingSource.FindMapping(typeof(string), null, size: 100);
    // Assert
    Assert.NotNull(mapping);
    Assert.Equal("VARCHAR(100)", mapping.StoreType);
  }

  [Fact]
  public void FindMapping_ForFixedLengthString_ReturnsChar() {
    // Arrange & Act
    var mapping = _typeMappingSource.FindMapping(typeof(string), null, size: 10, fixedLength: true);
    // Assert
    Assert.NotNull(mapping);
    Assert.Equal("CHAR(10)", mapping.StoreType);
  }

  [Fact]
  public void FindMapping_ForByteArray_ReturnsBlob() {
    // Arrange & Act
    var mapping = _typeMappingSource.FindMapping(typeof(byte[]));
    // Assert
    Assert.NotNull(mapping);
    Assert.Equal("BLOB", mapping.StoreType);
  }
}