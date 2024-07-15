using System.ComponentModel.DataAnnotations;

namespace X10sions.Fake.Features.Person;

public class GetPersonQuery {
    public int Id { get; set; }
    [Display(Name = "First Name")] public string FirstName { get; set; }
    [Display(Name = "Last Name")] public string LastName { get; set; }
    [Display(Name = "Email")] public string Email { get; set; }
    [Display(Name = "Mobile Number")] public string MobileNo { get; set; }
  }

