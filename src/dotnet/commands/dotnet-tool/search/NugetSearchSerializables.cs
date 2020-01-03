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
        public NugetSearchApiPackageSerializable[] data { get; set; }
    }

    internal class NugetSearchApiPackageTypesSerializable
    {
        public string name { get; set; }
    }

    internal class NugetSearchApiVersionSerializable
    {
        public string version { get; set; }
    }

    internal class NugetSearchApiPackageSerializable
    {
        public string id { get; set; }
        public string version { get; set; }
        public string description { get; set; }
        public string summary { get; set; }
        public string[] tags { get; set; }
        public string[] owners { get; set; }
        public string[] authors { get; set; }
        public int totalDownloads { get; set; }
        public bool verified { get; set; }
    }
}
