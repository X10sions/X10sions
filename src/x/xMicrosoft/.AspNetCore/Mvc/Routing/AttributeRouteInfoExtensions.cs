using Common;

namespace Microsoft.AspNetCore.Mvc.Routing {
  public static class AttributeRouteInfoExtensions {

    #region "DebugObject"
    public static AttributeRouteInfoDebugObject GetDebugObject(this AttributeRouteInfo attributeRouteInfo) => new AttributeRouteInfoDebugObject(attributeRouteInfo);
    #endregion
  }

  public class AttributeRouteInfoDebugObject : IDebugObject<AttributeRouteInfo> {
    public AttributeRouteInfoDebugObject(AttributeRouteInfo attributeRouteInfo) {
      this.attributeRouteInfo = attributeRouteInfo;
    }
    AttributeRouteInfo attributeRouteInfo;

    public string Name => attributeRouteInfo.Name;
    public int Order => attributeRouteInfo.Order;
    public string Template => attributeRouteInfo.Template;
    public bool SuppressLinkGeneration => attributeRouteInfo.SuppressLinkGeneration;
    public bool SuppressPathMatching => attributeRouteInfo.SuppressPathMatching;

  }
}
