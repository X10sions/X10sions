namespace Common.Data.Odbc {
  public class MicrosoftExcelOdbcConnectionStringBuilder : _BaseMicrosoftOdbcConnectionStringBuilder {
    public const string _DriverName = "{Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)}";
    public static string[] _FileExtensions = new[] { "xls", "xlsx", "xlsm", "xlsb" };
    public const int _DriverId = 1046;

    public MicrosoftExcelOdbcConnectionStringBuilder() : this(null) { }

    public MicrosoftExcelOdbcConnectionStringBuilder(string dbq, string driverName = _DriverName, string fil = "excel 12.0", int driverId = _DriverId)
      : base(driverName, fil, driverId, dbq) { }

  }
}