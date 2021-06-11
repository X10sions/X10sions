using System.Collections.Generic;

namespace DynamicQueryNet.Inputs {
  public class OrderFilterNonFilterInput : OrderFilterInput {
    public Dictionary<string, string> NonFilter { get; set; }
  }
}
