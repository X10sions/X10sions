namespace LinqToDB.Data.RetryPolicy {
  public class DB2iSeriesRetryPolicy : RetryPolicyBase {
    public DB2iSeriesRetryPolicy(int? maxRetryCount, TimeSpan? maxRetryDelay = null) : base(
      maxRetryCount ?? Common.Configuration.RetryPolicy.DefaultMaxRetryCount,
      maxRetryDelay ?? Common.Configuration.RetryPolicy.DefaultMaxDelay,
      Common.Configuration.RetryPolicy.DefaultRandomFactor,
      Common.Configuration.RetryPolicy.DefaultExponentialBase,
      Common.Configuration.RetryPolicy.DefaultCoefficient) { }
    protected override bool ShouldRetryOn(Exception exception) {
      //ERROR [08004] [IBM][System i Access ODBC Driver]Communication link failure. comm rc=10061 - CWBCO1049 - The IBM i server application  is not started, or the connection was blocked by a firewall
      return exception.Message.Contains("[08004]");
    }
  }
}
