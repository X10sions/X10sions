using RCommon.Persistence.Crud;

namespace X10sions.Fake.Features.Owner;

public interface IOwnerRepository : IReadOnlyRepository<Owner, Guid>, IWriteOnlyRepository<Owner,Guid> { }
