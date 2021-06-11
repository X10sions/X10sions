using System.Runtime.Serialization;
using System.Text;

namespace System.Data.Linq {
  [Serializable]
  [DataContract]
  public sealed class Binary : IEquatable<Binary> {
    [DataMember(Name = "Bytes")]
    private byte[] bytes;
    private int? hashCode;
    public int Length => bytes.Length;

    public Binary(byte[] value) {
      if (value == null) {
        bytes = new byte[0];
      } else {
        bytes = new byte[value.Length];
        Array.Copy(value, bytes, value.Length);
      }
      ComputeHash();
    }

    public byte[] ToArray() {
      var array = new byte[bytes.Length];
      Array.Copy(bytes, array, array.Length);
      return array;
    }

    public static implicit operator Binary(byte[] value) => new Binary(value);

    public bool Equals(Binary other) => EqualsTo(other);

    public static bool operator ==(Binary binary1, Binary binary2) {
      if ((object)binary1 == binary2) {
        return true;
      }
      if ((object)binary1 == null && (object)binary2 == null) {
        return true;
      }
      if ((object)binary1 == null || (object)binary2 == null) {
        return false;
      }
      return binary1.EqualsTo(binary2);
    }

    public static bool operator !=(Binary binary1, Binary binary2) {
      if ((object)binary1 == binary2) {
        return false;
      }
      if ((object)binary1 == null && (object)binary2 == null) {
        return false;
      }
      if ((object)binary1 == null || (object)binary2 == null) {
        return true;
      }
      return !binary1.EqualsTo(binary2);
    }

    public override bool Equals(object obj) => EqualsTo(obj as Binary);

    public override int GetHashCode() {
      if (!hashCode.HasValue) {
        ComputeHash();
      }
      return hashCode.Value;
    }

    public override string ToString() {
      var stringBuilder = new StringBuilder();
      stringBuilder.Append("\"");
      stringBuilder.Append(Convert.ToBase64String(bytes, 0, bytes.Length));
      stringBuilder.Append("\"");
      return stringBuilder.ToString();
    }

    private bool EqualsTo(Binary binary) {
      if ((object)this == binary) {
        return true;
      }
      if ((object)binary == null) {
        return false;
      }
      if (bytes.Length != binary.bytes.Length) {
        return false;
      }
      if (GetHashCode() != binary.GetHashCode()) {
        return false;
      }
      var i = 0;
      for (var num = bytes.Length; i < num; i++) {
        if (bytes[i] != binary.bytes[i]) {
          return false;
        }
      }
      return true;
    }

    private void ComputeHash() {
      var num = 314;
      var num2 = 159;
      hashCode = 0;
      for (var i = 0; i < bytes.Length; i++) {
        hashCode = hashCode * num + bytes[i];
        num *= num2;
      }
    }
  }

}