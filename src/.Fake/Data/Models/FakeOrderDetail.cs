using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {
  [Table("FakeOrderDetail")]
  public class FakeOrderDetail {
    [ServiceStack.DataAnnotations.AutoIncrement, LinqToDB.Mapping.Identity]
    public int Id { get; set; }

    [ServiceStack.DataAnnotations.References(typeof(FakeOrder))] //Creates Foreign Key
    public int OrderId { get; set; }

    public int ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; }
    public decimal Discount { get; set; }
  }


}
