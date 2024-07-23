namespace RCommon.Persistence;

public class DataStoreFactoryOptions {
  public IDictionary<string, Type> Types { get; } = new Dictionary<string, Type>();

  public void Register<T>(string name) where T : IDataStore {
    Types.Add(name, typeof(T));
  }
}
