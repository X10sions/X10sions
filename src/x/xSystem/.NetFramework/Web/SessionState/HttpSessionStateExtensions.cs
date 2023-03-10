namespace System.Web.SessionState {
  public static class HttpSessionStateExtensions {

    //public static T Ensure<T>(this HttpSessionState self, string key) where T : class, new() {
    //  var value = self.Get<T>(key);
    //  if (value != null) {
    //    return value;
    //  }
    //  value = new T();
    //  self.Set(key, value);
    //  return value;
    //}

    public static T? Get<T>(this HttpSessionState session) => session.Get<T>(typeof(T).FullName ?? typeof(T).Name);

    public static T? Get<T>(this HttpSessionState session, string key, T? defaultValue = default) {
      if (session is null) throw new ArgumentNullException(nameof(session));
      var value = session[key];
      return value is null || value is not T ? defaultValue : (T)value;
    }

    //public static T Get<T>(this HttpSessionState session, string key, Func<object, T> valueSelector) => valueSelector(session[key]);

    //public static T GetOrAdd<T>(this HttpSessionState ses, string sessionKey, T defaultValue) {
    //  var value = Get<T>(ses, sessionKey);
    //  if (value == null || value is not T) {
    //    ses.Add(sessionKey, defaultValue);
    //    value = defaultValue;
    //  }
    //  return value;
    //}

    public static bool HasKey(this HttpSessionState ses, string key) => ses[key] != null;

    //public static void RemoveKey(this HttpSessionState ses, string sessionKey) {
    //  if (HasKey(ses, sessionKey)) {
    //    ses.Remove(sessionKey);
    //  }
    //}

    public static void Set<T>(this HttpSessionState session, T value) => session.Set(typeof(T).FullName ?? typeof(T).Name, value);

    public static T Set<T>(this HttpSessionState session, string key, T value) {
      if (session is null) throw new ArgumentNullException(nameof(session));
      session[key] = value;
      return value;
    }

    public static IDictionary<string, object?> ToDictionary(this HttpSessionState session) {
      var dic = new Dictionary<string, object?>();
      foreach (string k in session.Keys) {
        var value = session[k];
        dic.Add(k, value);
      }
      return dic;
    }

  }
}
