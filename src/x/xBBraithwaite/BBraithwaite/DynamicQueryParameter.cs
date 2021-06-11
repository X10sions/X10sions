namespace BBaithwaite {
  public class DynamicQueryParameter {
    public string LinkingOperator { get; set; }
    public string PropertyName { get; set; }
    public object PropertyValue { get; set; }
    public string QueryOperator { get; set; }

    internal DynamicQueryParameter(string linkingOperator, string propertyName, object propertyValue, string queryOperator) {
      LinkingOperator = linkingOperator;
      PropertyName = propertyName;
      PropertyValue = propertyValue;
      QueryOperator = queryOperator;
    }
  }
}
