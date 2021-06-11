// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Remotion.Linq.Clauses;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace Microsoft.EntityFrameworkCore_v2_2_7.Query.ResultOperators.Internal
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class IncludeExpressionNode : IncludeExpressionNodeBase
    {
        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public static readonly IReadOnlyCollection<MethodInfo> SupportedMethods = new[]
            { EntityFrameworkQueryableExtensions.IncludeMethodInfo };

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IncludeExpressionNode(
            MethodCallExpressionParseInfo parseInfo,
            [NotNull] LambdaExpression navigationPropertyPathLambda)
            : base(parseInfo, navigationPropertyPathLambda)
        {
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override ResultOperatorBase CreateResultOperator(ClauseGenerationContext clauseGenerationContext)
        {
            var prm = Expression.Parameter(typeof(object));
            var pathFromQuerySource = Resolve(prm, prm, clauseGenerationContext);

            if (!NavigationPropertyPathLambda.TryGetComplexPropertyAccess(out var propertyPath))
            {
                throw new InvalidOperationException(
                    CoreStrings.InvalidIncludeLambdaExpression(
                        nameof(EntityFrameworkQueryableExtensions.Include),
                        NavigationPropertyPathLambda));
            }

            var includeResultOperator = new IncludeResultOperator(
                propertyPath.Select(p => p.Name),
                pathFromQuerySource);

            clauseGenerationContext.AddContextInfo(this, includeResultOperator);

            return includeResultOperator;
        }
    }
}
