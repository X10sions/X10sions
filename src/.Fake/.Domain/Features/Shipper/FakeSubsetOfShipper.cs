using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Features.Shipper;
[Table("FakeSubsetOfShipper")]
public class FakeSubsetOfShipper {
  public int ShipperId { get; set; }
  public string CompanyName { get; set; }
}