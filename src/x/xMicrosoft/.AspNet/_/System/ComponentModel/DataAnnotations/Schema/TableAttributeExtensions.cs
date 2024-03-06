namespace System.ComponentModel.DataAnnotations.Schema {

  public static class TableAttributeExtensions {
    public static string QualifiedName(this TableAttribute @this, string qualifier = ".") => (string.IsNullOrWhiteSpace(@this.Schema) ? string.Empty : (@this.Schema + qualifier)) + @this.Name;
  }
}
