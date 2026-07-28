namespace Common.Html.Attributes {
  public interface IStepHtmlAttribute : IHtmlAttributeDecimal{ };

  public readonly record struct StepHtmlAttribute(decimal? Value) : IStepHtmlAttribute {
    
    public StepHtmlAttribute(double? value):this ((decimal?)value){ }
    public StepHtmlAttribute(int? value):this ((decimal?)value){ }
    public StepHtmlAttribute(long? value):this ((decimal?)value){ }
    public StepHtmlAttribute(short? value):this ((decimal?)value){ }

    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.step;
  }
}