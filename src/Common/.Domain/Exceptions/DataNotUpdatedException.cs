namespace Common.Domain.Exceptions;

public class DataNotUpdatedException : Exception {
  public DataNotUpdatedException() { }
  public DataNotUpdatedException(string message) : base(message) { }
  public DataNotUpdatedException(string message, Exception innerException) : base(message, innerException) { }
}
