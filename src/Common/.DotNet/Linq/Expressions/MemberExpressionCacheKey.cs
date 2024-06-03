﻿using System.Linq.Expressions;
using System.Reflection;

namespace Common.Linq.Expressions;

/// <summary> https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.ViewFeatures/src/MemberExpressionCacheKey.cs </summary>
internal readonly struct MemberExpressionCacheKey {
  public MemberExpressionCacheKey(Type modelType, MemberExpression memberExpression) {
    ModelType = modelType;
    MemberExpression = memberExpression;
    Members = null;
  }

  public MemberExpressionCacheKey(Type modelType, MemberInfo[] members) {
    ModelType = modelType;
    Members = members;
    MemberExpression = null;
  }

  // We want to avoid caching a MemberExpression since it has references to other instances in the expression tree.
  // We instead store it as a series of MemberInfo items that comprise of the MemberExpression going from right-most
  // expression to left.
  public MemberExpressionCacheKey MakeCacheable() {
    var members = new List<MemberInfo>();
    foreach (var member in this) {
      members.Add(member);
    }
    return new MemberExpressionCacheKey(ModelType, members.ToArray());
  }

  public MemberExpression? MemberExpression { get; }
  public Type ModelType { get; }
  public MemberInfo[]? Members { get; }
  public Enumerator GetEnumerator() => new Enumerator(this);

  public struct Enumerator(in MemberExpressionCacheKey key) {
    private readonly MemberInfo[]? _members = key.Members;
    private int _index = -1;
    private MemberExpression? _memberExpression = key.MemberExpression;
    public MemberInfo? Current { get; private set; } = null;
    public bool MoveNext() {
      if (_members != null) {
        _index++;
        if (_index >= _members.Length) {
          return false;
        }
        Current = _members[_index];
        return true;
      }
      if (_memberExpression == null) {
        return false;
      }
      Current = _memberExpression.Member;
      _memberExpression = _memberExpression.Expression as MemberExpression;
      return true;
    }
  }
}
