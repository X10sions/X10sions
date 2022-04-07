using Systems;

namespace Microsoft.AspNetCore.Builder {
  public static class WebApplicationBuilderExtensions {

    public static WebApplicationBuilder AddScopedConfigurationOptionWithValidation<TIAppSettings, TAppSettings>(this WebApplicationBuilder builder)
      where TIAppSettings : class where TAppSettings : class, TIAppSettings {
      builder.AddScopedConfigurationOptionWithValidation<TAppSettings>();
      builder.Services.AddScoped<TIAppSettings, TAppSettings>(sp => sp.GetRequiredService<IOptionsMonitor<TAppSettings>>().CurrentValue);
      //builder.Services.AddScoped<TIAppSettings, TAppSettings>(sp => sp.GetRequiredService<IOptionsSnapshot<AppSettings>>().Value);
      return builder;
    }

    public static WebApplicationBuilder AddScopedConfigurationOptionWithValidation<TAppSettings>(this WebApplicationBuilder builder) where TAppSettings : class {
      builder.ConfigureWithValidation<TAppSettings>();
      //https://stackoverflow.com/questions/62761483/how-to-customise-configuration-binding-in-asp-net-core
      builder.Services.AddScoped(sp => sp.GetRequiredService<IOptionsMonitor<TAppSettings>>().CurrentValue);
      //builder.Services.AddScoped(sp => sp.GetRequiredService<IOptionsSnapshot<AppSettings>>().Value);
      return builder;
    }

    public static IServiceCollection ConfigureWithValidation<T>(this WebApplicationBuilder builder) where T : class => builder.Services
      .Configure<T>(builder.Configuration)
      .PostConfigure<T>(settings => {
        var configErrors = settings.GetValidationErrorMessages();
        if (configErrors.Any()) {
          var aggrErrors = string.Join(",", configErrors);
          var count = configErrors.Count();
          var configType = typeof(T).Name;
          throw new ApplicationException($"Found {count} configuration error(s) in {configType}: {aggrErrors}");
        }
    });

  }
}
