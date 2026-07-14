namespace Common;

public interface IJsonFileList<T, TConnection> {
  FileInfo JsonFileInfo { get; }
  List<T> List { get; }
  Task<List<T>> RefreshAsync(TConnection db);
  Type ListType { get; }
  Type ConnectionType { get; }
}