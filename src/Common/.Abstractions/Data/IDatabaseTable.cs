namespace Common.Data;

public interface IDatabaseTable<T> where T : class { }

//public interface IDatabaseTable<T, TDatabase, TTable> : IDatabaseTable<T> where T : class {
//  TTable GetTable(TDatabase db);
//}
