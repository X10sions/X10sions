using X10sions.Fake.Features.Order;

namespace X10sions.Fake.Features.OrderDetail;

public class OrderDetail {
  public int OrderId { get; set; }
  public int ProductId { get; set; }
  public Order.Order Order { get; set; }
  public Product.Product Product { get; set; }
}
