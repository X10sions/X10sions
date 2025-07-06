namespace RCommon.Entities;

public interface IAuditedEntity<TCreatedByUser, TLastModifiedByUser>    : IBusinessEntity {
  TCreatedByUser? CreatedBy { get; set; }
  DateTime? DateCreated { get; set; }
  DateTime? DateLastModified { get; set; }
  TLastModifiedByUser? LastModifiedBy { get; set; }
}
