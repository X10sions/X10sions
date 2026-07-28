namespace Common.Javascript;

public interface IJavascriptInput {
  string Name { get; }
  string ValueSetter { get; }
}

public static class Extensions { }

//public interface IJavascriptInput<T> : IJavascriptInput {
//  T Value { get; }
//}

public readonly record struct JavascriptInput(string Name, string ValueSetter) : IJavascriptInput { }
//  public readonly record struct JavascriptInput<T>(string Name, string ValueSetter): JavascriptInput(Name, ValueSetter) { }

//public interface IJavascriptDialog {
//  string DialogId { get; }
//}

//public interface IJavascriptInputs {
//  IJavascriptInput[] Inputs { get; }
//}

public static class IJavascriptDialogExtensions {
}

//public readonly record struct JavascriptDialog(string Id ) :IJavascriptDialog {  }
//public readonly record struct JavascriptDialogWithInputs(string Id, IJavascriptInput[] Inputs) :IJavascriptDialog, IJavascriptInputs {  }