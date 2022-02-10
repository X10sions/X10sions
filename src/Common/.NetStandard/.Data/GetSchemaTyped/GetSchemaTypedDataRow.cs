using System.Data;

namespace Common.Data.GetSchemaTyped {
  //[Obsolete("Not Used Yet") ]
  public class GetSchemaTypedDataRow : DataRow {
    internal GetSchemaTypedDataRow(DataRowBuilder builder) : base(builder) { }
    public string CollectionName { get => (string)base[nameof(CollectionName)]; set => base[nameof(CollectionName)] = value; }
    public int NumberOfRestrictions { get => (int)base[nameof(NumberOfRestrictions)]; set => base[nameof(NumberOfRestrictions)] = value; }
    public int NumberOfIdentifierParts { get => (int)base[nameof(NumberOfIdentifierParts)]; set => base[nameof(NumberOfIdentifierParts)] = value; }
  }
}
