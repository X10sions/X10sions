using Microsoft.Internal.WindowsBase;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace Microsoft.Internal {

  [FriendAccessAllowed]
  public static class CriticalExceptions {
    [FriendAccessAllowed]
    public static bool IsCriticalException(Exception ex) {
      ex = Unwrap(ex);
      if (!(ex is NullReferenceException) && !(ex is StackOverflowException) && !(ex is OutOfMemoryException) && !(ex is ThreadAbortException) && !(ex is SEHException)) {
        return ex is SecurityException;
      }
      return true;
    }

    [FriendAccessAllowed]
    public static bool IsCriticalApplicationException(Exception ex) {
      ex = Unwrap(ex);
      if (!(ex is StackOverflowException) && !(ex is OutOfMemoryException) && !(ex is ThreadAbortException)) {
        return ex is SecurityException;
      }
      return true;
    }

    [FriendAccessAllowed]
    public static Exception Unwrap(Exception ex) {
      while (ex.InnerException != null && ex is TargetInvocationException) {
        ex = ex.InnerException;
      }
      return ex;
    }
  }
}
