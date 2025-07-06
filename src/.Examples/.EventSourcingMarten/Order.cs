namespace X10sions.Examples.EventSourcingMarten;

public class Order {
  public Guid Id { get; set; }
  public string ProductName { get; set; }
  public string DeliveryAddress { get; set; }
  public DateTime? DispatchedAtUtc { get; set; }
  public DateTime? OutForDeliveryAtUtc { get; set; }
  public DateTime? DeliveredAtUtc { get; set; }

  public void Apply(Events.OrderCreated created) {
    ProductName = created.ProductName;
    DeliveryAddress = created.DeliveryAddress;
  }

  public void Apply(Events.OrderDispatched dispatched) {
    DispatchedAtUtc = dispatched.DispatchedAtUtc;
  }

  public void Apply(Events.OrderOutForDelivery outForDelivery) {
    OutForDeliveryAtUtc = outForDelivery.OutForDeliveryAtUtc;
  }

  public void Apply(Events.OrderDelivered delivered) {
    DeliveredAtUtc = delivered.DeliveredAtUtc;
  }

  public void Apply(Events.OrderAddressUpdated updated) {
    DeliveryAddress = updated.DeliveryAddress;
  }
}