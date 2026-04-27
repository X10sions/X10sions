using System.Collections;

namespace Common.AspNetCore.Identity;

public interface IIdentityDatabaseTable<out T> : IEnumerable, IQueryable<T> {

  // Microsoft.EntityFrameworkCore.DbSet
  // LinqToDB.ITable
}
