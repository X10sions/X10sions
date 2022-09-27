namespace LinqToDB.DataProvider {
  [UrlAsAt.AccessBulkCopy_2021_06_04]
  public class GenericBulkCopy : BasicBulkCopy { }

}

namespace LinqToDB.DataProvider.Access {

  [UrlAsAt.AccessBulkCopy_2021_06_04]
  public class AccessBulkCopy : BasicBulkCopy {
    /// <remarks>
    /// Settings based on https://www.jooq.org/doc/3.12/manual/sql-building/dsl-context/custom-settings/settings-inline-threshold/
    /// We subtract 1 here to be safe since some ADO providers use parameter for command itself. 
    /// </remarks>
    protected override int MaxParameters => 767;
    /// <remarks>
    /// This max is based on https://support.microsoft.com/en-us/office/access-specifications-0cf3c66f-9cf2-4e32-9568-98c1025bb47c
    /// </remarks>
    protected override int MaxSqlLength => 64000;
  }

}