using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Markup;

namespace System.Windows {
  /// <summary>Implements base WPF support for the <see cref="T:System.Windows.Markup.INameScope" /> methods that store or retrieve name-object mappings into a particular XAML namescope. Adds attached property support to make it simpler to get or set XAML namescope names dynamically at the element level..</summary>
  [TypeForwardedFrom("PresentationFramework, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
  public class NameScope : INameScopeDictionary {
    private class Enumerator : IEnumerator<KeyValuePair<string, object>> {
      private IDictionaryEnumerator _enumerator;

      public KeyValuePair<string, object> Current {
        get {
          if (_enumerator == null) {
            return default(KeyValuePair<string, object>);
          }
          return new KeyValuePair<string, object>((string)_enumerator.Key, _enumerator.Value);
        }
      }

      object IEnumerator.Current {
        get {
          return Current;
        }
      }

      public Enumerator(HybridDictionary nameMap) {
        _enumerator = null;
        if (nameMap != null) {
          _enumerator = nameMap.GetEnumerator();
        }
      }

      public void Dispose() {
        GC.SuppressFinalize(this);
      }

      public bool MoveNext() {
        if (_enumerator == null) {
          return false;
        }
        return _enumerator.MoveNext();
      }

      void IEnumerator.Reset() {
        if (_enumerator != null) {
          _enumerator.Reset();
        }
      }
    }

    /// <summary>Identifies the <see cref="P:System.Windows.NameScope.NameScope" />  attached property. </summary>
    /// <returns>The identifier for the <see cref="P:System.Windows.NameScope.NameScope" /> attached property.</returns>
    public static readonly DependencyProperty NameScopeProperty = DependencyProperty.RegisterAttached("NameScope", typeof(INameScope), typeof(NameScope));

    private HybridDictionary _nameMap;

    /// <summary>Returns the number of items in the collection of mapped names in this <see cref="T:System.Windows.NameScope" />.</summary>
    /// <returns>The number of items in the collection.</returns>
    public int Count {
      get {
        if (_nameMap == null) {
          return 0;
        }
        return _nameMap.Count;
      }
    }

    /// <summary>Gets a value indicating whether the collection is read-only.</summary>
    /// <returns>Always returns false.</returns>
    public bool IsReadOnly => false;

    /// <summary>Gets or sets the item with the specified key. </summary>
    /// <returns>The value of the object mapped by the XAML name provided as <paramref name="key" />.</returns>
    /// <param name="key">The string name for the XAML name mapping to get or set.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="key" /> is provided as null.-or-<paramref name="value" /> is provided as null for a set operation.</exception>
    public object this[string key] {
      get {
        if (key == null) {
          throw new ArgumentNullException("key");
        }
        return FindName(key);
      }
      set {
        if (key == null) {
          throw new ArgumentNullException("key");
        }
        if (value == null) {
          throw new ArgumentNullException("value");
        }
        RegisterName(key, value);
      }
    }

    /// <summary>Gets a collection of the keys in the <see cref="T:System.Windows.NameScope" /> dictionary.</summary>
    /// <returns>A collection of the keys in the <see cref="T:System.Windows.NameScope" /> dictionary.</returns>
    public ICollection<string> Keys {
      get {
        if (_nameMap == null) {
          return null;
        }
        List<string> list = new List<string>();
        foreach (string key in _nameMap.Keys) {
          list.Add(key);
        }
        return list;
      }
    }

    /// <summary>Gets a collection of the values in the <see cref="T:System.Windows.NameScope" /> dictionary.</summary>
    /// <returns>A collection of the values in the <see cref="T:System.Windows.NameScope" /> dictionary.</returns>
    public ICollection<object> Values {
      get {
        if (_nameMap == null) {
          return null;
        }
        List<object> list = new List<object>();
        foreach (object value in _nameMap.Values) {
          list.Add(value);
        }
        return list;
      }
    }

    /// <summary>Registers a new name-object pair into the current XAML namescope.</summary>
    /// <param name="name">The name to use for mapping the given object.</param>
    /// <param name="scopedElement">The object to be mapped to the provided name.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="name" /> or <paramref name="scopedElement" /> was provided as null.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///   <paramref name="name" /> was provided as empty string- or -<paramref name="name" /> provided was rejected by the parser, because it contained characters that are invalid for a XAML name- or -<paramref name="name" /> provided would result in a duplicate name registration.</exception>
    public void RegisterName(string name, object scopedElement) {
      if (name == null) {
        throw new ArgumentNullException("name");
      }
      if (scopedElement == null) {
        throw new ArgumentNullException("scopedElement");
      }
      if (name == string.Empty) {
        throw new ArgumentException(SR.Get("NameScopeNameNotEmptyString"));
      }
      if (!NameValidationHelper.IsValidIdentifierName(name)) {
        throw new ArgumentException(SR.Get("NameScopeInvalidIdentifierName", name));
      }
      if (_nameMap == null) {
        _nameMap = new HybridDictionary {
          [name] = scopedElement
        };
      } else {
        object obj = _nameMap[name];
        if (obj == null) {
          _nameMap[name] = scopedElement;
        } else if (scopedElement != obj) {
          throw new ArgumentException(SR.Get("NameScopeDuplicateNamesNotAllowed", name));
        }
      }
      if (TraceNameScope.IsEnabled) {
        TraceNameScope.TraceActivityItem(TraceNameScope.RegisterName, this, name, scopedElement);
      }
    }

    /// <summary>Removes a name-object mapping from the XAML namescope.</summary>
    /// <param name="name">The name of the mapping to remove.</param>
    /// <exception cref="T:System.ArgumentException">
    ///   <paramref name="name" /> was provided as empty string.- or -<paramref name="name" /> provided had not been registered.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="name" /> is null.</exception>
    public void UnregisterName(string name) {
      if (name == null) {
        throw new ArgumentNullException("name");
      }
      if (name == string.Empty) {
        throw new ArgumentException(SR.Get("NameScopeNameNotEmptyString"));
      }
      if (_nameMap == null || _nameMap[name] == null) {
        throw new ArgumentException(SR.Get("NameScopeNameNotFound", name));
      }
      _nameMap.Remove(name);
      if (TraceNameScope.IsEnabled) {
        TraceNameScope.TraceActivityItem(TraceNameScope.UnregisterName, this, name);
      }
    }

    /// <summary>Returns the corresponding object in the XAML namescope maintained by this <see cref="T:System.Windows.NameScope" />, based on a provided name string.</summary>
    /// <returns>The requested object that is mapped with <paramref name="name" />. Can return null if <paramref name="name" /> was provided as null or empty string, or if no matching object was found.</returns>
    /// <param name="name">Name portion of an existing mapping to retrieve the object portion for.</param>
    public object FindName(string name) {
      if (_nameMap == null || name == null || name == string.Empty) {
        return null;
      }
      return _nameMap[name];
    }

    internal static INameScope NameScopeFromObject(object obj) {
      INameScope nameScope = obj as INameScope;
      if (nameScope == null) {
        DependencyObject dependencyObject = obj as DependencyObject;
        if (dependencyObject != null) {
          nameScope = GetNameScope(dependencyObject);
        }
      }
      return nameScope;
    }

    /// <summary>Provides the attached property set accessor for the <see cref="P:System.Windows.NameScope.NameScope" /> attached property.</summary>
    /// <param name="dependencyObject">Object to change XAML namescope for.</param>
    /// <param name="value">The new XAML namescope, using an interface cast.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="dependencyObject" /> is null.</exception>
    public static void SetNameScope(DependencyObject dependencyObject, INameScope value) {
      if (dependencyObject == null) {
        throw new ArgumentNullException("dependencyObject");
      }
      dependencyObject.SetValue(NameScopeProperty, value);
    }

    /// <summary>Provides the attached property get accessor for the <see cref="P:System.Windows.NameScope.NameScope" /> attached property.</summary>
    /// <returns>A XAML namescope, as an <see cref="T:System.Windows.Markup.INameScope" /> instance.</returns>
    /// <param name="dependencyObject">The object to get the XAML namescope from.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="dependencyObject" /> is null.</exception>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public static INameScope GetNameScope(DependencyObject dependencyObject) {
      if (dependencyObject == null) {
        throw new ArgumentNullException("dependencyObject");
      }
      return (INameScope)dependencyObject.GetValue(NameScopeProperty);
    }

    private IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
      return new Enumerator(_nameMap);
    }

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An enumerator that iterates through a collection.</returns>
    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator() {
      return GetEnumerator();
    }

    /// <summary>Removes all items from the collection.</summary>
    public void Clear() {
      _nameMap = null;
    }

    /// <summary>Copies the elements of the collection to an array, starting at a particular array index.</summary>
    /// <param name="array">The one-dimensional array that is the destination of the elements copied from the collection The array must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) {
      if (_nameMap == null) {
        array = null;
      } else {
        IDictionaryEnumerator enumerator = _nameMap.GetEnumerator();
        try {
          while (enumerator.MoveNext()) {
            DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
            array[arrayIndex++] = new KeyValuePair<string, object>((string)dictionaryEntry.Key, dictionaryEntry.Value);
          }
        } finally {
          (enumerator as IDisposable)?.Dispose();
        }
      }
    }

    /// <summary>Removes the specific object from the collection.</summary>
    /// <returns>true if item was successfully removed from the collection, otherwise false. Also returns false if the item was not found in the collection.</returns>
    /// <param name="item">The object to remove from the collection, specified as a <see cref="T:System.Collections.Generic.KeyValuePair`2" />  (key is <see cref="T:System.String" />, value is <see cref="T:System.Object" />).</param>
    public bool Remove(KeyValuePair<string, object> item) {
      if (!Contains(item)) {
        return false;
      }
      if (item.Value != this[item.Key]) {
        return false;
      }
      return Remove(item.Key);
    }

    /// <summary>Adds an item to the collection.</summary>
    /// <param name="item">A <see cref="T:System.Collections.Generic.KeyValuePair`2" />  (key is <see cref="T:System.String" />, value is <see cref="T:System.Object" />) that represents the name mapping to add to the XAML namescope.</param>
    /// <exception cref="T:System.ArgumentException">Either or both components of <paramref name="item" /> are null.</exception>
    public void Add(KeyValuePair<string, object> item) {
      if (item.Key == null) {
        throw new ArgumentException(SR.Get("ReferenceIsNull", "item.Key"), "item");
      }
      if (item.Value == null) {
        throw new ArgumentException(SR.Get("ReferenceIsNull", "item.Value"), "item");
      }
      Add(item.Key, item.Value);
    }

    /// <summary>Determines whether the collection contains a specified item. </summary>
    /// <returns>true if the specified <see cref="T:System.Collections.Generic.KeyValuePair`2" /> identifies an existing mapping in this <see cref="T:System.Windows.NameScope" /> . false if the specified <see cref="T:System.Collections.Generic.KeyValuePair`2" /> does not exist in the current <see cref="T:System.Windows.NameScope" />.</returns>
    /// <param name="item">The item to find in the collection, specified as a <see cref="T:System.Collections.Generic.KeyValuePair`2" />  (key is <see cref="T:System.String" />, value is <see cref="T:System.Object" />).</param>
    /// <exception cref="T:System.ArgumentException">
    ///   <paramref name="key" /> is null.</exception>
    public bool Contains(KeyValuePair<string, object> item) {
      if (item.Key == null) {
        throw new ArgumentException(SR.Get("ReferenceIsNull", "item.Key"), "item");
      }
      return ContainsKey(item.Key);
    }

    /// <summary>Adds an item to the collection.</summary>
    /// <param name="key">The string key, which is the name of the XAML namescope mapping to add.</param>
    /// <param name="value">The object value, which is the object reference of the XAML namescope mapping to add.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="key" /> or <paramref name="value" /> is null.</exception>
    public void Add(string key, object value) {
      if (key == null) {
        throw new ArgumentNullException("key");
      }
      RegisterName(key, value);
    }

    /// <summary>Returns whether a provided name already exists in this <see cref="T:System.Windows.NameScope" />.</summary>
    /// <returns>true if the specified <paramref name="key" /> identifies a name for an existing mapping in this <see cref="T:System.Windows.NameScope" />. false if the specified <paramref name="key" /> does not exist in the current <see cref="T:System.Windows.NameScope" />.</returns>
    /// <param name="key">The string key to find.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="key" /> is null.</exception>
    public bool ContainsKey(string key) {
      if (key == null) {
        throw new ArgumentNullException("key");
      }
      object obj = FindName(key);
      return obj != null;
    }

    /// <summary>Removes a mapping for a specified name from the collection.</summary>
    /// <returns>true if item was successfully removed from the collection, otherwise false. Also returns false if the item was not found in the collection.</returns>
    /// <param name="key">The string key, which is the name of the XAML namescope mapping to remove.</param>
    public bool Remove(string key) {
      if (!ContainsKey(key)) {
        return false;
      }
      UnregisterName(key);
      return true;
    }

    /// <summary>Gets the value associated with the specified key.</summary>
    /// <returns>true if the <see cref="T:System.Windows.NameScope" /> contains a mapping for the name provided as <paramref name="key" />. Otherwise, false.</returns>
    /// <param name="key">The key of the value to get.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, a null object. This parameter is passed uninitialized.</param>
    public bool TryGetValue(string key, out object value) {
      if (!ContainsKey(key)) {
        value = null;
        return false;
      }
      value = FindName(key);
      return true;
    }
  }

}
