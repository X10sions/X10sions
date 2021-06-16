using Microsoft.Internal.WindowsBase;
using System;
using System.Globalization;

namespace Microsoft.Internal {

  [FriendAccessAllowed]
  internal class TokenizerHelper {
    private char _quoteChar;
    private char _argSeparator;
    private string _str;
    private int _strLen;
    private int _charIndex;
    internal int _currentTokenIndex;
    internal int _currentTokenLength;
    internal bool FoundSeparator { get; private set; }

    internal TokenizerHelper(string str, IFormatProvider formatProvider) {
      char numericListSeparator = GetNumericListSeparator(formatProvider);
      Initialize(str, '\'', numericListSeparator);
    }

    internal TokenizerHelper(string str, char quoteChar, char separator) {
      Initialize(str, quoteChar, separator);
    }

    private void Initialize(string str, char quoteChar, char separator) {
      _str = str;
      _strLen = (str?.Length ?? 0);
      _currentTokenIndex = -1;
      _quoteChar = quoteChar;
      _argSeparator = separator;
      while (_charIndex < _strLen && char.IsWhiteSpace(_str, _charIndex)) {
        _charIndex++;
      }
    }

    internal string GetCurrentToken() {
      if (_currentTokenIndex < 0) {
        return null;
      }
      return _str.Substring(_currentTokenIndex, _currentTokenLength);
    }

    internal void LastTokenRequired() {
      if (_charIndex != _strLen) {
        throw new InvalidOperationException(SR.Get("TokenizerHelperExtraDataEncountered", _charIndex, _str));
      }
    }

    internal bool NextToken() {
      return NextToken(false);
    }

    internal string NextTokenRequired() {
      if (!NextToken(false)) {
        throw new InvalidOperationException(SR.Get("TokenizerHelperPrematureStringTermination", _str));
      }
      return GetCurrentToken();
    }

    internal string NextTokenRequired(bool allowQuotedToken) {
      if (!NextToken(allowQuotedToken)) {
        throw new InvalidOperationException(SR.Get("TokenizerHelperPrematureStringTermination", _str));
      }
      return GetCurrentToken();
    }

    internal bool NextToken(bool allowQuotedToken) {
      return NextToken(allowQuotedToken, _argSeparator);
    }

    internal bool NextToken(bool allowQuotedToken, char separator) {
      _currentTokenIndex = -1;
      FoundSeparator = false;
      if (_charIndex >= _strLen) {
        return false;
      }
      char c = _str[_charIndex];
      int num = 0;
      if (allowQuotedToken && c == _quoteChar) {
        num++;
        _charIndex++;
      }
      int charIndex = _charIndex;
      int num2 = 0;
      while (_charIndex < _strLen) {
        c = _str[_charIndex];
        if (num > 0) {
          if (c == _quoteChar) {
            num--;
            if (num == 0) {
              _charIndex++;
              break;
            }
          }
        } else if (char.IsWhiteSpace(c) || c == separator) {
          if (c == separator) {
            FoundSeparator = true;
          }
          break;
        }
        _charIndex++;
        num2++;
      }
      if (num > 0) {
        throw new InvalidOperationException(SR.Get("TokenizerHelperMissingEndQuote", _str));
      }
      ScanToNextToken(separator);
      _currentTokenIndex = charIndex;
      _currentTokenLength = num2;
      if (_currentTokenLength < 1) {
        throw new InvalidOperationException(SR.Get("TokenizerHelperEmptyToken", _charIndex, _str));
      }
      return true;
    }

    private void ScanToNextToken(char separator) {
      if (_charIndex < _strLen) {
        char c = _str[_charIndex];
        if (c != separator && !char.IsWhiteSpace(c)) {
          throw new InvalidOperationException(SR.Get("TokenizerHelperExtraDataEncountered", _charIndex, _str));
        }
        int num = 0;
        while (_charIndex < _strLen) {
          c = _str[_charIndex];
          if (c == separator) {
            FoundSeparator = true;
            num++;
            _charIndex++;
            if (num > 1) {
              throw new InvalidOperationException(SR.Get("TokenizerHelperEmptyToken", _charIndex, _str));
            }
          } else {
            if (!char.IsWhiteSpace(c)) {
              break;
            }
            _charIndex++;
          }
        }
        if (num > 0 && _charIndex >= _strLen) {
          throw new InvalidOperationException(SR.Get("TokenizerHelperEmptyToken", _charIndex, _str));
        }
      }
    }

    internal static char GetNumericListSeparator(IFormatProvider provider) {
      char c = ',';
      NumberFormatInfo instance = NumberFormatInfo.GetInstance(provider);
      if (instance.NumberDecimalSeparator.Length > 0 && c == instance.NumberDecimalSeparator[0]) {
        c = ';';
      }
      return c;
    }
  }
}
