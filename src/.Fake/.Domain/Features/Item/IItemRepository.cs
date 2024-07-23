using RCommon.Persistence.Crud;

namespace X10sions.Fake.Features.Item;

public interface IItemRepository : IReadOnlyRepository<Item, int> { }
