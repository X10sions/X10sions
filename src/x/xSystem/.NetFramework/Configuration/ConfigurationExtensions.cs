using System.Net.Configuration;
using System.Web.Configuration;

namespace System.Configuration {
  public static class ConfigurationExtensions {
    static class SectionGroupNames {
      public const string System_Net = "system.net";
      public const string System_Web = "system.web";
    }
    public static KeyValueConfigurationCollection GetAppSettings(this Configuration configuration) => (KeyValueConfigurationCollection)(configuration.AppSettings?.Settings ?? ((object)new ConnectionStringSettingsCollection()));
    public static ConnectionStringSettingsCollection GetConnectionStrings(this Configuration configuration) => configuration.ConnectionStrings?.ConnectionStrings ?? new ConnectionStringSettingsCollection();
    public static NetSectionGroup GetSystemNet(this Configuration configuration) => (NetSectionGroup)(configuration.GetSectionGroup(SectionGroupNames.System_Net) ?? new NetSectionGroup());
    public static MailSettingsSectionGroup GetSystemNetMailSettings(this Configuration configuration) => configuration.GetSystemNet().MailSettings ?? new MailSettingsSectionGroup();
    public static SmtpSection GetSystemNetMailSettingsSmtp(this Configuration configuration) => configuration.GetSystemNet().MailSettings.Smtp ?? new SmtpSection();
    public static SystemWebSectionGroup GetSystemWeb(this Configuration configuration) => (SystemWebSectionGroup)(configuration.GetSectionGroup(SectionGroupNames.System_Web) ?? new SystemWebSectionGroup());
  }
}
