namespace System.ComponentModel.DataAnnotations.Schema {
  public static class TableAttributeExtensions {

    public static string QualifiedTableName(this TableAttribute tableAttribute, string schemaQualifier = ".") => tableAttribute.Schema.WrapIfNotNullOrWhiteSpace(string.Empty, schemaQualifier, string.Empty) + tableAttribute.Name;

  }
}

