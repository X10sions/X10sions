using RCommon.Persistence.Crud;

namespace X10sions.Fake.Features.Owner;

public interface IOwnerRepository : IReadOnlyRepository<FakeOwner, Guid>, IWriteOnlyRepository<FakeOwner, Guid> { }
