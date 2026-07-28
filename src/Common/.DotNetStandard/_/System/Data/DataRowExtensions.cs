using Common.Html.Tags;

namespace System.Data {
  public static class DataRowExtensions {

    public static string ToHtmlSelectOption(this DataRow row, string valueField, string textField, string selectedValue)
      => new OptionHtmlTag((string)row[textField], (string)row[valueField], selectedValue == (string)row[valueField]).GetRawHtml();

  }
}