using LinqToDB;

namespace CleanOnionExample.Data.DbContexts;

public interface IHaveDataContext {
  DataContext DataContext { get; }
}

/*

add-migration v1

update-database


 */