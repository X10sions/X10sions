using Delta;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped(_ => new SqlConnection(connectionString));
builder.Services.AddScoped(_ => new NpgsqlConnection(connectionString));


var app = builder.Build();
app.UseDelta();

app.MapGet("/", () => "Hello World!");

app.MapGroup("/group")    .UseDelta()    .MapGet("/", () => "Hello Group!");

app.UseDelta(
    shouldExecute: httpContext => {
      var path = httpContext.Request.Path.ToString();
      return path.Contains("match");
    });

app.Run();




static void InitConnectionTypes() {
  var sqlConnectionType = Type.GetType("Microsoft.Data.SqlClient.SqlConnection, Microsoft.Data.SqlClient");
  if (sqlConnectionType != null) {
    connectionType = sqlConnectionType;
    transactionType = sqlConnectionType.Assembly.GetType("Microsoft.Data.SqlClient.SqlTransaction")!;
    return;
  }

  var npgsqlConnection = Type.GetType("Npgsql.NpgsqlConnection, Npgsql");
  if (npgsqlConnection != null) {
    connectionType = npgsqlConnection;
    transactionType = npgsqlConnection.Assembly.GetType("Npgsql.NpgsqlTransaction")!;
    return;
  }

  throw new("Could not find connection type. Tried Microsoft.Data.SqlClient.SqlConnection and Npgsql.NpgsqlTransaction");
}

static Connection DiscoverConnection(HttpContext httpContext) {
  var provider = httpContext.RequestServices;
  var connection = (DbConnection)provider.GetRequiredService(connectionType);
  var transaction = (DbTransaction?)provider.GetService(transactionType);
  return new(connection, transaction);
}