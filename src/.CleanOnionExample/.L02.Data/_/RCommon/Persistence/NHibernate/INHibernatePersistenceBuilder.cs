using NHibernate.Cfg;

namespace RCommon.Persistence.NHibernate;
public interface INHibernatePersistenceBuilder : IPersistenceBuilder {
  INHibernatePersistenceBuilder AddDataConnection<TSession>(string dataStoreName, Configuration options) where TSession : RCommonNHContext;
}