using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Razor.TagHelpers {
  public static class TagHelperContextExtensions {

    public static bool HasContextItem<T>(this TagHelperContext context) => HasContextItem<T>(context, true);
    public static bool HasContextItem<T>(this TagHelperContext context, bool useInherited) => context.HasContextItem(typeof(T), useInherited);
    public static bool HasContextItem(this TagHelperContext context, Type type) => HasContextItem(context, type, true);

    public static bool HasContextItem(this TagHelperContext context, Type type, bool useInherited) {
      if (context == null)
        throw new ArgumentNullException(nameof(context));
      if (type == null)
        throw new ArgumentNullException(nameof(type));
      var contextItem = GetContextItem(context, type, useInherited);
      return contextItem != null && type.IsInstanceOfType(contextItem);
    }

    public static bool HasContextItem<T>(this TagHelperContext context, string key) => HasContextItem(context, typeof(T), key);

    public static bool HasContextItem(this TagHelperContext context, Type type, string key) {
      if (context == null)
        throw new ArgumentNullException(nameof(context));
      if (type == null)
        throw new ArgumentNullException(nameof(type));
      if (key == null)
        throw new ArgumentNullException(nameof(key));
      return context.Items.ContainsKey(key) && type.IsInstanceOfType(context.Items[key]);
    }

    public static T GetContextItem<T>(this TagHelperContext context) where T : class => GetContextItem<T>(context, true);
    public static T GetContextItem<T>(this TagHelperContext context, bool useInherited) where T : class => GetContextItem(context, typeof(T), useInherited) as T;
    public static object GetContextItem(this TagHelperContext context, Type type) => GetContextItem(context, type, true);
    public static T GetContextItem<T>(this TagHelperContext context, string key) where T : class => GetContextItem(context, typeof(T), key) as T;

    public static object GetContextItem(this TagHelperContext context, Type type, string key) {
      if (context == null)
        throw new ArgumentNullException(nameof(context));
      if (type == null)
        throw new ArgumentNullException(nameof(type));
      if (key == null)
        throw new ArgumentNullException(nameof(key));
      return context.Items.ContainsKey(key) && type.IsInstanceOfType(context.Items[key]) ? context.Items[key] : null;
    }

    public static object GetContextItem(this TagHelperContext context, Type type, bool useInherit) {
      if (context.Items.ContainsKey(type))
        return context.Items.First(kVP => kVP.Key.Equals(type)).Value;
      if (useInherit)
        return context.Items.FirstOrDefault(kVP => kVP.Key is Type && type.IsAssignableFrom((Type)kVP.Key)).Value;
      return null;
    }

    public static TagHelperContext SetContextItem<T>(this TagHelperContext context, T contextItem) => SetContextItem(context, typeof(T), contextItem);

    public static TagHelperContext SetContextItem(this TagHelperContext context, Type type, object contextItem) {
      if (context == null)
        throw new ArgumentNullException(nameof(context));
      if (type == null)
        throw new ArgumentNullException(nameof(type));
      if (context.Items.ContainsKey(type))
        context.Items[type] = contextItem;
      else
        context.Items.Add(type, contextItem);
      return context;
    }

    public static TagHelperContext SetContextItem(this TagHelperContext context, string key, object contextItem) {
      if (context == null)
        throw new ArgumentNullException(nameof(context));
      if (key == null)
        throw new ArgumentNullException(nameof(key));
      if (context.Items.ContainsKey(key))
        context.Items[key] = contextItem;
      else
        context.Items.Add(key, contextItem);
      return context;
    }

    public static TagHelperContext RemoveContextItem<T>(this TagHelperContext context) => RemoveContextItem<T>(context, true);
    public static TagHelperContext RemoveContextItem<T>(this TagHelperContext context, bool useInherited) => RemoveContextItem(context, typeof(T), useInherited);
    public static TagHelperContext RemoveContextItem(this TagHelperContext context, Type type) => RemoveContextItem(context, type, true);

    public static TagHelperContext RemoveContextItem(this TagHelperContext context, Type type, bool useInherited) {
      if (context == null)
        throw new ArgumentNullException(nameof(context));
      if (type == null)
        throw new ArgumentNullException(nameof(type));
      if (context.Items.ContainsKey(type))
        context.Items.Remove(type);
      else if (useInherited) {
        var key = context.Items.FirstOrDefault(kVP => kVP.Key is Type && ((Type)kVP.Key).IsAssignableFrom(type));
        if (!key.Equals(default(KeyValuePair<object, object>)))
          context.Items.Remove(key);
      }
      return context;
    }

    public static TagHelperContext RemoveContextItem(this TagHelperContext context, string key) {
      if (context == null)
        throw new ArgumentNullException(nameof(context));
      if (key == null)
        throw new ArgumentNullException(nameof(key));
      if (context.Items.ContainsKey(key))
        context.Items.Remove(key);
      return context;
    }

  }
}