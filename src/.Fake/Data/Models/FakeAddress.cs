using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {

  

  [Table("FakeAddress")]
  public class FakeAddress {
    public string Line1 { get; set; }
    public string Line2 { get; set; }
    public string ZipCode { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
  }


}
