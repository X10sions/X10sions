using Common.Data.Entities;

namespace CleanOnionExample.Data.Entities;

public class Order : EntityBase<int> {
  public Customer Customers { get; set; }
  public int CustomerId { get; set; }
  public int EmployeeId { get; set; }
  public DateTime OrderDate { get; set; }
  public DateTime RequiredDate { get; set; }
  public List<OrderDetail> OrderDetails { get; set; }
}
