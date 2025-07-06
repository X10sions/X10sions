namespace RCommon.Persistence.Dapper;
public class DapperFluentMappingsException : GeneralException {
  public DapperFluentMappingsException(string message) : base(SeverityOptions.Critical, message) { }
}
