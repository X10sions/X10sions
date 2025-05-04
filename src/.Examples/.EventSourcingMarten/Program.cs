using Marten;
using Marten.Events.Projections;
using Weasel.Core;
using X10sions.Examples.EventSourcingMarten;
using OrderCreated = X10sions.Examples.EventSourcingMarten.Events.OrderCreated;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMarten(options => {
  options.Connection("Server=localhost;Port=5432;Database=mydb;User ID=workshop;Password=changeme");

  options.UseSystemTextJsonForSerialization();

  // register projection
  options.Projections.Add<OrderProjection>(ProjectionLifecycle.Inline);

  if (builder.Environment.IsDevelopment()) {
    options.AutoCreateSchemaObjects = AutoCreate.All;
  }
});

var app = builder.Build();

app.MapGet("orders/{orderId:guid}", async (IQuerySession session, Guid orderId) => {
  // Load from projections
  var order = await session.LoadAsync<Order>(orderId);

  return order is not null ?
      Results.Ok(order) :
      Results.NotFound();
});

app.MapGet("orders", async (IQuerySession session) => {
  var orders = await session.Query<Order>().ToListAsync();

  return Results.Ok(orders);
});

app.MapPost("orders", async (IDocumentStore store, CreateOrderRequest request) => {
  var order = new OrderCreated {
    ProductName = request.ProductName,
    DeliveryAddress = request.DeliveryAddress
  };

  await using var session = store.LightweightSession();
  session.Events.StartStream<Order>(order.Id, order);
  await session.SaveChangesAsync();

  return Results.Ok(order);
});

app.MapPost("orders/{orderId:guid}/address",
    async (IDocumentStore store, Guid orderId, DeliveryAddressUpdateRequest request) => {
      var addressUpdated = new Events.OrderAddressUpdated {
        Id = orderId,
        DeliveryAddress = request.DeliveryAddress
      };

      await using var session = store.LightweightSession();
      session.Events.Append(orderId, addressUpdated);
      await session.SaveChangesAsync();

      return Results.Ok();
    });

app.MapPost("orders/{orderId:guid}/dispatch", async (IDocumentStore store, Guid orderId) => {
  var orderDispatch = new Events.OrderDispatched {
    Id = orderId,
    DispatchedAtUtc = DateTime.UtcNow
  };

  await using var session = store.LightweightSession();
  session.Events.Append(orderId, orderDispatch);
  await session.SaveChangesAsync();

  return Results.Ok();
});

app.MapPost("orders/{orderId:guid}/outfordelivery", async (IDocumentStore store, Guid orderId) => {
  var orderForDelivery = new Events.OrderOutForDelivery {
    Id = orderId,
    OutForDeliveryAtUtc = DateTime.UtcNow
  };

  await using var session = store.LightweightSession();
  session.Events.Append(orderId, orderForDelivery);
  await session.SaveChangesAsync();

  return Results.Ok();
});

app.MapPost("orders/{orderId:guid}/delivered", async (IDocumentStore store, Guid orderId) => {
  var orderDelivered = new Events.OrderDelivered {
    Id = orderId,
    DeliveredAtUtc = DateTime.UtcNow
  };

  await using var session = store.LightweightSession();
  session.Events.Append(orderId, orderDelivered);
  await session.SaveChangesAsync();

  return Results.Ok();
});

app.Run();