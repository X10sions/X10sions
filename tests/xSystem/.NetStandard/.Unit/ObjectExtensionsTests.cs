using FluentAssertions;
using X10sions.System.NetStandard.Tests.Unit;
using Xunit;

namespace System {
  public class ObjectExtensionsTests {

    [InlineData(null)]
    [Theory] public void IsNull_ReturnsTrueGivenNull(object value) => value.IsNull().Should().BeTrue();

    [InlineData(Constants.text)]
    [InlineData(45)]
    [InlineData(true)]
    [Theory] public void IsNull_ReturnsFalseGivenValue(object value) => value.IsNull().Should().BeTrue();

  }
}
