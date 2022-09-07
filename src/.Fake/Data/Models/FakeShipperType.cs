using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {
  //[Alias("FakeShipperTypes")]
  [Table("FakeShipperTypes")]
  public class FakeShipperType    : IHasId<int> {

    [AutoIncrement]    [Alias("ShipperTypeID")]    public int Id { get; set; }
    [Required]    [Index(Unique = true)]    [StringLength(40)]    public string Name { get; set; }
  }


}
