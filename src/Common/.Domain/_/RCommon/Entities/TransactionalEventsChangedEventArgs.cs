using RCommon.EventHandling;

namespace RCommon.Entities;

public class TransactionalEventsChangedEventArgs : EventArgs {
  public TransactionalEventsChangedEventArgs(IBusinessEntity entity, ISerializableEvent eventData) {
    Entity = entity;
    EventData = eventData;
  }

  public IBusinessEntity Entity { get; }
  public ISerializableEvent EventData { get; }
}
