using FluentAssertions;
using X10sions.System.NetStandard.Tests.Unit;
using Xunit;

namespace System;

public class StringExtensionsTests {
  [Theory][InlineData(null)] public void IsNull_ReturnsTrueGivenNullInput(string input) => input.IsNull().Should().BeTrue();

  [InlineData(Constants.ardalis)]
  [InlineData(Constants._empty)]
  [InlineData(Constants._space)]
  [Theory] public void IsNull_ReturnsFalseGivenAnyStringValue(string input) => input.IsNull().Should().BeFalse();




  [InlineData(Constants._null)]
  [InlineData(null)]
  [InlineData(Constants._empty)]
  [Theory] public void IsNullOrEmpty_ReturnsTrueGivenNullOrEmptyInput(string input) => input.IsNullOrEmpty().Should().BeTrue();


  [InlineData(Constants.ardalis)]
  [InlineData(Constants._space)]
  [Theory] public void IsNullOrEmpty_ReturnsFalseGivenAnyNonEmptyStringValue(string input) => input.IsNullOrEmpty().Should().BeFalse();


  [InlineData(null)]
  [InlineData(Constants._empty)]
  [InlineData(Constants._space)]
  [InlineData(Constants._newlineSpace)]
  [Theory]
  public void IsNullOrWhiteSpace_ReturnsTrueGivenNullOrEmptyOrWhiteSpaceInput(string input) => input.IsNullOrWhiteSpace().Should().BeTrue();


  [InlineData(Constants.ardalis)]
  [InlineData(Constants.x10sions)]
  [InlineData(Constants._dot)]
  [Theory] public void IsNullOrWhiteSpace_ReturnsFalseGivenAnyNonEmptyStringValue(string input) => input.IsNullOrWhiteSpace().Should().BeFalse();

}

