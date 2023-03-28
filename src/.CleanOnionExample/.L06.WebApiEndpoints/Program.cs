using FastEndpoints;
using FastEndpoints.Swagger;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc();

builder.Services.AddFastEndpoints();

builder.Services.AddDateOnlyConverter();
builder.Services.AddTimeOnlyConverter();

//builder.Services.AddControllers().AddJsonOptions(options => {
//  options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
//  options.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
//});

var app = builder.Build();

//if (app.Environment.IsDevelopment()) {
//  app.UseSwagger();
//  app.UseSwaggerUI();
//}
//app.UseHttpsRedirection();
//app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseOpenApi();
app.UseSwaggerUi3(x => x.ConfigureDefaults());

app.Run();
