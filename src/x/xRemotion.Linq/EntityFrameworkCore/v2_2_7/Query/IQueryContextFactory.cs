// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.EntityFrameworkCore_v2_2_7.Query
{
    /// <summary>
    ///     Factory for <see cref="QueryContext" /> instances.
    /// </summary>
    public interface IQueryContextFactory
    {
        /// <summary>
        ///     Creates a new QueryContext.
        /// </summary>
        /// <returns>
        ///     A QueryContext instance.
        /// </returns>
        QueryContext Create();
    }
}
