namespace BBaithwaite {
  public class ValidationError {
    public string PropertyName { get; set; }
    public string Message { get; set; }

    public ValidationError(string propertyName, string message) {
      PropertyName = propertyName;
      Message = message;
    }
  }
}
