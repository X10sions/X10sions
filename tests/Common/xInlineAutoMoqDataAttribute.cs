using AutoFixture.Xunit2;

namespace Xunit.Extensions {
  public class xInlineAutoMoqDataAttribute : CompositeDataAttribute {
    //https://engineering.thetrainline.com/an-introduction-to-xunit-moq-and-autofixture-995315f656f

    public xInlineAutoMoqDataAttribute(params object[] values)
        : base(new InlineDataAttribute(values), new xAutoMoqDataAttribute()) { }

  }
}