using System.ComponentModel.DataAnnotations;

namespace Common.Data;

public abstract class BaseEntity : IEntity {
  public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
}

public abstract class BaseEntity<TId> : BaseEntity, IEntity<TId> where TId : IEquatable<TId> {
  [Key] public TId Id { get; set; }
}

public abstract class BaseEntityAuditable<TId> : BaseEntity<TId>, IEntityAuditable<TId> where TId : IEquatable<TId> {
  public string CreatedBy { get; set; } = string.Empty;
  public DateTime CreatedOn { get; set; } = DateTime.Now;
  public string LastModifiedBy { get; set; } = string.Empty;
  public DateTime? LastModifiedOn { get; set; }
}