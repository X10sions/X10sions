using DynamicQueryNet.Enums;

namespace DynamicQueryNet.Inputs {
  public class OrderInput {
    public string PropertyName { get; set; }
    public OrderTypeEnum Order { get; set; }
  }
}
