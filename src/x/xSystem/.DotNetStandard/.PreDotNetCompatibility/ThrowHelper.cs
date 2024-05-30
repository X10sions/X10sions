#if !NET
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System {
  [PreDotNetCompatibility("https://github.com/dotnet/runtime/blob/main/src/libraries/Common/src/System/ThrowHelper.cs")]
  internal static partial class ThrowHelper {
    internal static void ThrowIfNull([NotNull] object? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
      if (argument is null) {
        Throw(paramName);
      }
    }

    [DoesNotReturn]
    private static void Throw(string? paramName) => throw new ArgumentNullException(paramName);

    /// <summary>
    /// Throws either an <see cref="System.ArgumentNullException"/> or an <see cref="System.ArgumentException"/>
    /// if the specified string is <see langword="null"/> or whitespace respectively.
    /// </summary>
    /// <param name="argument">String to be checked for <see langword="null"/> or whitespace.</param>
    /// <param name="paramName">The name of the parameter being checked.</param>
    /// <returns>The original value of <paramref name="argument"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNull]
    public static string IfNullOrWhitespace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string paramName = "") {
      if (argument == null) {
        throw new ArgumentNullException(paramName);
      }
      if (string.IsNullOrWhiteSpace(argument)) {
        if (argument == null) {
          throw new ArgumentNullException(paramName);
        } else {
          throw new ArgumentException(paramName, "Argument is whitespace");
        }
      }
      return argument;
    }
  }

}
#endif