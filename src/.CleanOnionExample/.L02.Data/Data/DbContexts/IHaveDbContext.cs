using Microsoft.EntityFrameworkCore;

namespace CleanOnionExample.Data.DbContexts;

public interface IHaveDbContext {
  DbContext DbContext { get; }
}

/*

add-migration v1

update-database


 */