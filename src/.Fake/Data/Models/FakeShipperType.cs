using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {
  //[Alias("FakeShipperTypes")]
  [Table("FakeShipperTypes")]
  public class FakeShipperType : ServiceStack.Model.IHasId<int> {

    [ServiceStack.DataAnnotations.AutoIncrement, LinqToDB.Mapping.Identity][ServiceStack.DataAnnotations.Alias("ShipperTypeID")] public int Id { get; set; }
    [Required][ServiceStack.DataAnnotations.Index(Unique = true)][StringLength(40)] public string Name { get; set; }
  }


}
