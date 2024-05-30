namespace System.Threading.Tasks;
public static class TaskExtensions {

  public static async Task<(T1, T2)> WhenAll<T1, T2>(this (Task<T1>, Task<T2>) tasks) {
    var task = Task.WhenAll(tasks.Item1, tasks.Item2);
    try {
      await task;
    } catch (Exception) {
      if (task.Exception != null) {
        throw task.Exception;
      }
      throw;
    }
    return (tasks.Item1.Result, tasks.Item2.Result);
  }

  public static async Task<(T1, T2, T3)> WhenAll<T1, T2, T3>(this (Task<T1>, Task<T2>, Task<T3>) tasks) {
    var task = Task.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3);
    try {
      await task;
    } catch (Exception) {
      if (task.Exception != null) {
        throw task.Exception;
      }
      throw;
    }
    return (tasks.Item1.Result, tasks.Item2.Result, tasks.Item3.Result);
  }

  public static async Task<(T1, T2, T3, T4)> WhenAll<T1, T2, T3, T4>(this (Task<T1>, Task<T2>, Task<T3>, Task<T4>) tasks) {
    var task = Task.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4);
    try {
      await task;
    } catch (Exception) {
      if (task.Exception != null) {
        throw task.Exception;
      }
      throw;
    }
    return (tasks.Item1.Result, tasks.Item2.Result, tasks.Item3.Result, tasks.Item4.Result);
  }

  public static async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
   this (Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>, Task<T9>, Task<T10>) tasks) {
    var task = Task.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10);
    try {
      await task;
    } catch (Exception) {
      if (task.Exception != null) {
        throw task.Exception;
      }
      throw;
    }
    return (tasks.Item1.Result, tasks.Item2.Result, tasks.Item3.Result, tasks.Item4.Result, tasks.Item5.Result,
            tasks.Item6.Result, tasks.Item7.Result, tasks.Item8.Result, tasks.Item9.Result, tasks.Item10.Result);
  }


}