// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.EntityFrameworkCore_v2_2_7.Storage
{
    /// <summary>
    ///     Factory for <see cref="IExecutionStrategy" /> instances.
    /// </summary>
    public interface IExecutionStrategyFactory
    {
        /// <summary>
        ///     Creates a new  <see cref="IExecutionStrategy" />.
        /// </summary>
        /// <returns>An instance of <see cref="IExecutionStrategy" />.</returns>
        IExecutionStrategy Create();
    }
}
