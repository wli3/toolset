// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Microsoft.DotNet.Cli;
using Microsoft.DotNet.Cli.CommandLine;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.DotNet.Configurer;
using Microsoft.DotNet.ToolPackage;
using Microsoft.DotNet.Tools.Tool.Common;
using Microsoft.Extensions.EnvironmentAbstractions;

namespace Microsoft.DotNet.Tools.Tool.Search
{
    internal class NugetSearchApiContainerSerializable
    {
        public NugetSearchApiPackageSerializable[] Data { get; set; }
    }

    internal class NugetSearchApiVersionSerializable
    {
        public string Version { get; set; }
        public int Downloads { get; set; }
    }

    internal class NugetSearchApiPackageSerializable
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string[] Tags { get; set; }
        public string[] Authors { get; set; }
        public int TotalDownloads { get; set; }
        public bool Verified { get; set; }
        public NugetSearchApiVersionSerializable[] Versions { get; set; }
    }

    internal class SearchResultPackage
    {
        public SearchResultPackage(PackageId id, string version, string description, string summary, string[] tags,
            string[] authors, int totalDownloads, bool verified, SearchResultPackageVersion[] versions)
        {
            Id = id;
            Version = version ?? throw new ArgumentNullException(nameof(version));
            Description = description;
            Summary = summary;
            Tags = tags ?? throw new ArgumentNullException(nameof(tags));
            Authors = authors ?? throw new ArgumentNullException(nameof(authors));
            TotalDownloads = totalDownloads;
            Verified = verified;
            Versions = versions ?? throw new ArgumentNullException(nameof(versions));
        }

        public PackageId Id { get; }
        public string Version { get; }
        public string Description { get; }
        public string Summary { get; }
        public string[] Tags { get; }
        public string[] Authors { get; }
        public int TotalDownloads { get; }
        public bool Verified { get; }
        public SearchResultPackageVersion[] Versions { get; }
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
