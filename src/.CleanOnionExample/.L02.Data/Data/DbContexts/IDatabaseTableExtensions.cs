using Common.Data;
using LinqToDB;
using Microsoft.EntityFrameworkCore;

namespace CleanOnionExample.Data.DbContexts;

public static class IDatabaseTableExtensions {
  public static DbSet<T> GetDbSet<T, TDatabase>(this IDatabaseTable<T, TDatabase> table) where T : class where TDatabase : IDatabase, IHaveDbContext => table.Database.DbContext.Set<T>();
  public static ITable<T> GetTable<T, TDatabase>(this IDatabaseTable<T, TDatabase> table) where T : class where TDatabase : IDatabase, IHaveDataContext => table.Database.DataContext.GetTable<T>();
}

/*

add-migration v1

update-database


 */