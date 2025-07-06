using System.ComponentModel.DataAnnotations.Schema;
using Common.Domain;
using X10sions.Fake.Features.Address;

namespace X10sions.Fake.Features.Customer;
[Table("FakeCustomer")]
public class FakeCustomer {
  [ServiceStack.DataAnnotations.AutoIncrement] // Creates Auto primary key
  public int Id { get; set; }

  public string FirstName { get; set; }
  public string LastName { get; set; }

  [ServiceStack.DataAnnotations.Index(Unique = true)] // Creates Unique Index
  public string Email { get; set; }

  public Dictionary<FakePhoneType, string> PhoneNumbers { get; set; } = new();  //Blobbed
  public Dictionary<FakeAddressType, FakeAddress> Addresses { get; set; } = new();  //Blobbed
  public DateTime CreatedAt { get; set; }
}

public enum FakeAddressType {
  Home,
  Work,
  Other,
}

public enum FakePhoneType {
  Home,
  Work,
  Mobile,
}