using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Xunit.Extensions {
  public class xAutoMoqDataAttribute : AutoDataAttribute {
    // https://engineering.thetrainline.com/an-introduction-to-xunit-moq-and-autofixture-995315f656f

    public xAutoMoqDataAttribute() : base(new Fixture().Customize(new AutoMoqCustomization())) { }

  }
}