﻿using System.Linq.Expressions;

namespace System;

/// <summary>Microsoft.AspNetCore.Mvc.ViewFeatures.Resources.cs</summary>
[PreDotNetCompatibility("https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.ViewFeatures/src/Resources.resx")]
public static class Resources {
  public const string ExpressionHelper_InvalidIndexerExpression = "The expression compiler was unable to evaluate the indexer expression '{0}' because it references the model parameter '{1}' which is unavailable.";
  public const string PropertyOfTypeCannotBeNull = "The '{0}' property of '{1}' must not be null.";

  public static string FormatExpressionHelper_InvalidIndexerExpression(Expression p0, string p1) => string.Format(ExpressionHelper_InvalidIndexerExpression, p0, p1);
  public static string FormatPropertyOfTypeCannotBeNull(string p0, string p1) => string.Format(PropertyOfTypeCannotBeNull, p0, p1);
  //public static void ArgumentNullException_ThrowIfNull(this Expression expression) => if(expression is null) throw new ArgumentNullException(nameof(expression));
}