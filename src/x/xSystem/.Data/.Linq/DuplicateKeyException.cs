namespace System.Data.Linq {
  public class DuplicateKeyException : InvalidOperationException {
    public object Object { get; }

    public DuplicateKeyException(object duplicate) {
      Object = duplicate;
    }

    public DuplicateKeyException(object duplicate, string message) : base(message) {
      Object = duplicate;
    }

    public DuplicateKeyException(object duplicate, string message, Exception innerException) : base(message, innerException) {
      Object = duplicate;
    }
  }
}