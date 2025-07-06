namespace Microsoft.AspNetCore.Mvc.ViewFeatures;
public static class ViewDataDictionaryExtensions {

  public static string Title(this ViewDataDictionary viewData) => viewData[nameof(Title)]?.ToString();
  public static string Title(this ViewDataDictionary viewData, string newValue) {
    viewData[nameof(Title)] = newValue;
    return newValue;
  }

}
