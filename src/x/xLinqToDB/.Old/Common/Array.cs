//using System;

//namespace LinqToDB.Common {
//  public static class Array<T> {

//    public static readonly T[] Empty = new T[0];

//    public static T[] Append(T[] array, T newElement) {
//      var oldSize = array.Length;
//      Array.Resize(ref array, oldSize + 1);
//      array[oldSize] = newElement;
//      return array;
//    }

//    public static T[] Append(T[] array, T[] otherArray) {
//      if (otherArray == null || otherArray.Length == 0) {
//        return array;
//      }
//      var oldSize = array.Length;
//      Array.Resize(ref array, oldSize + otherArray.Length);
//      for (var i = 0; i < otherArray.Length; i++) {
//        array[oldSize + i] = otherArray[i];
//      }
//      return array;
//    }

//  }
//}