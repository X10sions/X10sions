using Microsoft.Extensions.Options;

namespace X10sions.Wsus.Web;
public class AppSettings {

  public _ConnectionStrings ConnectionStrings { get; set; } = new _ConnectionStrings();

  public class _ConnectionStrings : Dictionary<string, string> {
    public string SUSDB { get => this[nameof(SUSDB)]; set => this[nameof(SUSDB)] = value; }
    public string WID { get => this[nameof(WID)]; set => this[nameof(WID)] = value; }
  }

  public static AppSettings Configure(WebApplicationBuilder builder) {
    builder.Services.Configure<AppSettings>(builder.Configuration);
    builder.Services.AddScoped(sp => sp.GetRequiredService<IOptionsSnapshot<AppSettings>>().Value);
    //builder.Services.AddScoped(sp => sp.GetRequiredService<IOptionsMonitor<AppSettings>>().CurrentValue);
    return builder.Configuration.Get<AppSettings>() ?? throw new ArgumentNullException();
  }

}
