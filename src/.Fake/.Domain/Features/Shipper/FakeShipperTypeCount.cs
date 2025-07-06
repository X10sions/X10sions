using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Features.Shipper;
[Table("FakeShipperTypeCount")]
public class FakeShipperTypeCount {
  public int ShipperTypeId { get; set; }
  public int Total { get; set; }
}
