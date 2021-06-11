namespace LinqToDB.Configuration {
  public static class Proxy {

    /// <summary>
    /// Unwraps all proxies, applied to passed object and returns unproxyfied value.
    /// </summary>
    /// <typeparam name="T">Type of proxified object.</typeparam>
    /// <param name="obj">Object, that must be stripped of proxies.</param>
    /// <returns>Unproxified object.</returns>
    public static T GetUnderlyingObject<T>(T obj) {
      while (obj is IProxy<T> proxy)
        obj = proxy.UnderlyingObject;
      return obj;
    }

  }
}
