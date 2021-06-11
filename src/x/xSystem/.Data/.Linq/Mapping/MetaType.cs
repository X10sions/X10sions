using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Data.Linq.Mapping {
  public abstract class MetaType {
    public abstract MetaModel Model { get; }
    public abstract MetaTable Table { get; }
    public abstract Type Type { get; }
    public abstract string Name { get; }
    public abstract bool IsEntity { get; }
    public abstract bool CanInstantiate { get; }
    public abstract MetaDataMember DBGeneratedIdentityMember { get; }
    public abstract MetaDataMember VersionMember { get; }
    public abstract MetaDataMember Discriminator { get; }
    public abstract bool HasUpdateCheck { get; }
    public abstract bool HasInheritance { get; }
    public abstract bool HasInheritanceCode { get; }
    public abstract object InheritanceCode { get; }
    public abstract bool IsInheritanceDefault { get; }
    public abstract MetaType InheritanceRoot { get; }
    public abstract MetaType InheritanceBase { get; }
    public abstract MetaType InheritanceDefault { get; }
    public abstract ReadOnlyCollection<MetaType> InheritanceTypes { get; }
    public abstract bool HasAnyLoadMethod { get; }
    public abstract bool HasAnyValidateMethod { get; }
    public abstract ReadOnlyCollection<MetaType> DerivedTypes { get; }
    public abstract ReadOnlyCollection<MetaDataMember> DataMembers { get; }
    public abstract ReadOnlyCollection<MetaDataMember> PersistentDataMembers { get; }
    public abstract ReadOnlyCollection<MetaDataMember> IdentityMembers { get; }
    public abstract ReadOnlyCollection<MetaAssociation> Associations { get; }
    public abstract MethodInfo OnLoadedMethod { get; }
    public abstract MethodInfo OnValidateMethod { get; }
    public abstract MetaType GetInheritanceType(Type type);
    public abstract MetaType GetTypeForInheritanceCode(object code);
    public abstract MetaDataMember GetDataMember(MemberInfo member);
  }

}