namespace RCommon.Entities;

public interface IAuditedEntity<TKey, TCreatedByUser, TLastModifiedByUser>
    : IAuditedEntity<TCreatedByUser, TLastModifiedByUser>, IBusinessEntity<TKey> {
}
