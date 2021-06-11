//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics.CodeAnalysis;
//using System.Globalization;
//using System.Text;
//using System.Xml;
//namespace LinqToDB.Tests {

//  public enum RunState {
//    NotRunnable,
//    Runnable,
//    Explicit,
//    Skipped,
//    Ignored
//  }

//  public interface IXmlNodeBuilder {
//    TNode ToXml(bool recursive);
//    TNode AddToXml(TNode parentNode, bool recursive);
//  }

//  public class TNode {
//    #region Constructors
//    public TNode(string name) {
//      Name = name;
//      Attributes = new AttributeDictionary();
//      ChildNodes = new NodeList();
//    }

//    public TNode(string name, string? value) : this(name, value, false) { }

//    public TNode(string name, string? value, bool valueIsCDATA) : this(name) {
//      Value = value;
//      ValueIsCDATA = valueIsCDATA;
//    }

//    #endregion

//    #region Properties
//    public string Name { get; }
//    public string? Value { get; set; }
//    public bool ValueIsCDATA { get; }
//    public AttributeDictionary Attributes { get; }
//    public NodeList ChildNodes { get; }
//    public TNode? FirstChild => ChildNodes.Count == 0 ? null : ChildNodes[0];
//    public string OuterXml {
//      get {
//        var stringWriter = new System.IO.StringWriter();
//        var settings = new XmlWriterSettings {
//          ConformanceLevel = ConformanceLevel.Fragment
//        };

//        using (var xmlWriter = XmlWriter.Create(stringWriter, settings)) {
//          WriteTo(xmlWriter);
//        }

//        return stringWriter.ToString();
//      }
//    }

//    #endregion

//    #region Static Methods

//    public static TNode FromXml(string xmlText) {
//      var doc = new XmlDocument();
//      doc.LoadXml(xmlText);
//      return FromXml(doc.FirstChild);
//    }

//    #endregion

//    #region Instance Methods

//    public TNode AddElement(string name) {
//      var childResult = new TNode(name);
//      ChildNodes.Add(childResult);
//      return childResult;
//    }

//    public TNode AddElement(string name, string value) {
//      var childResult = new TNode(name, EscapeInvalidXmlCharacters(value));
//      ChildNodes.Add(childResult);
//      return childResult;
//    }

//    public TNode AddElementWithCDATA(string name, string value) {
//      var childResult = new TNode(name, EscapeInvalidXmlCharacters(value), true);
//      ChildNodes.Add(childResult);
//      return childResult;
//    }

//    public void AddAttribute(string name, string value) => Attributes.Add(name, EscapeInvalidXmlCharacters(value));

//    public TNode? SelectSingleNode(string xpath) {
//      var nodes = SelectNodes(xpath);

//      return nodes.Count > 0
//          ? nodes[0]
//          : null;
//    }

//    public NodeList SelectNodes(string xpath) {
//      var nodeList = new NodeList {
//        this
//      };

//      return ApplySelection(nodeList, xpath);
//    }

//    public void WriteTo(XmlWriter writer) {
//      writer.WriteStartElement(Name);

//      foreach (var name in Attributes.Keys)
//        writer.WriteAttributeString(name, Attributes[name]);

//      if (Value != null)
//        if (ValueIsCDATA)
//          WriteCDataTo(writer);
//        else
//          writer.WriteString(Value);

//      foreach (var node in ChildNodes)
//        node.WriteTo(writer);

//      writer.WriteEndElement();
//    }

//    #endregion

//    #region Helper Methods

//    private static TNode FromXml(XmlNode xmlNode) {
//      var tNode = new TNode(xmlNode.Name, xmlNode.InnerText);

//      foreach (XmlAttribute attr in xmlNode.Attributes)
//        tNode.AddAttribute(attr.Name, attr.Value);

//      foreach (XmlNode child in xmlNode.ChildNodes)
//        if (child.NodeType == XmlNodeType.Element)
//          tNode.ChildNodes.Add(FromXml(child));

//      return tNode;
//    }

//    private static NodeList ApplySelection(NodeList nodeList, string xpath) {
//      Guard.ArgumentNotNullOrEmpty(xpath, nameof(xpath));
//      if (xpath[0] == '/')
//        throw new ArgumentException("XPath expressions starting with '/' are not supported", nameof(xpath));
//      if (xpath.IndexOf("//") >= 0)
//        throw new ArgumentException("XPath expressions with '//' are not supported", nameof(xpath));

//      var head = xpath;
//      string? tail = null;

//      var slash = xpath.IndexOf('/');
//      if (slash >= 0) {
//        head = xpath.Substring(0, slash);
//        tail = xpath.Substring(slash + 1);
//      }

//      var resultNodes = new NodeList();
//      var filter = new NodeFilter(head);

//      foreach (var node in nodeList)
//        foreach (var childNode in node.ChildNodes)
//          if (filter.Pass(childNode))
//            resultNodes.Add(childNode);

//      return tail != null
//          ? ApplySelection(resultNodes, tail)
//          : resultNodes;
//    }

//    [return: NotNullIfNotNull("str")]
//    private static string? EscapeInvalidXmlCharacters(string? str) {
//      if (str == null) return null;

//      StringBuilder? builder = null;
//      for (var i = 0; i < str.Length; i++) {
//        var c = str[i];
//        if (c > 0x20 && c < 0x7F) {
//          if (builder != null)
//            builder.Append(c);
//        } else if (!(0x0 <= c && c <= 0x8) &&
//              c != 0xB &&
//              c != 0xC &&
//              !(0xE <= c && c <= 0x1F) &&
//              !(0x7F <= c && c <= 0x84) &&
//              !(0x86 <= c && c <= 0x9F) &&
//              !(0xD800 <= c && c <= 0xDFFF) &&
//              c != 0xFFFE &&
//              c != 0xFFFF) {
//          if (builder != null)
//            builder.Append(c);
//        } else if (char.IsHighSurrogate(c) &&
//              i + 1 != str.Length &&
//              char.IsLowSurrogate(str[i + 1])) {
//          if (builder != null) {
//            builder.Append(c);
//            builder.Append(str[i + 1]);
//          }
//          i++;
//        } else {
//          if (builder == null) {
//            builder = new StringBuilder();
//            for (var index = 0; index < i; index++)
//              builder.Append(str[index]);
//          }
//          builder.Append(CharToUnicodeSequence(c));
//        }
//      }

//      if (builder != null)
//        return builder.ToString();
//      else
//        return str;
//    }

//    private static string CharToUnicodeSequence(char symbol) => string.Format("\\u{0}", ((int)symbol).ToString("x4"));

//    private void WriteCDataTo(XmlWriter writer) {
//      var start = 0;
//      var text = Value ?? throw new InvalidOperationException();

//      while (true) {
//        var illegal = text.IndexOf("]]>", start);
//        if (illegal < 0)
//          break;
//        writer.WriteCData(text.Substring(start, illegal - start + 2));
//        start = illegal + 2;
//        if (start >= text.Length)
//          return;
//      }

//      if (start > 0)
//        writer.WriteCData(text.Substring(start));
//      else
//        writer.WriteCData(text);
//    }

//    #endregion

//    #region Nested NodeFilter class

//    class NodeFilter {
//      private readonly string _nodeName;
//      private readonly string? _propName;
//      private readonly string? _propValue;

//      public NodeFilter(string xpath) {
//        _nodeName = xpath;

//        var lbrack = xpath.IndexOf('[');
//        if (lbrack >= 0) {
//          if (!xpath.EndsWith("]"))
//            throw new ArgumentException("Invalid property expression", nameof(xpath));

//          _nodeName = xpath.Substring(0, lbrack);
//          var filter = xpath.Substring(lbrack + 1, xpath.Length - lbrack - 2);

//          var equals = filter.IndexOf('=');
//          if (equals < 0 || filter[0] != '@')
//            throw new ArgumentException("Invalid property expression", nameof(xpath));

//          _propName = filter.Substring(1, equals - 1).Trim();
//          _propValue = filter.Substring(equals + 1).Trim(new char[] { ' ', '"', '\'' });
//        }
//      }

//      public bool Pass(TNode node) {
//        if (node.Name != _nodeName)
//          return false;

//        if (_propName == null)
//          return true;

//        return node.Attributes[_propName] == _propValue;
//      }
//    }

//    #endregion
//  }

//  public class NodeList : System.Collections.Generic.List<TNode> { }

//  public class AttributeDictionary : System.Collections.Generic.Dictionary<string, string> {
//    public new string? this[string key] => TryGetValue(key, out var value) ? value : null;
//  }

//  public interface IPropertyBag : IXmlNodeBuilder {
//    void Add(string key, object value);
//    void Set(string key, object value);
//    object? Get(string key);
//    bool ContainsKey(string key);
//    IList this[string key] { get; set; }
//    ICollection<string> Keys { get; }
//  }

//  public interface ITestData {
//    string? TestName { get; }
//    RunState RunState { get; }
//    object?[] Arguments { get; }
//    IPropertyBag Properties { get; }
//  }

//  public interface ITestCaseData : ITestData {
//    object? ExpectedResult { get; }
//    bool HasExpectedResult { get; }
//  }

//  public interface IApplyToTest {
//    void ApplyToTest(Test test);
//  }

//  [EditorBrowsable(EditorBrowsableState.Never)]
//  public sealed class IgnoredTestCaseData : TestCaseData {
//    #region Instance Fields

//    private RunState _prevRunState;

//    #endregion

//    #region Constructors

//    internal IgnoredTestCaseData(TestCaseData data, RunState prevRunState) {
//      this.Arguments = data.Arguments;
//      this.ArgDisplayNames = data.ArgDisplayNames;
//      this.ExpectedResult = data.ExpectedResult;
//      this.HasExpectedResult = data.HasExpectedResult;
//      this.OriginalArguments = data.OriginalArguments;
//      this.Properties = data.Properties;
//      this.RunState = data.RunState;
//      this.TestName = data.TestName;
//      this._prevRunState = prevRunState;
//    }

//    #endregion

//    #region Fluent Instance Modifiers

//    public TestCaseData Until(DateTimeOffset datetime) {
//      if (_prevRunState != RunState.NotRunnable) {
//        if (datetime > DateTimeOffset.UtcNow) {
//          RunState = RunState.Ignored;
//          string reason = (string)Properties.Get(PropertyNames.SkipReason);
//          Properties.AddIgnoreUntilReason(datetime, reason);
//        } else {
//          RunState = _prevRunState;
//        }
//        Properties.Set(PropertyNames.IgnoreUntilDate, datetime.ToString("u"));
//      }
//      return this;
//    }

//    #endregion

//  }

//  public class TestParameters {
//    private static readonly IFormatProvider MODIFIED_INVARIANT_CULTURE = CreateModifiedInvariantCulture();

//    private readonly Dictionary<string, string> _parameters = new Dictionary<string, string>();

//    public int Count => _parameters.Count;

//    public ICollection<string> Names => _parameters.Keys;

//    public bool Exists(string name) => _parameters.ContainsKey(name);

//    public string? this[string name] => Get(name);

//    public string? Get(string name) => Exists(name) ? _parameters[name] : null;

//    [return: NotNullIfNotNull("defaultValue")]
//    public string? Get(string name, string? defaultValue) => Get(name) ?? defaultValue;

//    [return: NotNullIfNotNull("defaultValue")]
//    public T Get<T>(string name, [MaybeNull] T defaultValue) {
//      var val = Get(name);
//      return val != null ? (T)Convert.ChangeType(val, typeof(T), MODIFIED_INVARIANT_CULTURE) : defaultValue;
//    }

//    internal void Add(string name, string value) => _parameters[name] = value;

//    private static IFormatProvider CreateModifiedInvariantCulture() {
//      var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();

//      culture.NumberFormat.CurrencyGroupSeparator = string.Empty;
//      culture.NumberFormat.NumberGroupSeparator = string.Empty;
//      culture.NumberFormat.PercentGroupSeparator = string.Empty;

//      return culture;
//    }
//  }

//  public class TestCaseParameters : TestParameters, ITestCaseData, IApplyToTest {
//    #region Instance Fields
//    private object? _expectedResult;
//    #endregion
//    #region Constructors
//    public TestCaseParameters() { }
//    public TestCaseParameters(Exception exception) : base(exception) { }
//    public TestCaseParameters(object?[] args) : base(args) { }
//    public TestCaseParameters(ITestCaseData data) : base(data) {
//      if (data.HasExpectedResult)
//        ExpectedResult = data.ExpectedResult;
//    }
//    #endregion
//    #region ITestCaseData Members

//    public object? ExpectedResult {
//      get => _expectedResult;
//      set {
//        _expectedResult = value;
//        HasExpectedResult = true;
//      }
//    }
//    public bool HasExpectedResult { get; set; }
//    #endregion
//  }

//  public class TestCaseData : TestCaseParameters {
//    #region Constructors
//    public TestCaseData(params object?[]? args) : base(args == null ? new object?[] { null } : args) { }
//    public TestCaseData(object? arg) : base(new object?[] { arg }) { }
//    public TestCaseData(object? arg1, object? arg2) : base(new object?[] { arg1, arg2 }) { }
//    public TestCaseData(object? arg1, object? arg2, object? arg3) : base(new object?[] { arg1, arg2, arg3 }) { }
//    #endregion

//    #region Fluent Instance Modifiers

//    public TestCaseData Returns(object? result) {
//      ExpectedResult = result;
//      return this;
//    }

//    public TestCaseData SetName(string? name) {
//      this.TestName = name;
//      return this;
//    }

//    public TestCaseData SetArgDisplayNames(params string[]? displayNames) {
//      ArgDisplayNames = displayNames;
//      return this;
//    }

//    public TestCaseData SetDescription(string description) {
//      this.Properties.Set(PropertyNames.Description, description);
//      return this;
//    }

//    public TestCaseData SetCategory(string category) {
//      this.Properties.Add(PropertyNames.Category, category);
//      return this;
//    }

//    public TestCaseData SetProperty(string propName, string propValue) {
//      this.Properties.Add(propName, propValue);
//      return this;
//    }

//    public TestCaseData SetProperty(string propName, int propValue) {
//      this.Properties.Add(propName, propValue);
//      return this;
//    }

//    public TestCaseData SetProperty(string propName, double propValue) {
//      this.Properties.Add(propName, propValue);
//      return this;
//    }

//    public TestCaseData Explicit() {
//      this.RunState = RunState.Explicit;
//      return this;
//    }

//    public TestCaseData Explicit(string reason) {
//      this.RunState = RunState.Explicit;
//      this.Properties.Set(PropertyNames.SkipReason, reason);
//      return this;
//    }

//    public IgnoredTestCaseData Ignore(string reason) {
//      RunState prevRunState = this.RunState;
//      this.RunState = RunState.Ignored;
//      this.Properties.Set(PropertyNames.SkipReason, reason);
//      var ignoredData = new IgnoredTestCaseData(this, prevRunState);
//      return ignoredData;
//    }

//    #endregion
//  }
//}

