using System;

namespace Common.Structures {
  struct OrdinalIgnoreCaseString : IEquatable<OrdinalIgnoreCaseString>, IEquatable<string> {
    public OrdinalIgnoreCaseString(string str) {
      _str = str;
    }

    string _str;

    public static implicit operator OrdinalIgnoreCaseString(string str) => new OrdinalIgnoreCaseString(str);
    public static explicit operator string(OrdinalIgnoreCaseString str) => str._str;

    public bool Equals(string other) => _str.Equals(other, StringComparison.OrdinalIgnoreCase);
    public bool Equals(OrdinalIgnoreCaseString other) => _str.Equals(other._str, StringComparison.OrdinalIgnoreCase);

  }
}