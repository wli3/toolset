// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DotNet.NugetSearch
{
    internal interface INugetSearchApiRequest
    {
        string GetResult(string searchTerm = null, int? skip = null, int? take = null, bool prerelease = false,
            string semverLevel = null);
    }
}
