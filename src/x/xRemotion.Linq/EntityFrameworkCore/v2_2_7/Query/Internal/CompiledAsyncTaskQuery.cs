// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore_v2_2_7.Query.Internal
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class CompiledAsyncTaskQuery<TContext, TResult> : CompiledQueryBase<TContext, Task<TResult>>
        where TContext : DbContext
    {
        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public CompiledAsyncTaskQuery([NotNull] LambdaExpression queryExpression)
            : base(queryExpression)
        {
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Task<TResult> ExecuteAsync(
            [NotNull] TContext context)
            => ExecuteCore(context);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Task<TResult> ExecuteAsync(
            [NotNull] TContext context,
            CancellationToken cancellationToken)
            => ExecuteCore(context, cancellationToken);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Task<TResult> ExecuteAsync<TParam1>(
            [NotNull] TContext context,
            [CanBeNull] TParam1 param1)
            => ExecuteCore(context, param1);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Task<TResult> ExecuteAsync<TParam1>(
            [NotNull] TContext context,
            [CanBeNull] TParam1 param1,
            CancellationToken cancellationToken)
            => ExecuteCore(context, cancellationToken, param1);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Task<TResult> ExecuteAsync<TParam1, TParam2>(
            [NotNull] TContext context,
            [CanBeNull] TParam1 param1,
            [CanBeNull] TParam2 param2)
            => ExecuteCore(context, param1, param2);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Task<TResult> ExecuteAsync<TParam1, TParam2>(
            [NotNull] TContext context,
            [CanBeNull] TParam1 param1,
            [CanBeNull] TParam2 param2,
            CancellationToken cancellationToken)
            => ExecuteCore(context, cancellationToken, param1, param2);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Task<TResult> ExecuteAsync<TParam1, TParam2, TParam3>(
            [NotNull] TContext context,
            [CanBeNull] TParam1 param1,
            [CanBeNull] TParam2 param2,
            [CanBeNull] TParam3 param3)
            => ExecuteCore(context, param1, param2, param3);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Task<TResult> ExecuteAsync<TParam1, TParam2, TParam3>(
            [NotNull] TContext context,
            [CanBeNull] TParam1 param1,
            [CanBeNull] TParam2 param2,
            [CanBeNull] TParam3 param3,
            CancellationToken cancellationToken)
            => ExecuteCore(context, cancellationToken, param1, param2, param3);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Task<TResult> ExecuteAsync<TParam1, TParam2, TParam3, TParam4>(
            [NotNull] TContext context,
            [CanBeNull] TParam1 param1,
            [CanBeNull] TParam2 param2,
            [CanBeNull] TParam3 param3,
            [CanBeNull] TParam4 param4)
            => ExecuteCore(context, param1, param2, param3, param4);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Task<TResult> ExecuteAsync<TParam1, TParam2, TParam3, TParam4>(
            [NotNull] TContext context,
            [CanBeNull] TParam1 param1,
            [CanBeNull] TParam2 param2,
            [CanBeNull] TParam3 param3,
            [CanBeNull] TParam4 param4,
            CancellationToken cancellationToken)
            => ExecuteCore(context, cancellationToken, param1, param2, param3, param4);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Task<TResult> ExecuteAsync<TParam1, TParam2, TParam3, TParam4, TParam5>(
            [NotNull] TContext context,
            [CanBeNull] TParam1 param1,
            [CanBeNull] TParam2 param2,
            [CanBeNull] TParam3 param3,
            [CanBeNull] TParam4 param4,
            [CanBeNull] TParam5 param5)
            => ExecuteCore(context, param1, param2, param3, param4, param5);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Task<TResult> ExecuteAsync<TParam1, TParam2, TParam3, TParam4, TParam5>(
            [NotNull] TContext context,
            [CanBeNull] TParam1 param1,
            [CanBeNull] TParam2 param2,
            [CanBeNull] TParam3 param3,
            [CanBeNull] TParam4 param4,
            [CanBeNull] TParam5 param5,
            CancellationToken cancellationToken)
            => ExecuteCore(context, cancellationToken, param1, param2, param3, param4, param5);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override Func<QueryContext, Task<TResult>> CreateCompiledQuery(
            IQueryCompiler queryCompiler, Expression expression)
            => queryCompiler.CreateCompiledAsyncTaskQuery<TResult>(expression);
    }
}
