using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {
  [Table("FakeOrder")]
  public class FakeOrder {
    [AutoIncrement]
    public int Id { get; set; }

    [References(typeof(FakeCustomer))]      //Creates Foreign Key
    public int CustomerId { get; set; }

    [References(typeof(FakeEmployee))]      //Creates Foreign Key
    public int EmployeeId { get; set; }

    public FakeAddress ShippingAddress { get; set; } //Blobbed (no Address table)

    public DateTime? OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public int? ShipVia { get; set; }
    public decimal Freight { get; set; }
    public decimal Total { get; set; }
  }


}
