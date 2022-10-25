namespace System.Collections.Specialized {
  public static class NameValueCollectionExtensions {

    public static string Get(this NameValueCollection coll, string key, string defaultValue) => coll[key] ?? defaultValue;

  }
}
