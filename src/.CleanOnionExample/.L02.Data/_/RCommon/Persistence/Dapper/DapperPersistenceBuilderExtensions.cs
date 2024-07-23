using Dapper.FluentMap;
using Dapper.FluentMap.Configuration;

namespace RCommon.Persistence.Dapper;

public static class DapperPersistenceBuilderExtensions {

  public static IDapperBuilder AddFluentMappings(this IDapperBuilder config, Action<FluentMapConfiguration> fluentMapConfig) {
    Guard.Against<DapperFluentMappingsException>(fluentMapConfig == null, "You must configure the fluent mappings options for fluent mappings to be useful");
    FluentMapper.Initialize(fluentMapConfig);
    return config;
  }
}
