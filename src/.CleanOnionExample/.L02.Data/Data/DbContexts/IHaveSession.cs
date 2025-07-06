using NHibernate;

namespace CleanOnionExample.Data.DbContexts;

public interface IHaveSession {
  ISession Session { get; }
}

/*

add-migration v1

update-database


 */