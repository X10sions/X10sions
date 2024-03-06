namespace Common.Attributes {
  [Obsolete("TODO: Work still to be done", false)]
  public class ToDoAttribute : Attribute {
    public ToDoAttribute() : this(string.Empty) { }

    public ToDoAttribute(string message) {
      Message = message;
    }
    public string Message { get; set; }
  }
}