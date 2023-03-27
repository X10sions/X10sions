namespace Common.Data;

public class MessageResult : IMessageResult {
  public string Message { get; set; }
  public MessageType MessageType { get; set; }
}
