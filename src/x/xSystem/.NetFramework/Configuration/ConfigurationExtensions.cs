using System.Net.Configuration;
using System.Web.Configuration;

namespace System.Configuration {
  public static class ConfigurationExtensions {
    public static KeyValueConfigurationCollection GetAppSettings(this Configuration configuration) => (KeyValueConfigurationCollection)(configuration.AppSettings?.Settings ?? ((object)new ConnectionStringSettingsCollection()));
    public static ConnectionStringSettingsCollection GetConnectionStrings(this Configuration configuration) => configuration.ConnectionStrings?.ConnectionStrings ?? new ConnectionStringSettingsCollection();
    public static NetSectionGroup GetSystemNet(this Configuration configuration) => (NetSectionGroup)(configuration.GetSectionGroup("system.net") ?? new NetSectionGroup());
    public static MailSettingsSectionGroup GetSystemNetMailSettings(this Configuration configuration) => (MailSettingsSectionGroup)(configuration.GetSectionGroup("system.net/mailSettings") ?? new MailSettingsSectionGroup());
    public static SmtpSection GetSystemNetMailSettingsSmtp(this Configuration configuration) => (SmtpSection)(configuration.GetSection("system.net/mailSettings/smtp") ?? new SmtpSection());
    public static SystemWebSectionGroup GetSystemWeb(this Configuration configuration) => (SystemWebSectionGroup)(configuration.GetSectionGroup("system.web") ?? new SystemWebSectionGroup());
  }
}
