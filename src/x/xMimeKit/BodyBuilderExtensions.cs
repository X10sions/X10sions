namespace MimeKit;
public static class BodyBuilderExtensions {

  public static BodyBuilder Body(this BodyBuilder builder, string body, bool isHtml = true) {
    if (isHtml) {
      builder.HtmlBody = body;
    } else {
      builder.TextBody = body;
    }
    return builder;
  }

  public static BodyBuilder BodyFromFile(this BodyBuilder builder, string fileName, bool isHtml = true, Func<string, string>? fileContentsFormatter = null) {
    var body = File.ReadAllText(fileName);
    //var body = "";
    // using (var reader = new StreamReader(File.OpenRead(fileName))) {
    //   body = reader.ReadToEnd();
    // }
    return builder.Body(fileContentsFormatter is not null ? fileContentsFormatter(body) : body, isHtml);
  }

  public static BodyBuilder HtmlBodyFromFile(this BodyBuilder builder, string filename, Func<string, string>? fileContentsFormatter = null) => builder.BodyFromFile(filename, true, fileContentsFormatter);
  public static BodyBuilder TextBodyFromFile(this BodyBuilder builder, string filename, Func<string, string>? fileContentsFormatter = null) => builder.BodyFromFile(filename, false, fileContentsFormatter);


  //public static MimeMessage UsingTemplateFromFile<T>(this MimeMessage message, string filename, T model, bool isHtml = true) {
  //  var template = "";
  //  using (var reader = new StreamReader(File.OpenRead(filename))) {
  //    template = reader.ReadToEnd();
  //  }
  //  var result = Renderer.Parse(template, model, isHtml);
  //  message.Body( result, isHtml? TextFormat.Html : TextFormat.Plain);
  //  return message;
  //}

}
