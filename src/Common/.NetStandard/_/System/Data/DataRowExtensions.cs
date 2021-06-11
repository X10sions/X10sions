using Common.Models.Html.Tags;

namespace System.Data {
  public static class DataRowExtensions {

    public static string ToHtmlSelectOption(this DataRow row, string valueField, string textField, string selectedValue)
      => new Option {
        Value = row.Field<string>(valueField),
        Text = row.Field<string>(textField),
        Selected = selectedValue == row.Field<string>(valueField)
      }.ToHtml();

  }
}