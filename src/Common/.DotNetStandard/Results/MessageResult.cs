namespace Common.Results;

public readonly record struct MessageResult(string Message, MessageResult.Type MessageType) : IMessageResult {

  public enum Type {
    Success = 1,
    Warning = 2,
    Error = 3
  }

}

public interface IMessageResult {
  string Message { get;  }
  MessageResult.Type MessageType { get;  }
}
