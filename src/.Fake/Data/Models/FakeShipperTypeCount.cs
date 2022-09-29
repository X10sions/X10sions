using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {
  [Table("FakeShipperTypeCount")]
  public class FakeShipperTypeCount {
    public int ShipperTypeId { get; set; }
    public int Total { get; set; }
  }

  }
