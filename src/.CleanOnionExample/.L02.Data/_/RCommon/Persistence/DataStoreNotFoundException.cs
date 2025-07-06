namespace RCommon.Persistence;

public class DataStoreNotFoundException : GeneralException {
  public DataStoreNotFoundException(string message) : base(message) {

  }
}
