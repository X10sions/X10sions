using Common.Models.Html.Tags;

namespace System.Data {
  public static class DataRowExtensions {

    public static string ToHtmlSelectOption(this DataRow row, string valueField, string textField, string selectedValue)
      => new Option {
        Value = (string)row[valueField],
        Text = (string)row[textField],
        Selected = selectedValue == (string)row[valueField]
      }.ToHtml();

  }
}