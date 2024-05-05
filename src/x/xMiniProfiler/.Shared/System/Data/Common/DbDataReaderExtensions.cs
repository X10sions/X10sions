using StackExchange.Profiling.Data;

namespace System.Data.Common;
public static class DbDataReaderExtensions {
  public static DbDataReader UnwrapDataReader(this DbDataReader dataReader) => dataReader is ProfiledDbDataReader dr ? dr.WrappedReader : dataReader;
}