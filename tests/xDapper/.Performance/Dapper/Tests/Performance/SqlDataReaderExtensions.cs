using System;
using System.Data.SqlClient;

namespace Dapper.Tests.Performance {
  public static class SqlDataReaderExtensions {

    public static string GetNullableString(this SqlDataReader reader, int index) {
      var tmp = reader.GetValue(index);
      if(tmp != DBNull.Value) {
        return (string)tmp;
      }
      return null;
    }

    public static T? GetNullableValue<T>(this SqlDataReader reader, int index) where T : struct {
      var tmp = reader.GetValue(index);
      if(tmp != DBNull.Value) {
        return (T)tmp;
      }
      return null;
    }

  }
}