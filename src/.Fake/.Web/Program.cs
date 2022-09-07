using X10sions.Fake;

var builder = WebApplication.CreateBuilder(args);

var programLoggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder.SetMinimumLevel(LogLevel.Trace).AddConsole());
var programLogger = programLoggerFactory.CreateLogger<Program>();

// Add services to the container.
builder.Services.AddEFCore_Fake(builder.Configuration, programLoggerFactory);
builder.Services.AddLinqToDb_Fake(builder.Configuration, programLogger);

builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
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
