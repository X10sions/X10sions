namespace System.Windows.Markup {
  public interface INameScope {
    /// <summary>Registers the provided name into the current XAML namescope. </summary>
    /// <param name="name">The name to register.</param>
    /// <param name="scopedElement">The specific element that the provided <paramref name="name" /> refers to.</param>
    void RegisterName(string name, object scopedElement);

    /// <summary>Unregisters the provided name from the current XAML namescope. </summary>
    /// <param name="name">The name to unregister.</param>
    void UnregisterName(string name);

    /// <summary>Returns an object that has the provided identifying name. </summary>
    /// <returns>The object, if found. Returns null if no object of that name was found.</returns>
    /// <param name="name">The name identifier for the object being requested.</param>
    object FindName(string name);
  }
}