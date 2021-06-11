﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.EntityFrameworkCore.Storage
{
    /// <summary>
    ///     Represents a plugin relational type mapping source.
    /// </summary>
    public interface IRelationalTypeMappingSourcePlugin
    {
        /// <summary>
        ///     Finds a type mapping for the given info.
        /// </summary>
        /// <param name="mappingInfo"> The mapping info to use to create the mapping. </param>
        /// <returns> The type mapping, or <c>null</c> if none could be found. </returns>
        RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo);
    }
}
