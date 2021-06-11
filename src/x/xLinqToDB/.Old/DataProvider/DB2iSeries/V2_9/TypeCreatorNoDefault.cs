using System;

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9 {
  public class TypeCreatorNoDefault<T> : TypeCreatorBase {
      Func<T, object> _creator;
      public dynamic CreateInstance(T value) => (_creator ?? (_creator = GetCreator<T>()))(value);
    }
}
