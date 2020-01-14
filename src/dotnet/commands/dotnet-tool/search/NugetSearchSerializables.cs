// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.DotNet.ToolPackage;

namespace Microsoft.DotNet.Tools.Tool.Search
{
    /// <summary>
    /// All fields are possibly null other than Id, Version, Tags, Authors, Versions
    /// </summary>
    internal class SearchResultPackage
    {
        public SearchResultPackage(
            PackageId id,
            string version,
            string description,
            string summary,
            IReadOnlyCollection<string> tags,
            IReadOnlyCollection<string> authors,
            int totalDownloads,
            bool verified,
            IReadOnlyCollection<SearchResultPackageVersion> versions)
        {
            Id = id;
            LatestVersion = version ?? throw new ArgumentNullException(nameof(version));
            Description = description;
            Summary = summary;
            Tags = tags ?? throw new ArgumentNullException(nameof(tags));
            Authors = authors ?? throw new ArgumentNullException(nameof(authors));
            TotalDownloads = totalDownloads;
            Verified = verified;
            Versions = versions ?? throw new ArgumentNullException(nameof(versions));
        }

        public PackageId Id { get; }
        public string LatestVersion { get; }
        public string Description { get; }
        public string Summary { get; }
        public IReadOnlyCollection<string> Tags { get; }
        public IReadOnlyCollection<string> Authors { get; }
        public int TotalDownloads { get; }
        public bool Verified { get; }
        public IReadOnlyCollection<SearchResultPackageVersion> Versions { get; }
    }

    internal class SearchResultPackageVersion
    {
        public SearchResultPackageVersion(string version, int downloads)
        {
            Version = version ?? throw new ArgumentNullException(nameof(version));
            Downloads = downloads;
        }

        public string Version { get; }
        public int Downloads { get; }
    }
}
