using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq.Expressions;

namespace Microsoft.AspNetCore.Mvc.Rendering;
public static class IHtmlHelperExtensions {

  //public static AssetsHelper Assets(this  IHtmlHelper htmlHelper) { get; } = new AssetsHelper();

  public static T GetService<TModel, T>(this IHtmlHelper<TModel> html, Type type) => (T)html.ViewContext.HttpContext.RequestServices.GetService(type);
  public static T GetService<TModel, T>(this IHtmlHelper<TModel> html) => (T)html.ViewContext.HttpContext.RequestServices.GetService(typeof(T));

  //    public static ModelExpressionProvider GetModelExpressionProvider<TModel>(this IHtmlHelper<TModel> html) => html.GetService<TModel, ModelExpressionProvider>(typeof(ModelExpressionProvider));
  public static ModelExpressionProvider GetModelExpressionProvider<TModel>(this IHtmlHelper<TModel> html) => html.GetService<TModel, ModelExpressionProvider>();

  public static string GetExpressionText<TModel, TResult>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TResult>> expression) => html.GetModelExpressionProvider().GetExpressionText(expression);

  public static IHtmlContent MetaDataFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Func<ModelMetadata, string> property) {
    if (html == null) throw new ArgumentNullException(nameof(html));
    if (expression == null) throw new ArgumentNullException(nameof(expression));
    var modelExpressionProvider = html.GetModelExpressionProvider();
    var modelExplorer = modelExpressionProvider.CreateModelExpression(html.ViewData, expression);
    if (modelExplorer == null) throw new InvalidOperationException($"Failed to get model explorer for {modelExpressionProvider.GetExpressionText(expression)}");
    return new HtmlString(property(modelExplorer.Metadata));
  }

  //public static IHtmlContent PageLink(this IHtmlHelper htmlHelper, string linkText, string pageName) => htmlHelper.RouteLink();

}