namespace Microsoft.Extensions.Configuration;

public static class IConfigurationExtensions {
  public static IConfigurationSection GetSection(this IConfiguration configuration, string configurationSectionName, bool isRequired)
    => isRequired ? configuration.GetRequiredSection(nameof(configurationSectionName)) : configuration.GetSection(configurationSectionName);
}
