using LinqToDB.Mapping;
using System;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_2_3.TB.xMapping {
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public class xAssociationAttribute_TB : Attribute {

    public string Configuration { get; set; }
    public string ThisKey { get; set; }
    public string OtherKey { get; set; }
    public string ExpressionPredicate { get; set; }
    public string Storage { get; set; }
    public bool CanBeNull { get; set; } = true;
    public string KeyName { get; set; }
    public string BackReferenceName { get; set; }
    public bool IsBackReference { get; set; }
    public Relationship Relationship { get; set; }

    public string[] GetThisKeys() => AssociationDescriptor.ParseKeys(ThisKey);
    public string[] GetOtherKeys() => AssociationDescriptor.ParseKeys(OtherKey);
  }
}
