using System;

namespace Common.AspNetCore.Identity {
  public struct ProtectedDataString : IFormattable {

    public ProtectedDataString(bool value) {
      Value = value.ToString();
    }

    public ProtectedDataString(DateTime value) {
      Value = value.ToString();
    }

    public ProtectedDataString(decimal value) {
      Value = value.ToString();
    }

    public ProtectedDataString(int value) {
      Value = value.ToString();
    }

    public ProtectedDataString(string value) {
      Value = value;
    }

    public string Value { get; set; }

    //public int CYY => Value - 1900;
    //public int CYYMM(int month) => CYY * 100 + month;
    //public int CYYMMDD(int month, int day) => CYYMM(month) * 100 + day;

    //public static implicit operator ProtectedDataString (decimal value) => new ProtectedDataString ((int)value);
    public static implicit operator ProtectedDataString(bool value) => new ProtectedDataString(value);
    public static implicit operator ProtectedDataString(DateTime value) => new ProtectedDataString(value);
    public static implicit operator ProtectedDataString(decimal value) => new ProtectedDataString(value);
    public static implicit operator ProtectedDataString(int value) => new ProtectedDataString(value);
    public static implicit operator ProtectedDataString(string value) => new ProtectedDataString(value);

    #region IFormattable
    public override string ToString() => Value;
    public string ToString(string format, IFormatProvider formatProvider) => ToString(ToString(), formatProvider);
    #endregion

    public static string Protect(string value) => throw new NotImplementedException();
    public static string Unprotect(ProtectedDataString value) => throw new NotImplementedException();

  }
}