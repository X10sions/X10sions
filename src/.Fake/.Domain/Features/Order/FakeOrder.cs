using System.ComponentModel.DataAnnotations.Schema;
using X10sions.Fake.Features.Address;
using X10sions.Fake.Features.Customer;
using X10sions.Fake.Features.Employee;

namespace X10sions.Fake.Features.Order;
[Table("FakeOrder")]
  public class FakeOrder {
    [ServiceStack.DataAnnotations.AutoIncrement]
    public int Id { get; set; }

    [ServiceStack.DataAnnotations.References(typeof(FakeCustomer))]      //Creates Foreign Key
    public int CustomerId { get; set; }

    [ServiceStack.DataAnnotations.References(typeof(FakeEmployee))]      //Creates Foreign Key
    public int EmployeeId { get; set; }

    public FakeAddress ShippingAddress { get; set; } //Blobbed (no Address table)

    public DateTime? OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public int? ShipVia { get; set; }
    public decimal Freight { get; set; }
    public decimal Total { get; set; }
  }
