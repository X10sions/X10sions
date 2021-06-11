namespace BBaithwaite {
  public interface IValidatable {
    bool IsValid { get; }
    ValidationErrors ValidationErrors { get; }
  }
}
