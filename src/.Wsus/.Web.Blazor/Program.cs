using Microsoft.EntityFrameworkCore;
using X10sions.Wsus.Components;
using X10sions.Wsus.Data;
using X10sions.Wsus.Web.Blazor;

var builder = WebApplication.CreateBuilder(args);
var appSettings = AppSettings.Configure(builder);

builder.Services.AddDbContext<SusdbDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.SUSDB ?? throw new InvalidOperationException("Connection string 'X10sionsWsusContext' not found.")));

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddScoped(_ => new SusdbSqlSugarClient(appSettings.ConnectionStrings.SUSDB));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
