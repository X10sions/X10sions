namespace Common.Features.DummyFakeExamples.OrderDetail;

public class OrderDetail {
  public int OrderId { get; set; }
  public int ProductId { get; set; }
  public Order.Order Orders { get; set; }
  public Product.Product Product { get; set; }
}
