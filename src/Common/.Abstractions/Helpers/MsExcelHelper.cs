using Common.Enums;
using System.Data.Common;

namespace Common.Helpers {
  public static class MsExcelHelper {

    public enum ImportExportMode_IMEX {
      ExportMode = 0,// Writes
      ImportMode = 1,// Read Only
      LinkedMOde = 2 // Updates
    }

    public enum FileExtension {
      xls,
      xlsb,
      xlsm,
      xlsx
    }

    public enum Version {
      Excel_08,
      Excel_12,
      Excel_12_Macro,
      Excel_12_Xml
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

      public ConnectionStringBuilder(string dataSource, bool hasHeader, ImportExportMode_IMEX? imex = null)
        : this(dataSource, DataProviders.Microsoft_ACE_OLEDB_12.Name(), hasHeader, imex) {
      }

      public ConnectionStringBuilder(string dataSource, string provider, bool hasHeader, ImportExportMode_IMEX? imex = null) {
        DataSource = dataSource;
        Provider = provider;

        ExtendedProperties = new ExtendedProperties(ExcelFileExtension.Version()) { HasHeader = hasHeader, IMEX = imex };
        this[nameof(ExtendedPropertiesKey)] = ExtendedProperties.ToCsvString;
      }

      const string DataSourceKey = "Data Source";
      public string DataSource { get => (string)this[nameof(DataSourceKey)]; set => this[nameof(DataSourceKey)] = value; }

      public string DirectoryName { get => Path.GetDirectoryName(DataSource); set => this[nameof(DataSourceKey)] = Path.Combine(value, FileName + "." + FileExtension); }
      public string FileName { get => Path.GetFileNameWithoutExtension(DataSource); set => this[nameof(DataSourceKey)] = Path.Combine(DirectoryName, value + "." + FileExtension); }
      public string FileExtension { get => Path.GetExtension(DataSource).TrimStart('.'); set => this[nameof(DataSourceKey)] = Path.Combine(DirectoryName, FileName + "." + value); }
      public FileExtension ExcelFileExtension { get => (FileExtension)Enum.Parse(typeof(FileExtension), FileExtension, true); set => this[nameof(DataSourceKey)] = Path.Combine(DirectoryName, FileName + "." + value.ToString()); }

      const string ExtendedPropertiesKey = "Extended Properties";
      public readonly ExtendedProperties ExtendedProperties;

      public bool HasHeader { get => ExtendedProperties.HasHeader; set { ExtendedProperties.HasHeader = value; this[nameof(ExtendedPropertiesKey)] = ExtendedProperties.ToCsvString; } }

      public ImportExportMode_IMEX IMEX { get => (ImportExportMode_IMEX)ExtendedProperties.IMEX; set { ExtendedProperties.IMEX = value; this[nameof(ExtendedPropertiesKey)] = ExtendedProperties.ToCsvString; } }

      public Version Version => ExtendedProperties.Version;
      public string Provider { get => (string)this[nameof(Provider)]; set => this[nameof(Provider)] = value; }
    }

    public class ExtendedProperties {
      public ExtendedProperties(Version version) {
        Version = version;
      }
      public Version Version { get; }
      public bool HasHeader { get; set; }
      public ImportExportMode_IMEX? IMEX { get; set; }

      public string ToCsvString => Version.ToVersionString() + ";" + (HasHeader ? "HDR=Yes;" : string.Empty) + (IMEX.HasValue ? $"IMEX={IMEX.Value};" : string.Empty);

    }

  }

  public static class MsExcelHelper_FileExtensionExtensions {

    public static MsExcelHelper.Version Version(this MsExcelHelper.FileExtension @this) {
      switch (@this) {
        case MsExcelHelper.FileExtension.xls: return MsExcelHelper.Version.Excel_08;
        case MsExcelHelper.FileExtension.xlsb: return MsExcelHelper.Version.Excel_12;
        case MsExcelHelper.FileExtension.xlsm: return MsExcelHelper.Version.Excel_12_Macro;
        case MsExcelHelper.FileExtension.xlsx: return MsExcelHelper.Version.Excel_12_Xml;
        default: throw new NotImplementedException($"Unknown excel file extension: {@this}.");
      }
    }

  }

  public static class MsExcelHelper_VersionExtensions {

    public static bool IsMacroVersion(this MsExcelHelper.Version version) => version == MsExcelHelper.Version.Excel_12_Macro;
    public static bool IsXmlVersion(this MsExcelHelper.Version version) => version == MsExcelHelper.Version.Excel_12_Xml;
    public static int ToVersionNumber(this MsExcelHelper.Version version) => version == MsExcelHelper.Version.Excel_08 ? 8 : 12;
    public static string ToVersionString(this MsExcelHelper.Version version) => $"Excel {version.ToVersionNumber()}.0{(version.IsMacroVersion() ? " Macro" : version.IsXmlVersion() ? " Xml" : string.Empty)}";

  }
}
