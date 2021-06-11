using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Microsoft.Internal.ComponentModel {
  internal sealed class DependencyObjectProvider : TypeDescriptionProvider {
    private static readonly UncommonField<IDictionary> _cacheSlot = new UncommonField<IDictionary>(null);

    private static Dictionary<PropertyKey, DependencyObjectPropertyDescriptor> _propertyMap = new Dictionary<PropertyKey, DependencyObjectPropertyDescriptor>();

    private static Dictionary<PropertyKey, DependencyPropertyKind> _propertyKindMap = new Dictionary<PropertyKey, DependencyPropertyKind>();

    private static Hashtable _attachInfoMap = new Hashtable();

    public DependencyObjectProvider()
      : base(TypeDescriptor.GetProvider(typeof(DependencyObject))) {
      TypeDescriptor.Refreshed += delegate (RefreshEventArgs args) {
        if (args.TypeChanged != null && typeof(DependencyObject).IsAssignableFrom(args.TypeChanged)) {
          ClearCache();
          DependencyObjectPropertyDescriptor.ClearCache();
          DPCustomTypeDescriptor.ClearCache();
          DependencyPropertyDescriptor.ClearCache();
        }
      };
    }

  }
