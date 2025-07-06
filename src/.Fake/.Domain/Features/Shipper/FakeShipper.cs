using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Features.Shipper;
//[Alias("FakeShippers")]
[Table("FakeShippers")]
  public class FakeShipper : ServiceStack.Model.IHasId<int> {
    [ServiceStack.DataAnnotations.AutoIncrement][ServiceStack.DataAnnotations.Alias("ShipperID")] public int Id { get; set; }

    [Required][ServiceStack.DataAnnotations.Index(Unique = true)][StringLength(40)] public string CompanyName { get; set; }

    [StringLength(24)] public string Phone { get; set; }

    [ServiceStack.DataAnnotations.References(typeof(FakeShipperType))] public int ShipperTypeId { get; set; }
  }