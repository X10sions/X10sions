using Microsoft.Internal.ComponentModel;
using System.ComponentModel;

namespace System.Windows {
  /// <summary>Represents an object that participates in the dependency property system.</summary>
  [TypeDescriptionProvider(typeof(DependencyObjectProvider))]
  [NameScopeProperty("NameScope", typeof(NameScope))]
  public class DependencyObject : DispatcherObject {
  }

}