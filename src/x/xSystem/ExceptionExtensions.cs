using System.Collections.Generic;

namespace System {
  public static class ExceptionExtensions {

    public static Exception GetInnerMostException(this Exception ex) {
      var currentEx = ex;
      while (currentEx.InnerException != null) {
        currentEx = currentEx.InnerException;
      }
      return currentEx;
    }

    public static List<Exception> GetInnerExceptionList(this Exception ex) {
      var exceptions = new List<Exception> {
        ex
      };
      var currentEx = ex;
      while (currentEx.InnerException != null) {
        currentEx = currentEx.InnerException;
        exceptions.Add(currentEx);
      }
      exceptions.Reverse();
      return exceptions;
    }

    public static Exception[] GetInnerExceptions(this Exception ex) => ex.GetInnerExceptionList().ToArray();

  }
}