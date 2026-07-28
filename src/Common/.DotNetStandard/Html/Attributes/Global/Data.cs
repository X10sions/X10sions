#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Html.Attributes {
  public interface IDataHtmlAttribute : IHtmlAttribute<DataAttributeDictionary> { };

  public static class IDataHtmlAttributeExtensions {

    public static IDataHtmlAttribute Set(this IDataHtmlAttribute attr, params DataItem[] dataItems) {
      if (dataItems is not null) {
        foreach (var dataItem in dataItems) {
          //if (dataItem != null) {
          attr.Value[dataItem.Key] = dataItem;
          //}
        }
      }
      return attr;
    }

    [Obsolete] public static IDataHtmlAttribute Set(this IDataHtmlAttribute attr, string suffix, object value) => attr.Set(new DataItem(suffix, value));

  }

  /// <summary>Used To store custom data Private To the page Or application</summary>
  public readonly record struct DataHtmlAttribute : IDataHtmlAttribute {
    public DataHtmlAttribute(DataAttributeDictionary value) {
      Value = value ?? new DataAttributeDictionary();
    }

    public DataAttributeDictionary Value { get; } = new DataAttributeDictionary();
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.data;
  }

  public readonly record struct DataItem(string Suffix, object Value) {
    public const string Prefix = "data-";
    public string HtmlKeyValue => this.GetHtmlKeyValue(Value);
    public string Key => $"{Prefix}{Suffix}";

    public static string GetHtmlKeyValue<T>(string suffix, T value) => $"{Prefix}{suffix}=\"{value}\"";
  }

  public static class DataItemExtensions {
    public static string JoinHtmlKeyValues(this IEnumerable<DataItem> dataItems) => string.Join(" ", dataItems.Select(x => x.HtmlKeyValue));
    public static string GetHtmlKeyValue<T>(this DataItem dataItem) => DataItem.GetHtmlKeyValue(dataItem.Suffix, dataItem.Value);
    public static string GetHtmlKeyValue<T>(this DataItem dataItem, T value) => DataItem.GetHtmlKeyValue(dataItem.Suffix, value);
  }

  public class DataAttributeDictionary : Dictionary<string, DataItem> {
    public DataAttributeDictionary() : base(StringComparer.OrdinalIgnoreCase) { }
    //public DataAttributeDictionary(IEnumerable<DataItem> items) : base(StringComparer.OrdinalIgnoreCase) {
    //  this.Set(items);
    //}

    public string GetJoinedHtmlKeyValues() {
      var attributes = from item in Values
                         //where !string.IsNullOrWhiteSpace(a.Value.HtmlKeyValue)
                       orderby item.Suffix
                       select item.HtmlKeyValue;
      return string.Join(" ", attributes);
    }

  }

}