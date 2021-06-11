using System.Collections.Generic;

namespace System.Data.Linq {
  public interface IMultipleResults : IFunctionResult, IDisposable {
    IEnumerable<TElement> GetResult<TElement>();
  }

}