using Microsoft.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;

namespace System.Windows.Markup {
  internal static class ReflectionHelper {
    private static Hashtable _loadedAssembliesHash = new Hashtable(8);

    internal static Type GetQualifiedType(string typeName) {
      string[] array = typeName.Split(new char[1]
      {
      ','
      }, 2);
      Type result = null;
      if (array.Length == 1) {
        result = Type.GetType(array[0]);
      } else {
        if (array.Length != 2) {
          throw new InvalidOperationException(SR.Get("QualifiedNameHasWrongFormat", typeName));
        }
        Assembly assembly = null;
        try {
          assembly = LoadAssembly(array[1].TrimStart(), null);
        } catch (Exception ex) {
          if (CriticalExceptions.IsCriticalException(ex)) {
            throw;
          }
          assembly = null;
        }
        if (assembly != null) {
          try {
            result = assembly.GetType(array[0]);
            return result;
          } catch (ArgumentException) {
            assembly = null;
            return result;
          } catch (SecurityException) {
            assembly = null;
            return result;
          }
        }
      }
      return result;
    }

    internal static bool IsNullableType(Type type) {
      if (type.IsGenericType) {
        return type.GetGenericTypeDefinition() == typeof(Nullable<>);
      }
      return false;
    }

    internal static bool IsInternalType(Type type) {
      Type left = type;
      while (type.IsNestedAssembly || type.IsNestedFamORAssem || (left != type && type.IsNestedPublic)) {
        type = type.DeclaringType;
      }
      if (!type.IsNotPublic) {
        if (left != type) {
          return type.IsPublic;
        }
        return false;
      }
      return true;
    }

    internal static bool IsPublicType(Type type) {
      while (type.IsNestedPublic) {
        type = type.DeclaringType;
      }
      return type.IsPublic;
    }

    internal static Type GetSystemType(Type type) {
      return type;
    }

    internal static Type GetReflectionType(object item) {
      if (item == null) {
        return null;
      }
      ICustomTypeProvider customTypeProvider = item as ICustomTypeProvider;
      if (customTypeProvider == null) {
        return item.GetType();
      }
      return customTypeProvider.GetCustomType();
    }

    internal static string GetTypeConverterAttributeData(Type type, out Type converterType) {
      bool attributeDataFound = false;
      return GetCustomAttributeData(type, GetSystemType(typeof(TypeConverterAttribute)), true, ref attributeDataFound, out converterType);
    }

    internal static string GetTypeConverterAttributeData(MemberInfo mi, out Type converterType) {
      return GetCustomAttributeData(mi, GetSystemType(typeof(TypeConverterAttribute)), out converterType);
    }

    private static string GetCustomAttributeData(MemberInfo mi, Type attrType, out Type typeValue) {
      IList<CustomAttributeData> customAttributes = CustomAttributeData.GetCustomAttributes(mi);
      string customAttributeData = GetCustomAttributeData(customAttributes, attrType, out typeValue, true, false);
      if (customAttributeData != null) {
        return customAttributeData;
      }
      return string.Empty;
    }

    private static string GetCustomAttributeData(IList<CustomAttributeData> list, Type attrType, out Type typeValue, bool allowTypeAlso, bool allowZeroArgs) {
      typeValue = null;
      string text = null;
      for (int i = 0; i < list.Count; i++) {
        text = GetCustomAttributeData(list[i], attrType, out typeValue, allowTypeAlso, false, allowZeroArgs);
        if (text != null) {
          break;
        }
      }
      return text;
    }

    internal static string GetCustomAttributeData(Type t, Type attrType, bool allowTypeAlso, ref bool attributeDataFound, out Type typeValue) {
      typeValue = null;
      attributeDataFound = false;
      Type type = t;
      string result = null;
      while (type != null && !attributeDataFound) {
        IList<CustomAttributeData> customAttributes = CustomAttributeData.GetCustomAttributes(type);
        for (int i = 0; i < customAttributes.Count; i++) {
          if (attributeDataFound) {
            break;
          }
          CustomAttributeData customAttributeData = customAttributes[i];
          if (customAttributeData.Constructor.ReflectedType == attrType) {
            attributeDataFound = true;
            result = GetCustomAttributeData(customAttributeData, attrType, out typeValue, allowTypeAlso, false, false);
          }
        }
        if (!attributeDataFound) {
          type = type.BaseType;
        }
      }
      return result;
    }

    private static string GetCustomAttributeData(CustomAttributeData cad, Type attrType, out Type typeValue, bool allowTypeAlso, bool noArgs, bool zeroArgsAllowed) {
      string text = null;
      typeValue = null;
      ConstructorInfo constructor = cad.Constructor;
      if (constructor.ReflectedType == attrType) {
        IList<CustomAttributeTypedArgument> constructorArguments = cad.ConstructorArguments;
        if (constructorArguments.Count == 1 && !noArgs) {
          CustomAttributeTypedArgument customAttributeTypedArgument = constructorArguments[0];
          text = (customAttributeTypedArgument.Value as string);
          if (((text == null) & allowTypeAlso) && customAttributeTypedArgument.ArgumentType == typeof(Type)) {
            typeValue = (customAttributeTypedArgument.Value as Type);
            text = typeValue.AssemblyQualifiedName;
          }
          if (text == null) {
            throw new ArgumentException(SR.Get("ParserAttributeArgsLow", attrType.Name));
          }
        } else {
          if (constructorArguments.Count != 0) {
            throw new ArgumentException(SR.Get("ParserAttributeArgsHigh", attrType.Name));
          }
          if (!(noArgs | zeroArgsAllowed)) {
            throw new ArgumentException(SR.Get("ParserAttributeArgsLow", attrType.Name));
          }
          text = string.Empty;
        }
      }
      return text;
    }

    internal static void ResetCacheForAssembly(string assemblyName) {
      string key = assemblyName.ToUpper(CultureInfo.InvariantCulture);
      _loadedAssembliesHash[key] = null;
    }

    internal static Assembly LoadAssembly(string assemblyName, string assemblyPath) {
      return LoadAssemblyHelper(assemblyName, assemblyPath);
    }

    internal static Assembly GetAlreadyLoadedAssembly(string assemblyNameLookup) {
      return (Assembly)_loadedAssembliesHash[assemblyNameLookup];
    }

    private static Assembly LoadAssemblyHelper(string assemblyGivenName, string assemblyPath) {
      AssemblyName assemblyName = new AssemblyName(assemblyGivenName);
      string name = assemblyName.Name;
      name = name.ToUpper(CultureInfo.InvariantCulture);
      Assembly assembly = (Assembly)_loadedAssembliesHash[name];
      if (assembly != null) {
        if (assemblyName.Version != null) {
          AssemblyName assemblyName2 = new AssemblyName(assembly.FullName);
          if (!AssemblyName.ReferenceMatchesDefinition(assemblyName, assemblyName2)) {
            string text = assemblyName.ToString();
            string text2 = assemblyName2.ToString();
            throw new InvalidOperationException(SR.Get("ParserAssemblyLoadVersionMismatch", text, text2));
          }
        }
      } else {
        if (string.IsNullOrEmpty(assemblyPath)) {
          assembly = SafeSecurityHelper.GetLoadedAssembly(assemblyName);
        }
        if (assembly == null) {
          if (string.IsNullOrEmpty(assemblyPath)) {
            try {
              assembly = Assembly.Load(assemblyGivenName);
            } catch (FileNotFoundException) {
              assembly = null;
            }
          } else {
            assembly = Assembly.LoadFile(assemblyPath);
          }
        }
        if (assembly != null) {
          _loadedAssembliesHash[name] = assembly;
        }
      }
      return assembly;
    }
  }
}