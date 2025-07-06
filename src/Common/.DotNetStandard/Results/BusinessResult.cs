namespace Common.Results;

public class BusinessResult : IBusinessResult {
  public BusinessResult() {
    Messages = new HashSet<IMessageResult>();
  }

  public ICollection<IMessageResult> Messages { get; }
  public bool Succeeded { get; set; }
  public static BusinessResult Success {
    get {
      var result = new BusinessResult { Succeeded = true };
      result.Messages.Add(new MessageResult { Message = "Operation done successfully", MessageType = MessageResult.Type.Success });
      return result;
    }
  }
}

public interface IBusinessResult {
  ICollection<IMessageResult> Messages { get; }
  bool Succeeded { get; set; }
}

