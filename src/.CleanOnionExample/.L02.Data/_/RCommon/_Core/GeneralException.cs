namespace RCommon;

[Serializable]
public class GeneralException : BaseApplicationException {
  private SeverityOptions _severity = SeverityOptions.High;
  private string _debugMessage = string.Empty;
  private object[] _messageParameters = null;

  public GeneralException() : base() { }

  public GeneralException(string keyMessage) : base(keyMessage) {
    _debugMessage = keyMessage;
  }

  public GeneralException(SeverityOptions severity, string keyMessage) : base(keyMessage) {
    _severity = severity;
    _debugMessage = keyMessage;
  }

  public GeneralException(string keyMessage, params object[] messageParameters) : base(keyMessage) {
    _debugMessage = keyMessage;
    _messageParameters = messageParameters;
  }

  public GeneralException(SeverityOptions severity, string keyMessage, params object[] messageParameters) : base(keyMessage) {
    _severity = severity;
    _debugMessage = keyMessage;
    _messageParameters = messageParameters;
  }

  public GeneralException(string keyMessage, Exception innerException) : base(keyMessage, innerException) {
    _debugMessage = keyMessage;
  }

  public GeneralException(string keyMessage, Exception innerException, params object[] messageParameters)      : base(keyMessage, innerException) {
    _debugMessage = keyMessage;
    _messageParameters = messageParameters;
  }

  public override string Message {
    get {
      return FormatMessage();
    }
  }

  public string DebugMessage {
    get {
      return (_messageParameters == null) ? base.Message : string.Format(base.Message, _messageParameters);
    }
  }

  public SeverityOptions Severity {
    get {
      return _severity;
    }
    set {
      _severity = value;
    }
  }

  private string FormatMessage() {
    return DebugMessage;
  }
}
