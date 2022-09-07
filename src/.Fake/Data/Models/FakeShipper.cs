using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {
  //[Alias("FakeShippers")]
  [Table("FakeShippers")]
  public class FakeShipper : IHasId<int> {
    [AutoIncrement][Alias("ShipperID")] public int Id { get; set; }

    [Required][Index(Unique = true)][StringLength(40)] public string CompanyName { get; set; }

    [StringLength(24)] public string Phone { get; set; }

    [References(typeof(FakeShipperType))] public int ShipperTypeId { get; set; }
  }


}
