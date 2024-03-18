using Microsoft.Extensions.Options;
using System.Diagnostics.Contracts;

namespace X10sions.Wsus.Web.Blazor;
public class AppSettings {

  public ConnectionStringsAppSettings ConnectionStrings { get; set; } = new();


  public class ConnectionStringsAppSettings : Dictionary<string, string> {
    public ConnectionStringsAppSettings() : base(StringComparer.OrdinalIgnoreCase) { }
    public string SUSDB { get => this[nameof(SUSDB)]; set => this[nameof(SUSDB)] = value; }
  }

  public static AppSettings Configure(WebApplicationBuilder builder) {
    builder.Services.Configure<AppSettings>(builder.Configuration);
    builder.Services.AddScoped(sp => sp.GetRequiredService<IOptionsSnapshot<AppSettings>>().Value);

    var appSettings = builder.Configuration.Get<AppSettings>();
    Contract.Requires(appSettings != null);
    return builder.Configuration.Get<AppSettings>() ?? throw new InvalidOperationException();
  }

}
