namespace Microsoft.Extensions.Configuration;

public static class IConfigurationExtensions {
  public static IConfigurationSection GetSection(this IConfiguration configuration, string configurationSectionName, bool isRequired)
    => isRequired ? configuration.GetRequiredSection(configurationSectionName) : configuration.GetSection(configurationSectionName);
}
