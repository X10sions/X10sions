using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Common.AspNetCore.Identity.Providers.EntityFrameworkCore {
  public class PersonalDataConverter : ValueConverter<string, string> {
    public PersonalDataConverter(IPersonalDataProtector protector) : base(s => protector.Protect(s), s => protector.Unprotect(s), default) { }
  }

}