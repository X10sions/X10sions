using Common.Enums;
using System;
using System.Data.Common;
using System.IO;

namespace Common.Helpers {
  public static class MsAccessHelper {

    public enum FileExtension {
      accdb,
      mdb
    }

    public class ConnectionStringBuilder : DbConnectionStringBuilder {

      public const char keyDelim = ';';
      public const char valueDelim = '=';

      public ConnectionStringBuilder(string connectionString) {
        var kvp = connectionString.ToKeyValueDictionary(keyDelim, valueDelim, StringSplitOptions.RemoveEmptyEntries);
        foreach (var kv in connectionString.ToKeyValuePairs(keyDelim, valueDelim, StringSplitOptions.RemoveEmptyEntries)) {
          this[kv.Key] = kv.Value;
        }
        Provider = DataProviders.Microsoft_ACE_OLEDB_12.Name();
      }

      public ConnectionStringBuilder(string dataSource, DataProviders provider) : this(dataSource, provider.Name()) { }
      public ConnectionStringBuilder(string dataSource, string provider) {
        DataSource = dataSource;
        Provider = provider;
      }

      const string DataSourceKey = "Data Source";
      public string DataSource { get => (string)this[nameof(DataSourceKey)]; set => this[nameof(DataSourceKey)] = value; }

      public string DirectoryName { get => Path.GetDirectoryName(DataSource); set => this[nameof(DataSourceKey)] = Path.Combine(value, FileName + "." + FileExtension); }
      public string FileName { get => Path.GetFileNameWithoutExtension(DataSource); set => this[nameof(DataSourceKey)] = Path.Combine(DirectoryName, value + "." + FileExtension); }
      public string FileExtension { get => Path.GetExtension(DataSource).TrimStart('.'); set => this[nameof(DataSourceKey)] = Path.Combine(DirectoryName, FileName + "." + value); }
      public FileExtension AccessFileExtension { get => (FileExtension)Enum.Parse(typeof(FileExtension), FileExtension, true); set => this[nameof(DataSourceKey)] = Path.Combine(DirectoryName, FileName + "." + value.ToString()); }

      public string Provider { get => (string)this[nameof(Provider)]; set => this[nameof(Provider)] = value; }

    }

  }
}