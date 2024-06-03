using Common.Data.Entities;

namespace Common.Features.DummyFakeExamples.Order;
public class Order : EntityBase<int> {
  public Customer.Customer Customers { get; set; }
  public int CustomerId { get; set; }
  public int EmployeeId { get; set; }
  public DateTime OrderDate { get; set; }
  public DateTime RequiredDate { get; set; }
  public List<OrderDetail.OrderDetail> OrderDetails { get; set; }
}
