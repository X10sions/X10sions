namespace RCommon.Persistence;

public class PersistenceException : GeneralException {
  public PersistenceException(string message, Exception exception) : base(message, exception) {  }
}