namespace System.Data {
  public static class DataRowExtensions {
    public static void MapToObject(this DataRow @this, object obj) {
      foreach (var propertyInfo in obj.GetType().GetProperties()) {
        if (@this.Table.Columns.Contains(propertyInfo.Name)) {
          var column = @this.Table.Columns[propertyInfo.Name];
          var objectValue = (@this[column]);
          if (!(objectValue is DBNull)) {
            propertyInfo.SetValue((obj), (Convert.ChangeType((objectValue), propertyInfo.PropertyType)), null);
          }
        }
      }
    }

    public static string ToHtmlSelectOption(this DataRow @this, string valueField, string textField) => HtmlForOption((@this[valueField]), (@this[textField]), string.Empty);

    public static string ToHtmlSelectOption(this DataRow @this, string valueField, string textField, int selectedValue) => HtmlForOption((@this[valueField]), (@this[textField]), selectedValue);

    private static string HtmlForOption<T>(object dataValue, object dataText, T selectedValue) => string.Format("<option value=\"{0}\"{1}>{2}</option>", dataValue.ToString(), (dataText == null) ? dataValue.ToString() : dataText.ToString(), ((selectedValue.ToString().ToUpper() == dataValue.ToString().ToUpper())) ? " selected=\"selected\" " : "");


  }
}
