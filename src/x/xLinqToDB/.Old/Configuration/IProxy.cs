namespace LinqToDB.Configuration {
  public interface IProxy<T> {
    /// <summary>
    /// Proxified object.
    /// </summary>
    T UnderlyingObject { get; }
  }
}
