namespace BBaithwaite {
  public abstract class EntityBase : IValidatable {
    private readonly ValidationErrors _validationErrors;

    protected EntityBase() {
      _validationErrors = new ValidationErrors();
    }

    public virtual bool IsValid {
      get {
        _validationErrors.Clear();
        Validate();
        return ValidationErrors.Items.Count == 0;
      }
    }

    public virtual ValidationErrors ValidationErrors => _validationErrors;

    protected virtual void Validate() {
    }
  }
}
