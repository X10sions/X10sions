using Delta;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.Emit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped(_ => new SqlConnection("connectionString"));
builder.Services.AddScoped(_ => new NpgsqlConnection("connectionString"));

var app = builder.Build();
app.UseDelta();

app.MapGet("/", () => "Hello World!");

app.MapGroup("/group")    .UseDelta()    .MapGet("/", () => "Hello Group!");

app.UseDelta(
  //getConnection: httpContext => httpContext.RequestServices.GetRequiredService<SqlConnection>(),
  getConnection: httpContext => {
    var provider = httpContext.RequestServices;
    var connection = provider.GetRequiredService<SqlConnection>();
    var transaction = provider.GetService<SqlTransaction>();
    return new(connection, transaction);
  },
  shouldExecute: httpContext => {
    var path = httpContext.Request.Path.ToString();
    return path.Contains("match");
  });

app.Run();

static void InitConnectionTypes(Type connectionType, Type transactionType) {
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

static Connection DiscoverConnection(HttpContext httpContext, Type connectionType, Type transactionType) {
  var provider = httpContext.RequestServices;
  var connection = (DbConnection)provider.GetRequiredService(connectionType);
  var transaction = (DbTransaction?)provider.GetService(transactionType);
  return new(connection, transaction);
}


