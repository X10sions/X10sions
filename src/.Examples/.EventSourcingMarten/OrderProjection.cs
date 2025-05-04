using Marten.Events.Aggregation;

namespace X10sions.Examples.EventSourcingMarten;

public class OrderProjection : SingleStreamProjection<Order> {
  public void Apply(Events.OrderCreated created, Order order) {
    order.Id = created.Id;
    order.ProductName = created.ProductName;
    order.DeliveryAddress = created.DeliveryAddress;
  }

  public void Apply(Events.OrderDispatched dispatched, Order order) {
    order.DispatchedAtUtc = dispatched.DispatchedAtUtc;
  }

  public void Apply(Events.OrderOutForDelivery outForDelivery, Order order) {
    order.OutForDeliveryAtUtc = outForDelivery.OutForDeliveryAtUtc;
  }

  public void Apply(Events.OrderDelivered delivered, Order order) {
    order.DeliveredAtUtc = delivered.DeliveredAtUtc;
  }

  public void Apply(Events.OrderAddressUpdated updated, Order order) {
    order.DeliveryAddress = updated.DeliveryAddress;
  }
}