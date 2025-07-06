using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;

namespace NHibernate;

public static class NHibernateExtensions {

  public static IServiceCollection AddNHibernateSession(this IServiceCollection services, Configuration configuration) {
    var sessionFactory = configuration.BuildSessionFactory();
    return services.AddSingleton(sessionFactory)
      .AddScoped(factory => sessionFactory.OpenSession());
  }

}