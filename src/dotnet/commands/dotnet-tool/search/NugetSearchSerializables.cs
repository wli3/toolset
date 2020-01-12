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

    internal class NugetSearchApiPackageTypesSerializable
    {
        public string Name { get; set; }
    }

    internal class NugetSearchApiVersionSerializable
    {
        public string Version { get; set; }
    }

    internal class NugetSearchApiPackageSerializable
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string[] Tags { get; set; }
        public string[] Owners { get; set; }
        public string[] Authors { get; set; }
        public int TotalDownloads { get; set; }
        public bool Verified { get; set; }
    }
}
