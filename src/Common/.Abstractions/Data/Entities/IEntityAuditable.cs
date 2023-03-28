namespace Common.Data.Entities;

public interface IEntityAuditable<TBy> : IEntityAuditableDelete<TBy>, IEntityAuditableInsert<TBy>, IEntityAuditableUpdate<TBy> { }

//public interface IEntityAuditable<TKey> : IEntityWithId<TKey> where TKey : IEquatable<TKey> {
//  string CreatedBy { get; set; }
//  DateTime CreatedOn { get; set; }
//  string LastModifiedBy { get; set; }
//  DateTime? LastModifiedOn { get; set; }
//}

public interface IEntityAuditableDelete<TBy> {
  TBy DeleteBy { get; set; }
  DateTime? DeleteDate { get; set; }
}

public interface IEntityAuditableInsert<TBy> {
  TBy InsertBy { get; set; }
  DateTime InsertDate { get; set; }
}

public interface IEntityAuditableUpdate<TBy> {
  TBy UpdateBy { get; set; }
  DateTime? UpdateDate { get; set; }
}


public interface IEntityAuditableDeleteByName : IEntityAuditableDelete<string> { }
public interface IEntityAuditableInsertByName : IEntityAuditableInsert<string> { }
public interface IEntityAuditableUpdateByName : IEntityAuditableUpdate<string> { }

public interface IEntityAuditableDeleteById : IEntityAuditableDelete<int> { }
public interface IEntityAuditableInsertById : IEntityAuditableInsert<int> { }
public interface IEntityAuditableUpdateById : IEntityAuditableUpdate<int> { }