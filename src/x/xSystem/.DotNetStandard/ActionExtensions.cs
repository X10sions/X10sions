using System.Web;

namespace System;

public static class ActionExtensions {

  public static Task FromResultOf(this Action action) {
    try {
      action();
      return Task.CompletedTask;
    } catch (Exception ex) {
      return Task.FromException(ex);
    }
  }

}
