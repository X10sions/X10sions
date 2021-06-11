// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Remotion.Linq;

namespace Microsoft.EntityFrameworkCore_v2_2_7.Query.Internal
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public interface IQueryModelGenerator
    {
        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        QueryModel ParseQuery([NotNull] Expression query);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        Expression ExtractParameters(
            [NotNull] IDiagnosticsLogger<DbLoggerCategory.Query> logger,
            [NotNull] Expression query,
            [NotNull] IParameterValues parameterValues,
            bool parameterize = true,
            bool generateContextAccessors = false);
    }
}
