namespace Common.Data.Odbc {
  public abstract class _BaseMicrosoftOdbcConnectionStringBuilder : _BaseOdbcConnectionStringBuilder {
    public _BaseMicrosoftOdbcConnectionStringBuilder() { }

    public _BaseMicrosoftOdbcConnectionStringBuilder(string driverName, string fil, int driverId, string dbq) {
      Dbq = dbq;
      FIL = fil;
      Driver = driverName;
      DriverId = driverId;
    }

    public override string ConnectionString { get => base.ConnectionString; set => this.SetConnectionString(value); }

    public string Dbq { get => (string)this[nameof(Dbq)]; set => this[nameof(Dbq)] = value; }
    public string Uid { get => (string)this[nameof(Uid)]; set => this[nameof(Uid)] = value; } //= "admin";

    public string DefaultDir { get => (string)this[nameof(DefaultDir)]; set => this[nameof(DefaultDir)] = value; }
    public int DriverId { get => (int)this[nameof(DriverId)]; set => this[nameof(DriverId)] = value; }
    public string FIL { get => (string)this[nameof(FIL)]; set => this[nameof(FIL)] = value; }
    public int MaxBufferSize { get => (int)this[nameof(MaxBufferSize)]; set => this[nameof(MaxBufferSize)] = value; }//=2048;
    public int MaxScanRows { get => (int)this[nameof(MaxScanRows)]; set => this[nameof(MaxScanRows)] = value; }//=8;
    public int PageTimeout { get => (int)this[nameof(PageTimeout)]; set => this[nameof(PageTimeout)] = value; }// = 5;
    public int ReadOnly { get => (int)this[nameof(ReadOnly)]; set => this[nameof(ReadOnly)] = value; }//=0;
    public int SafeTransactions { get => (int)this[nameof(SafeTransactions)]; set => this[nameof(SafeTransactions)] = value; }//=0;
    public int Threads { get => (int)this[nameof(Threads)]; set => this[nameof(Threads)] = value; }// = 3;
    public string UserCommitSync { get => (string)this[nameof(UserCommitSync)]; set => this[nameof(UserCommitSync)] = value; }//="Yes";
  }
}