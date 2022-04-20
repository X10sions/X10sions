using System.Configuration;

namespace Common.Data;

public interface IHaveConnectionStringSettings {
  ConnectionStringSettings ConnectionStringSettings { get; set; }
}
