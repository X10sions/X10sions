namespace Common.Data.Entities;

public abstract class EntityBase : IEntity {
  public List<DomainEventBase> Events = new List<DomainEventBase>();
}

public abstract class EntityBase<TId> : EntityBase, IEntityWithId<TId>
  //where TId : IEquatable<TId>
  {
  //[Key] 
  public TId Id { get; set; }
}

public abstract class EntityAuditableBase<TId> : EntityBase<TId>, IEntityAuditableInsert<string>, IEntityAuditableUpdate<string>
  //where TId : IEquatable<TId> 
  {
  public string InsertBy { get; set; } = string.Empty;
  public DateTime InsertDate { get; set; } = DateTime.Now;
  public string UpdateBy { get; set; } = string.Empty;
  public DateTime? UpdateDate { get; set; }
}