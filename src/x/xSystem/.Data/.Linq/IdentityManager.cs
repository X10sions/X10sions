using System.Collections;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using xSystem.Data.Linq.SqlClient;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Data.Linq {
  internal abstract class IdentityManager {
    private class StandardIdentityManager : IdentityManager {
      internal abstract class KeyManager {
        internal abstract Type KeyType {
          get;
        }
      }

      internal abstract class KeyManager<T, K> : KeyManager {
        internal abstract IEqualityComparer<K> Comparer {
          get;
        }

        internal abstract K CreateKeyFromInstance(T instance);

        internal abstract bool TryCreateKeyFromValues(object[] values, out K k);
      }

      internal class SingleKeyManager<T, V> : KeyManager<T, V> {
        private bool isKeyNullAssignable;

        private MetaAccessor<T, V> accessor;

        private int offset;

        private IEqualityComparer<V> comparer;

        internal override Type KeyType => typeof(V);

        internal override IEqualityComparer<V> Comparer {
          get {
            if (comparer == null) {
              comparer = EqualityComparer<V>.Default;
            }
            return comparer;
          }
        }

        internal SingleKeyManager(MetaAccessor<T, V> accessor, int offset) {
          this.accessor = accessor;
          this.offset = offset;
          isKeyNullAssignable = TypeSystem.IsNullAssignable(typeof(V));
        }

        internal override V CreateKeyFromInstance(T instance) => accessor.GetValue(instance);

        internal override bool TryCreateKeyFromValues(object[] values, out V v) {
          var obj = values[offset];
          if (obj == null && !isKeyNullAssignable) {
            v = default;
            return false;
          }
          v = (V)obj;
          return true;
        }
      }

      internal class MultiKeyManager<T, V1, V2> : KeyManager<T, MultiKey<V1, V2>> {
        private MetaAccessor<T, V1> accessor;

        private int offset;

        private KeyManager<T, V2> next;

        private IEqualityComparer<MultiKey<V1, V2>> comparer;

        internal override Type KeyType => typeof(MultiKey<V1, V2>);

        internal override IEqualityComparer<MultiKey<V1, V2>> Comparer {
          get {
            if (comparer == null) {
              comparer = new MultiKey<V1, V2>.Comparer(EqualityComparer<V1>.Default, next.Comparer);
            }
            return comparer;
          }
        }

        internal MultiKeyManager(MetaAccessor<T, V1> accessor, int offset, KeyManager<T, V2> next) {
          this.accessor = accessor;
          this.next = next;
          this.offset = offset;
        }

        internal override MultiKey<V1, V2> CreateKeyFromInstance(T instance) => new MultiKey<V1, V2>(accessor.GetValue(instance), next.CreateKeyFromInstance(instance));

        internal override bool TryCreateKeyFromValues(object[] values, out MultiKey<V1, V2> k) {
          var obj = values[offset];
          if (obj == null && typeof(V1).IsValueType) {
            k = default(MultiKey<V1, V2>);
            return false;
          }
          if (!next.TryCreateKeyFromValues(values, out var k2)) {
            k = default(MultiKey<V1, V2>);
            return false;
          }
          k = new MultiKey<V1, V2>((V1)obj, k2);
          return true;
        }
      }

      internal struct MultiKey<T1, T2> {
        internal class Comparer : IEqualityComparer<MultiKey<T1, T2>>, IEqualityComparer {
          private IEqualityComparer<T1> comparer1;

          private IEqualityComparer<T2> comparer2;

          internal Comparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2) {
            this.comparer1 = comparer1;
            this.comparer2 = comparer2;
          }

          public bool Equals(MultiKey<T1, T2> x, MultiKey<T1, T2> y) {
            if (comparer1.Equals(x.value1, y.value1)) {
              return comparer2.Equals(x.value2, y.value2);
            }
            return false;
          }

          public int GetHashCode(MultiKey<T1, T2> x) => comparer1.GetHashCode(x.value1) ^ comparer2.GetHashCode(x.value2);

          bool IEqualityComparer.Equals(object x, object y) => Equals((MultiKey<T1, T2>)x, (MultiKey<T1, T2>)y);

          int IEqualityComparer.GetHashCode(object x) => GetHashCode((MultiKey<T1, T2>)x);
        }

        private T1 value1;

        private T2 value2;

        internal MultiKey(T1 value1, T2 value2) {
          this.value1 = value1;
          this.value2 = value2;
        }
      }

      internal abstract class IdentityCache {
        internal abstract object Find(object[] keyValues);

        internal abstract object FindLike(object instance);

        internal abstract object InsertLookup(object instance);

        internal abstract bool RemoveLike(object instance);
      }

      internal class IdentityCache<T, K> : IdentityCache {
        internal struct Slot {
          internal int hashCode;

          internal K key;

          internal T value;

          internal int next;
        }

        private int[] buckets;

        private Slot[] slots;

        private int count;

        private int freeList;

        private KeyManager<T, K> keyManager;

        private IEqualityComparer<K> comparer;

        public IdentityCache(KeyManager<T, K> keyManager) {
          this.keyManager = keyManager;
          comparer = keyManager.Comparer;
          buckets = new int[7];
          slots = new Slot[7];
          freeList = -1;
        }

        internal override object InsertLookup(object instance) {
          var value = (T)instance;
          var key = keyManager.CreateKeyFromInstance(value);
          Find(key, ref value, true);
          return value;
        }

        internal override bool RemoveLike(object instance) {
          var instance2 = (T)instance;
          var val = keyManager.CreateKeyFromInstance(instance2);
          var num = comparer.GetHashCode(val) & 0x7FFFFFFF;
          var num2 = num % buckets.Length;
          var num3 = -1;
          for (var num4 = buckets[num2] - 1; num4 >= 0; num4 = slots[num4].next) {
            if (slots[num4].hashCode == num && comparer.Equals(slots[num4].key, val)) {
              if (num3 < 0) {
                buckets[num2] = slots[num4].next + 1;
              } else {
                slots[num3].next = slots[num4].next;
              }
              slots[num4].hashCode = -1;
              slots[num4].value = default(T);
              slots[num4].next = freeList;
              freeList = num4;
              return true;
            }
            num3 = num4;
          }
          return false;
        }

        internal override object Find(object[] keyValues) {
          if (keyManager.TryCreateKeyFromValues(keyValues, out var k)) {
            var value = default(T);
            if (Find(k, ref value, false)) {
              return value;
            }
          }
          return null;
        }

        internal override object FindLike(object instance) {
          var value = (T)instance;
          var key = keyManager.CreateKeyFromInstance(value);
          if (Find(key, ref value, false)) {
            return value;
          }
          return null;
        }

        private bool Find(K key, ref T value, bool add) {
          var num = comparer.GetHashCode(key) & 0x7FFFFFFF;
          for (var num2 = buckets[num % buckets.Length] - 1; num2 >= 0; num2 = slots[num2].next) {
            if (slots[num2].hashCode == num && comparer.Equals(slots[num2].key, key)) {
              value = slots[num2].value;
              return true;
            }
          }
          if (add) {
            int num3;
            if (freeList >= 0) {
              num3 = freeList;
              freeList = slots[num3].next;
            } else {
              if (count == slots.Length) {
                Resize();
              }
              num3 = count;
              count++;
            }
            var num4 = num % buckets.Length;
            slots[num3].hashCode = num;
            slots[num3].key = key;
            slots[num3].value = value;
            slots[num3].next = buckets[num4] - 1;
            buckets[num4] = num3 + 1;
          }
          return false;
        }

        private void Resize() {
          var num = checked(count * 2 + 1);
          var array = new int[num];
          var array2 = new Slot[num];
          Array.Copy(slots, 0, array2, 0, count);
          for (var i = 0; i < count; i++) {
            var num2 = array2[i].hashCode % num;
            array2[i].next = array[num2] - 1;
            array[num2] = i + 1;
          }
          buckets = array;
          slots = array2;
        }
      }

      private Dictionary<MetaType, IdentityCache> caches;

      private IdentityCache currentCache;

      private MetaType currentType;

      internal StandardIdentityManager() {
        caches = new Dictionary<MetaType, IdentityCache>();
      }

      internal override object InsertLookup(MetaType type, object instance) {
        SetCurrent(type);
        return currentCache.InsertLookup(instance);
      }

      internal override bool RemoveLike(MetaType type, object instance) {
        SetCurrent(type);
        return currentCache.RemoveLike(instance);
      }

      internal override object Find(MetaType type, object[] keyValues) {
        SetCurrent(type);
        return currentCache.Find(keyValues);
      }

      internal override object FindLike(MetaType type, object instance) {
        SetCurrent(type);
        return currentCache.FindLike(instance);
      }

      [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
      private void SetCurrent(MetaType type) {
        type = type.InheritanceRoot;
        if (currentType != type) {
          if (!caches.TryGetValue(type, out currentCache)) {
            var keyManager = GetKeyManager(type);
            currentCache = (IdentityCache)Activator.CreateInstance(typeof(IdentityCache<,>).MakeGenericType(type.Type, keyManager.KeyType), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new object[1]
            {
            keyManager
            }, null);
            caches.Add(type, currentCache);
          }
          currentType = type;
        }
      }

      [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
      private static KeyManager GetKeyManager(MetaType type) {
        var count = type.IdentityMembers.Count;
        var metaDataMember = type.IdentityMembers[0];
        var keyManager = (KeyManager)Activator.CreateInstance(typeof(SingleKeyManager<,>).MakeGenericType(type.Type, metaDataMember.Type), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new object[2]
        {
        metaDataMember.StorageAccessor,
        0
        }, null);
        for (var i = 1; i < count; i++) {
          metaDataMember = type.IdentityMembers[i];
          keyManager = (KeyManager)Activator.CreateInstance(typeof(MultiKeyManager<,,>).MakeGenericType(type.Type, metaDataMember.Type, keyManager.KeyType), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new object[3]
          {
          metaDataMember.StorageAccessor,
          i,
          keyManager
          }, null);
        }
        return keyManager;
      }
    }

    private class ReadOnlyIdentityManager : IdentityManager {
      internal ReadOnlyIdentityManager() {
      }

      internal override object InsertLookup(MetaType type, object instance) => instance;

      internal override bool RemoveLike(MetaType type, object instance) => false;

      internal override object Find(MetaType type, object[] keyValues) => null;

      internal override object FindLike(MetaType type, object instance) => null;
    }

    internal abstract object InsertLookup(MetaType type, object instance);

    internal abstract bool RemoveLike(MetaType type, object instance);

    internal abstract object Find(MetaType type, object[] keyValues);

    internal abstract object FindLike(MetaType type, object instance);

    internal static IdentityManager CreateIdentityManager(bool asReadOnly) {
      if (asReadOnly) {
        return new ReadOnlyIdentityManager();
      }
      return new StandardIdentityManager();
    }
  }

}