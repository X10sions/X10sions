namespace Common.Data;

public class BusinessResult : IBusinessResult {
  public BusinessResult() {
    Messages = new HashSet<IMessageResult>();//!!!
  }

  public ICollection<IMessageResult> Messages { get; }
  public bool Succeeded { get; set; }
  public static BusinessResult Success {
    get {
      var result = new BusinessResult { Succeeded = true };
      result.Messages.Add(new MessageResult { Message = "Operation done successfully", MessageType = MessageType.Success });
      return result;
    }
  }
}
