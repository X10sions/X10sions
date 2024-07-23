namespace RCommon.Persistence.Crud;

public class RepositoryException : ApplicationException {
  public RepositoryException(string message, Exception innerException) : base(message, innerException) {  }
}
