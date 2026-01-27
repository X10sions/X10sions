using Common.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using X10sions.Fake.Features.Address;
using X10sions.Fake.Features.Order;

namespace X10sions.Fake.Features.Customer;
[Table("FakeCustomer")]
public class FakeCustomer : EntityBase<int> {
  [ServiceStack.DataAnnotations.AutoIncrement] // Creates Auto primary key
  public int Id { get; set; }

  public string FirstName { get; set; }
  public string LastName { get; set; }

  [ServiceStack.DataAnnotations.Index(Unique = true)] // Creates Unique Index
  public string Email { get; set; }

  public string CustomerName { get; set; }
  public string ContactName { get; set; }
  public string ContactTitle { get; set; }
  public string Address { get; set; }
  public string City { get; set; }
  public string Region { get; set; }
  public string PostalCode { get; set; }
  public string Country { get; set; }
  public string Phone { get; set; }
  public string Fax { get; set; }



  public Dictionary<FakePhoneType, string> PhoneNumbers { get; set; } = new();  //Blobbed
  public Dictionary<FakeAddressType, FakeAddress> Addresses { get; set; } = new();  //Blobbed

  public DateTime CreatedAt { get; set; }
}

public class CustomerEntity {
  public List<FakeOrder> Orders { get; set; } = new();
}


public enum FakeAddressType { Home, Work, Other, }
public enum FakePhoneType { Home, Work, Mobile, }