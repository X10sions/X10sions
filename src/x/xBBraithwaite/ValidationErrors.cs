using System.Collections.Generic;

namespace BBaithwaite {
  public class ValidationErrors {
    private List<ValidationError> _errors = new List<ValidationError>();

    public IList<ValidationError> Items {
      get {
        return _errors;
      }
    }

    public void Add(string propertyName) {
      _errors.Add(new ValidationError(propertyName, propertyName + " is required."));
    }

    public void Add(string propertyName, string errorMessage) {
      _errors.Add(new ValidationError(propertyName, errorMessage));
    }

    public void Add(ValidationError error) {
      _errors.Add(error);
    }

    public void AddRange(IList<ValidationError> errors) {
      _errors.AddRange(errors);
    }

    internal void Clear() {
      _errors.Clear();
    }
  }
}
