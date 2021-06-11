namespace xNonFactors.MvcTemplate.Resources {

  public static class Message {
    public static string For<TView>(string key, params object[] args) {
      var message = Resource.Localized(typeof(TView).Name, "Messages", key);

      return string.IsNullOrEmpty(message) || args.Length == 0 ? message : string.Format(message, args);
    }
  }
}