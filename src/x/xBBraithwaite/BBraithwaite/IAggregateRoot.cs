namespace BBaithwaite {
  public interface IAggregateRoot<TId> {
    TId Id { get; set; }
  }
}
