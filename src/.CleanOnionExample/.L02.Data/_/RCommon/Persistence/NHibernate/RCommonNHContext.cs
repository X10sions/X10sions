using NHibernate;
using NHibernate.Cfg;
using System.Data.Common;

namespace RCommon.Persistence.NHibernate;

public abstract class RCommonNHContext : IDataStore {

  public RCommonNHContext( Configuration options) {
    if (options is null) throw new ArgumentNullException(nameof(options));
    //var config = Fluently.Configure().BuildConfiguration();
    var sessionFactory = options.BuildSessionFactory();
    Session =  sessionFactory.OpenSession();
  }

  public async ValueTask DisposeAsync() {
     Session.Dispose();
     //SessionFactory.Dispose();
  }

  //public global::NHibernate.ISessionFactory SessionFactory { get; }
  public ISession Session { get; }
  public DbConnection GetDbConnection() => Session.Connection;
}

