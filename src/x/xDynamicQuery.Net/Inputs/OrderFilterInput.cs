using System.Collections.Generic;

namespace DynamicQueryNet.Inputs {
  public class OrderFilterInput {
    public List<FilterInput> Filter { get; set; }
    public List<OrderInput> Order { get; set; }
  }
}
