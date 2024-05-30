using X10sions.System.NetStandard.Tests.Unit;
using Xunit;

namespace System.Threading.Tasks;
public class TaskExtensionsTests {

  [Fact]
  public async Task WhenAll_TakesTupleOfTwoTasksAndReturnsTupleOfTwoResult() {
    var tasks = (GetIntAsync(1), GetStringAsync(Constants.Two));
    var result = await tasks.WhenAll();
    Assert.Equal((1, Constants.Two), result);
  }

  [Fact]
  public async Task WhenAll_TakesTupleOfThreeTasksAndReturnsTupleOfThreeResult() {
    var tasks = (GetIntAsync(1), GetStringAsync(Constants.Two), GetDateTimeAsync(3));
    var result = await tasks.WhenAll();
    Assert.Equal((1, Constants.Two, new DateTime(3)), result);
  }

  [Fact]
  public async Task WhenAll_TakesTupleOfFourTasksAndReturnsTupleOfFourResult() {
    var tasks = (GetIntAsync(1), GetStringAsync(Constants.Two), GetDateTimeAsync(3), GetIntAsync(4));
    var result = await tasks.WhenAll();
    Assert.Equal((1, Constants.Two, new DateTime(3), 4), result);
  }

  //[Fact]
  //public async Task WhenAll_TakesTupleOfFiveTasksAndReturnsTupleOfFiveResult() {
  //  var tasks = (GetIntAsync(1), GetStringAsync(Constants.Two), GetDateTimeAsync(3), GetIntAsync(4), GetStringAsync(Constants.Five));
  //  var result = await tasks.WhenAll();
  //  Assert.Equal((1, Constants.Two, new DateTime(3), 4, Constants.Five), result);
  //}

  //[Fact]
  //public async Task WhenAll_TakesTupleOfSixTasksAndReturnsTupleOfSixResult() {
  //  var tasks = (
  //    GetIntAsync(1), GetStringAsync(Constants.Two), GetDateTimeAsync(3), GetIntAsync(4), GetStringAsync(Constants.Five),
  //    GetDateTimeAsync(6));
  //  var result = await tasks.WhenAll();
  //  Assert.Equal((1, Constants.Two, new DateTime(3), 4, Constants.Five, new DateTime(6)), result);
  //}

  //[Fact]
  //public async Task WhenAll_TakesTupleOfSevenTasksAndReturnsTupleOfSevenResult() {
  //  var tasks = (
  //    GetIntAsync(1), GetStringAsync(Constants.Two), GetDateTimeAsync(3), GetIntAsync(4), GetStringAsync(Constants.Five),
  //    GetDateTimeAsync(6), GetIntAsync(7));
  //  var result = await tasks.WhenAll();
  //  Assert.Equal((1, Constants.Two, new DateTime(3), 4, Constants.Five, new DateTime(6), 7), result);
  //}

  //[Fact]
  //public async Task WhenAll_TakesTupleOfEightTasksAndReturnsTupleOfEightResult() {
  //  var tasks = (
  //    GetIntAsync(1), GetStringAsync(Constants.Two), GetDateTimeAsync(3), GetIntAsync(4), GetStringAsync(Constants.Five),
  //    GetDateTimeAsync(6), GetIntAsync(7), GetStringAsync(Constants.Eight));
  //  var result = await tasks.WhenAll();
  //  Assert.Equal((1, Constants.Two, new DateTime(3), 4, Constants.Five, new DateTime(6), 7, Constants.Eight), result);
  //}

  //[Fact]
  //public async Task TakesTupleOfNineTasksAndReturnsTupleOfNineResult() {
  //  var tasks = (
  //    GetIntAsync(1), GetStringAsync(Constants.Two), GetDateTimeAsync(3), GetIntAsync(4), GetStringAsync(Constants.Five),
  //    GetDateTimeAsync(6), GetIntAsync(7), GetStringAsync(Constants.Eight), GetDateTimeAsync(9));
  // var result = await tasks.WhenAll();
  //  Assert.Equal((1, Constants.Two, new DateTime(3), 4, Constants.Five, new DateTime(6), 7, Constants.Eight, new DateTime(9)), result);
  //}

  [Fact]
  public async Task WhenAll_TakesTupleOfTenTasksAndReturnsTupleOfTenResult() {
    var tasks = (
      GetIntAsync(1), GetStringAsync(Constants.Two), GetDateTimeAsync(3), GetIntAsync(4), GetStringAsync(Constants.Five),
      GetDateTimeAsync(6), GetIntAsync(7), GetStringAsync(Constants.Eight), GetDateTimeAsync(9), GetIntAsync(10));
    var result = await tasks.WhenAll();
    Assert.Equal((1, Constants.Two, new DateTime(3), 4, Constants.Five, new DateTime(6), 7, Constants.Eight, new DateTime(9), 10), result);
  }

  [Fact]
  public async Task WhenAll_ThrowsWhenATaskThrows() {
    var tasks = (GetIntAsync(1), GetExceptionAsync(), GetDateTimeAsync(3));
    await Assert.ThrowsAsync<AggregateException>(() => tasks.WhenAll());
  }

  [Fact]
  public async Task WhenAll_ResultsInFaultedStateWhenATaskThrows() {
    var tasks = (GetIntAsync(1), GetExceptionAsync(), GetDateTimeAsync(3));
    try {
      await tasks.WhenAll();
      throw new Exception("Should not reach here");
    } catch (AggregateException) {
      Assert.True(tasks.Item2.IsFaulted);
    }
  }

  [Fact]
  public async Task WhenAll_ThrowsWhenMoreThanOneTaskThrows() {
    var tasks = (GetExceptionAsync(Constants.First), GetExceptionAsync(Constants.Second), GetDateTimeAsync(3));
    var exception = await Assert.ThrowsAsync<AggregateException>(() => tasks.WhenAll());
    Assert.Equal(Constants.First, exception.InnerExceptions[0].Message);
    Assert.Equal(Constants.Second, exception.InnerExceptions[1].Message);
  }

  [Fact]
  public async Task WhenAll_ThrowsWhenTheTasksTupleContainedANullTask() {
    (Task<int>, Task<string>, Task<DateTime>) tasks = (GetExceptionAsync(Constants.First), null, GetDateTimeAsync(3));
    var exception = await Assert.ThrowsAsync<ArgumentException>(() => tasks.WhenAll());
  }

  [Fact]
  public async Task WhenAll_ThrowsACancelledExceptionWhenOneTaskIsCancelled() {
    var tasks = (GetIntAsync(1), GetCancelledAsync(), GetIntAsync(3));
    var exception = await Assert.ThrowsAsync<TaskCanceledException>(() => tasks.WhenAll());
  }

  [Fact]
  public async Task WhenAll_ResultsInCancelledStateWhenOneTaskIsCancelled() {
    var tasks = (GetIntAsync(1), GetCancelledAsync(), GetIntAsync(3));
    var result = tasks.WhenAll();
    try {
      await result;
      throw new Exception("Should not get here");
    } catch (TaskCanceledException) {
      Assert.Equal(TaskStatus.Canceled, result.Status);
    }
  }
  private Task<int> GetIntAsync(int value) => Task.FromResult(value);
  private Task<string> GetStringAsync(string value) => Task.FromResult(value);
  private Task<DateTime> GetDateTimeAsync(long value) => Task.FromResult(new DateTime(value));

  private async Task<int> GetExceptionAsync(string message = null) {
    for (var i = 0; i <= 10; i++) {
      await Task.Delay(1);
      if (i == 2) {
        throw new Exception(message);
      }
    }
    return 44;
  }

  private async Task<int> GetCancelledAsync() {
    for (var i = 0; i <= 10; i++) {
      await Task.Delay(1);
      if (i == 2) {
        return await Task.FromCanceled<int>(new CancellationToken(true));
      }
    }
    return 44;
  }

}