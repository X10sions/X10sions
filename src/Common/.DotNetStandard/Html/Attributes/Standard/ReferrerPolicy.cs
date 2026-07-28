namespace Common.Html.Attributes {

  public interface IReferrerPolicyHtmlAttribute : IHtmlAttribute<IReferrerPolicyHtmlAttribute.Values?> {
    public enum Values { no_referrer_when_downgrade, no_referrer, origin, origin_when_cross_origin, same_origin, strict_origin_when_cross_origin, unsafe_url }

  };

  /// <summary>Specifies which referrer information to send when the user clicks on the hyperlink</summary>
  public readonly record struct ReferrerPolicyHtmlAttribute(IReferrerPolicyHtmlAttribute.Values? Value) : IReferrerPolicyHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.referrerpolicy;
  }
}