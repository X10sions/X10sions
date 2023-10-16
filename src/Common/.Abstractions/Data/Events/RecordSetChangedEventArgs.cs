namespace Common.Data.Events;

public class RecordSetChangedEventArgs : EventArgs {
  public Type? RecordType { get; set; }
  public static new RecordSetChangedEventArgs Empty => new RecordSetChangedEventArgs();
  public static RecordSetChangedEventArgs Create<TRecord>() => new RecordSetChangedEventArgs { RecordType = typeof(TRecord) };
  public static RecordSetChangedEventArgs Create(Type recordType) => new RecordSetChangedEventArgs { RecordType = recordType };
}
