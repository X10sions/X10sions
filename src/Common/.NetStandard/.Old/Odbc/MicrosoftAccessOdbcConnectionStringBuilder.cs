namespace Common.Data.Odbc {

  public class MicrosoftAccessOdbcConnectionStringBuilder : _BaseMicrosoftOdbcConnectionStringBuilder {
    public const string _DriverName = "{Microsoft Access Driver (*.mdb, *.accdb)}";
    public const int _DriverId = 25;

    public MicrosoftAccessOdbcConnectionStringBuilder() : this(null) { }

    public MicrosoftAccessOdbcConnectionStringBuilder(string dbq, string driverName = _DriverName, string fil = "MS Access", int driverId = _DriverId)
      : base(driverName, fil, driverId, dbq) { }

    public override string ConnectionString { get => base.ConnectionString; set => this.SetConnectionString(value); }

    public int Exclusive { get => (int)this[nameof(Exclusive)]; set => this[nameof(Exclusive)] = value; }
    public int ExtendedAnsiSQL { get => (int)this[nameof(ExtendedAnsiSQL)]; set => this[nameof(ExtendedAnsiSQL)] = value; }
    public int LocaleIdentifier { get => (int)this["Locale Identifier"]; set => this["Locale Identifier"] = value; }
    public string Pwd { get => (string)this[nameof(Pwd)]; set => this[nameof(Pwd)] = value; }
    public string SystemDB { get => (string)this[nameof(SystemDB)]; set => this[nameof(SystemDB)] = value; }
  }
}