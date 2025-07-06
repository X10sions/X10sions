namespace X10sions.Examples.EventSourcingMarten.NickChapsasTutorial.Events;

public abstract class Event {
  public abstract Guid StreamId { get; }
  public DateTime CreatedAtUtc { get; set; }
}
