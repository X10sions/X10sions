using CleanOnionExample.Data.Entities;
using CleanOnionExample.Data.Entities.Services;
using Microsoft.Extensions.Options;

namespace CleanOnionExample.Infrastructure.Extension {
  public static class WebApplicationBuilderExtensions {

    public static void ConfigureWebApi(this WebApplicationBuilder builder) {
      Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
      builder.Logging.AddSerilog();

      //var appSettingsPaths = builder.Configuration.GetSection("AppSettingsPaths").Get<List<string>>();
      //builder.ImportConfigurationJsonFiles(appSettingsPaths, "AppSettingsRemote.json");
      builder.AddScopedConfigurationOptionWithValidation<AppSettings>();

      builder.Services.AddControllers().AddNewtonsoftJson();
      builder.Services.AddApplicationDbContext(builder.Configuration.GetConnectionString("OnionArchConn"));
      builder.Services.AddIdentityService(builder.Configuration);
      builder.Services.AddAutoMapper();
      builder.Services.AddScopedServices();
      builder.Services.AddTransientServices();
      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();
      builder.Services.AddSwaggerOpenAPI();
      builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
      builder.Services.AddServiceLayer();
      builder.Services.AddVersion();

      var servicesProvider = builder.Services.BuildServiceProvider();
      var applicationDetailOptions = servicesProvider.GetRequiredService<IOptionsMonitor<AppSettings>>();
      //var appSettings = applicationDetailOptions.CurrentValue;
      //var appSettings = builder.Configuration.Get<AppSettings>();

      builder.Services.AddHealthCheck(applicationDetailOptions, builder.Configuration);
      builder.Services.AddFeatureManagement();
    }

    public static void ConfigureWebBlazorWasm(this WebAssemblyHostBuilder builder) {
      builder.RootComponents.Add<HeadOutlet>("head::after");

      builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecastApiRepository>();
      builder.Services.AddScoped<WeatherForecastService>();

      //builder.Services.AddSingleton<IWeatherForecastDataBroker, WeatherForecastServerDataBroker>();
      //builder.Services.AddSingleton<WeatherForecastDataStore>();

      builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
    }





  }
}
