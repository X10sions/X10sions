using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace X10sions.ERP.Data;
public class ApplicationDbContext : IdentityDbContext {
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options) {
  }
}
