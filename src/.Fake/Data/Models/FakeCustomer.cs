using X10sions.Fake.Data.Enums;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {
  [Table("FakeCustomer")]
  public class FakeCustomer {
    [AutoIncrement] // Creates Auto primary key
    public int Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    [Index(Unique = true)] // Creates Unique Index
    public string Email { get; set; }

    public Dictionary<FakePhoneType, string> PhoneNumbers { get; set; } = new();  //Blobbed
    public Dictionary<FakeAddressType, FakeAddress> Addresses { get; set; } = new();  //Blobbed
    public DateTime CreatedAt { get; set; }
  }


}
