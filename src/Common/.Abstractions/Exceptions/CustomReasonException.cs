using System;

namespace Common.Exceptions {
  [Serializable]
  public class CustomReasonException : ApplicationException {

    public CustomReasonException(ReasonCode reason) : this(reason, string.Empty) { }

    public CustomReasonException(ReasonCode reason, string message) : base(message) {
      Reason = reason;
    }

    public CustomReasonException(ReasonCode reason, string message, Exception inner) : base(message, inner) {
      Reason = reason;
    }

    CustomReasonException() { }
    CustomReasonException(string message) : base(message) { }
    CustomReasonException(string message, Exception innerException) : base(message, innerException) { }

    public enum ReasonCode {
      Handled,
      PageUnderConstruction
    }

    public ReasonCode Reason { get; set; }
  }
}