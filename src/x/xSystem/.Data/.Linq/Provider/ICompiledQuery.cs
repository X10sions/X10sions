namespace System.Data.Linq.Provider {
  internal interface ICompiledQuery {
    IExecuteResult Execute(IProvider provider, object[] arguments);
  }
}