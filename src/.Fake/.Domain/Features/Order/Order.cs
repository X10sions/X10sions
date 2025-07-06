using Common.Domain.Entities;

namespace X10sions.Fake.Features.Order;
public class Order : EntityBase<int> {
  public Customer.Customer Customer { get; set; }
  public int CustomerId { get; set; }
  public int EmployeeId { get; set; }
  public DateTime OrderDate { get; set; }
  public DateTime RequiredDate { get; set; }
  public List<OrderDetail.OrderDetail> OrderDetails { get; set; }
}
