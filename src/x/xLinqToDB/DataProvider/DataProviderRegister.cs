namespace LinqToDB.DataProvider {
  public class DataProviderRegister {

    public static DataProviderRegister Instance = new DataProviderRegister();

    Dictionary<string, IDataProvider> Instances = new Dictionary<string, IDataProvider>();

    public IDataProvider Get(string key, Func<IDataProvider> getter) {
      IDataProvider instance;
      if (!Instances.TryGetValue(key, out instance)) {
        instance = getter();
        Instances[key] = instance;
      }
      return instance;
    }

  }
}
