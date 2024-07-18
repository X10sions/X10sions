using Common.Data;

namespace CleanOnionExample.Data.DbContexts;

public class RepositoryManager<T> where T : class {
  public RepositoryManager(IEFCoreRepository<T> efCore, ILinq2DBRepository<T> linqToDb, INHibernateRepository<T> nHibernate) {
    EFCore = efCore;
    LinqToDb = linqToDb;
    NHibernate = nHibernate;
  }

  public IEFCoreRepository<T> EFCore { get; }
  public ILinq2DBRepository<T> LinqToDb { get; }
  public INHibernateRepository<T> NHibernate { get; }

}

/*

add-migration v1

update-database


 */