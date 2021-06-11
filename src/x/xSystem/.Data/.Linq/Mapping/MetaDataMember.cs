using System.Reflection;

namespace System.Data.Linq.Mapping {
  /// <summary>Represents the mapping between a field or a property of a domain object into a column of a database table.</summary>
  public abstract class MetaDataMember {
    public abstract MetaType DeclaringType { get; }
    public abstract MemberInfo Member { get; }
    public abstract MemberInfo StorageMember { get; }
    public abstract string Name { get; }
    public abstract string MappedName { get; }
    public abstract int Ordinal { get; }
    public abstract Type Type { get; }
    public abstract MetaAccessor MemberAccessor { get; }
    public abstract MetaAccessor StorageAccessor { get; }
    public abstract MetaAccessor DeferredValueAccessor { get; }
    public abstract MetaAccessor DeferredSourceAccessor { get; }
    public abstract bool IsDeferred { get; }
    public abstract bool IsPersistent { get; }
    public abstract bool IsAssociation { get; }
    public abstract bool IsPrimaryKey { get; }
    public abstract bool IsDbGenerated { get; }
    public abstract bool IsVersion { get; }
    public abstract bool IsDiscriminator { get; }
    public abstract bool CanBeNull { get; }
    public abstract string DbType { get; }
    public abstract string Expression { get; }
    public abstract UpdateCheck UpdateCheck { get; }
    public abstract AutoSync AutoSync { get; }
    public abstract MetaAssociation Association { get; }
    public abstract MethodInfo LoadMethod { get; }
    public abstract bool IsDeclaredBy(MetaType type);
  }

}