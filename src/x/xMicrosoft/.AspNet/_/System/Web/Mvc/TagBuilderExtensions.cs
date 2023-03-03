namespace System.Web.Mvc {
  public static class TagBuilderExtensions {

    public static TagBuilder MergeAttributeIf(this TagBuilder tagBuilder, bool mergeIfTrue, string key, string value) {
      if (mergeIfTrue) tagBuilder.MergeAttribute(key, value);
      return tagBuilder;
    }

  }
}
