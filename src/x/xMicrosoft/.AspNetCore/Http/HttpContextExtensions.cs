namespace Microsoft.AspNetCore.Http {
  public static class HttpContextExtensions {

    //public static AppSessionInfo CurrentAppSessionInfo(this HttpContext ctx, bool deleteme) => AppSessionInfo.Current(ctx, deleteme);

    //public static AppSessionInfo GetAppSessionInfo(this HttpContext httpContext) {
    //  var session = httpContext.Session;
    //  AppSessionInfo value = session.GetObjectFromJson<AppSessionInfo>(nameof(AppSessionInfo));
    //  if (value == null) {
    //    value = new AppSessionInfo().Refresh(httpContext);
    //    session.SetObjectAsJson(nameof(AppSessionInfo), value);
    //  }
    //  return value;
    //}

    //public static AppSessionInfo Current(HttpContext httpContext, bool deleteme, AS400Conn db) {
    //  Check.NotNull(httpContext, nameof(httpContext));
    //  var session = httpContext.Session;
    //  var appSessionInfo = session.Get<AppSessionInfo>(nameof(AppSessionInfo), null);
    //  if (appSessionInfo == null) {
    //    appSessionInfo = new AppSessionInfo(httpContext, deleteme, db);
    //    session.Set(nameof(AppSessionInfo), appSessionInfo);
    //    Console.WriteLine($"{DateTime.Now}: new {nameof(AppSessionInfo)}");
    //  }
    //  return appSessionInfo;
    //}

    //public AppSessionInfo(HttpContext httpContext, bool deleteme, AS400Conn db) {
    //  Check.NotNull(httpContext, nameof(httpContext));
    //  HttpContext = httpContext;
    //  using (var conn = new iDB2Connection(db.ConnectionString())) {
    //    conn.Open();
    //    RefreshLogonUser(conn);
    //    conn.Close();
    //  }
    //  AppInfo = httpContext.RequestServices.GetService<AppInfo>();
    //  AppInfo.AppSessionInfos.Add(this);
    //}

    //public static T RequestThenSessionValue<T>(this HttpContext httpContext, string key, T defaultValue) {
    //  var value = httpContext.Items[key].As(httpContext.Session.Get(key).As(defaultValue));
    //  httpContext.Session.Set(key, value);
    //  return value;
    //}

  }
}