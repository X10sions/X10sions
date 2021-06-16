using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Markup;

namespace Microsoft.Internal.WindowsBase {

  internal static class SafeSecurityHelper {
    private static Dictionary<object, AssemblyName> _assemblies;

    private static object syncObject = new object();

    private static bool _isGCCallbackPending;

    private static readonly WaitCallback _cleanupCollectedAssemblies = CleanupCollectedAssemblies;

    internal const string IMAGE = "image";

    internal static Assembly GetLoadedAssembly(AssemblyName assemblyName) {
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      Version version = assemblyName.Version;
      CultureInfo cultureInfo = assemblyName.CultureInfo;
      byte[] publicKeyToken = assemblyName.GetPublicKeyToken();
      for (int num = assemblies.Length - 1; num >= 0; num--) {
        AssemblyName assemblyName2 = GetAssemblyName(assemblies[num]);
        Version version2 = assemblyName2.Version;
        CultureInfo cultureInfo2 = assemblyName2.CultureInfo;
        byte[] publicKeyToken2 = assemblyName2.GetPublicKeyToken();
        if (string.Compare(assemblyName2.Name, assemblyName.Name, true, TypeConverterHelper.InvariantEnglishUS) == 0 && (version == null || version.Equals(version2)) && (cultureInfo == null || cultureInfo.Equals(cultureInfo2)) && (publicKeyToken == null || IsSameKeyToken(publicKeyToken, publicKeyToken2))) {
          return assemblies[num];
        }
      }
      return null;
    }

    private static AssemblyName GetAssemblyName(Assembly assembly) {
      object key = assembly.IsDynamic ? ((ISerializable)new WeakRefKey(assembly)) : ((ISerializable)assembly);
      lock (syncObject) {
        AssemblyName value;
        if (_assemblies == null) {
          _assemblies = new Dictionary<object, AssemblyName>();
        } else if (_assemblies.TryGetValue(key, out value)) {
          return value;
        }
        value = new AssemblyName(assembly.FullName);
        _assemblies.Add(key, value);
        if (assembly.IsDynamic && !_isGCCallbackPending) {
          GCNotificationToken.RegisterCallback(_cleanupCollectedAssemblies, null);
          _isGCCallbackPending = true;
        }
        return value;
      }
    }

    private static void CleanupCollectedAssemblies(object state) {
      bool flag = false;
      List<object> list = null;
      lock (syncObject) {
        foreach (object key in _assemblies.Keys) {
          WeakReference weakReference = key as WeakReference;
          if (weakReference != null) {
            if (weakReference.IsAlive) {
              flag = true;
            } else {
              if (list == null) {
                list = new List<object>();
              }
              list.Add(key);
            }
          }
        }
        if (list != null) {
          foreach (object item in list) {
            _assemblies.Remove(item);
          }
        }
        if (flag) {
          GCNotificationToken.RegisterCallback(_cleanupCollectedAssemblies, null);
        } else {
          _isGCCallbackPending = false;
        }
      }
    }

    private static bool IsSameKeyToken(byte[] reqKeyToken, byte[] curKeyToken) {
      bool result = false;
      if (reqKeyToken == null && curKeyToken == null) {
        result = true;
      } else if (reqKeyToken != null && curKeyToken != null && reqKeyToken.Length == curKeyToken.Length) {
        result = true;
        for (int i = 0; i < reqKeyToken.Length; i++) {
          if (reqKeyToken[i] != curKeyToken[i]) {
            result = false;
            break;
          }
        }
      }
      return result;
    }
  }

}