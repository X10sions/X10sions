namespace Common.Data;

public interface IMessageResult {
  string Message { get; set; }
  MessageType MessageType { get; set; }
}
