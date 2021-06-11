namespace CommonOrm {
  public interface IHaveOrmConfig<T> where T:class {
    xIOrmClass<T> GetOrmConfig();
    xIOrmClass<T> OrmConfig{ get; }
  }
}