namespace Common.Exceptions;
public class UnderConstructionException : Exception {
  public UnderConstructionException() : base("This path is currently under construction.") { }

  public UnderConstructionException(string message) : base(message) { }

  public UnderConstructionException(string message, Exception innerException) : base(message, innerException) { }
  //public UnderConstructionException() { }
  //public UnderConstructionException(string message) : base(message) { }
  //public UnderConstructionException(string message, Exception inner) : base(message, inner) { }
  //public UnderConstructionException(string format, params object[] args) : base(string.Format(format, args)) { }
}