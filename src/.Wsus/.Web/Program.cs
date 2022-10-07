using Microsoft.Extensions.Options;
using X10sions.Wsus.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ILoggerFactory programLoggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder.SetMinimumLevel(LogLevel.Trace).AddConsole());
ILogger<Program> programLogger = programLoggerFactory.CreateLogger<Program>();

// Add services to the container.

builder.Services.AddSingleton(x => new X10sions.Wsus.Pages.Shared._LayoutSettings(typeof(Program).Assembly, "X10sions.Wsus.Web"));

//builder.Services.AddEFCore_Wsus(builder.Configuration, programLoggerFactory);
//builder.Services.AddLinqToDb_Wsus(builder.Configuration, programLoggerFactory);

builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.AddScoped(sp => sp.GetService<IOptionsSnapshot<AppSettings>>().Value);

builder.Services.AddRazorPages();


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseDeveloperExceptionPage();
} else {
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
