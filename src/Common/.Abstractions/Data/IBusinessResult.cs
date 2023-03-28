namespace Common.Data;

public interface IBusinessResult {
  ICollection<IMessageResult> Messages { get; }
  bool Succeeded { get; set; }
}
