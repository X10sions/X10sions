namespace RCommon.Entities;

public class TransactionalEventsClearedEventArgs : EventArgs {
  public TransactionalEventsClearedEventArgs(IBusinessEntity entity) {
    Entity = entity;
  }

  public IBusinessEntity Entity { get; }
}
