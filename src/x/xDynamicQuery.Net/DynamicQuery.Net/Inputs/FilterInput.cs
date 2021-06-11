using DynamicQueryNet.Enums;

namespace DynamicQueryNet.Inputs {
  public class FilterInput {
    public string PropertyName { get; set; }
    public OperationTypeEnum Operation { get; set; }
    public InputTypeEnum Type { get; set; }
    public object Value { get; set; }
  }
}
