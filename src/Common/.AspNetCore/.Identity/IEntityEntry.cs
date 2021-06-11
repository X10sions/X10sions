namespace Common.AspNetCore.Identity {
  public interface IEntityEntry {
  }

  public interface IEntityEntry<TEntity> : IEntityEntry where TEntity : class {
  }

}