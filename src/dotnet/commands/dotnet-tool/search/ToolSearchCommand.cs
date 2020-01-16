// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using Microsoft.DotNet.Cli;
using Microsoft.DotNet.Cli.CommandLine;
using Microsoft.DotNet.NugetSearch;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.DotNet.Configurer;
using Microsoft.DotNet.ToolPackage;
using Microsoft.DotNet.Tools.Tool.Common;
using Microsoft.Extensions.EnvironmentAbstractions;

namespace Microsoft.DotNet.Tools.Tool.Search
{
    internal class ToolSearchCommand : CommandBase
    {
        private readonly AppliedOption _options;
        private readonly ParseResult _result;
        private readonly INugetSearchApiRequest _nugetSearchApiRequest;

        public ToolSearchCommand(
            AppliedOption options,
            ParseResult result,
            INugetSearchApiRequest nugetSearchApiRequest = null
        )
            : base(result)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _result = result ?? throw new ArgumentNullException(nameof(result));
            _nugetSearchApiRequest = nugetSearchApiRequest ?? new NugetSearchApiRequest();
        }

        public override int Execute()
        {
            var searchTerm = _options.Arguments.Single();
            var isDetailed = _options.ValueOrDefault<bool>("detail");
            var skipString = _options.ValueOrDefault<string>("skip");
            var take = _options.ValueOrDefault<string>("take");
            var prerelease = _options.ValueOrDefault<bool>("prerelease");
            var semverLevel = _options.ValueOrDefault<string>("semver-level");
            //
            // var searchResultPackages =
            //     NugetSearchApiResultDeserializer.Deserialize(_nugetSearchApiRequest.GetResult(searchTerm,));
            var table = new PrintableTable<SearchResultPackage>();

            if (!isDetailed)
            {
                table.AddColumn(
                    "Package ID",
                    p => p.Id.ToString());
                table.AddColumn(
                    "Latest Version",
                    p => p.LatestVersion);
                table.AddColumn(
                    "Authors",
                    p => p.Authors == null ? "" : string.Join(", ", p.Authors));
                table.AddColumn(
                    "Downloads",
                    p => p.TotalDownloads.ToString());
                table.AddColumn(
                    "Verified",
                    p => p.Verified ? "x" : "");

                table.PrintRows(searchResultPackages, l => Reporter.Output.WriteLine(l));
            }
            else
            {
                foreach (var p in searchResultPackages)
                {
                    Reporter.Output.WriteLine(p.Id.ToString());
                    Reporter.Output.WriteLine("\tVersion: " + p.LatestVersion);
                    if (p.Authors != null)
                    {
                        Reporter.Output.WriteLine("\tAuthors: " + string.Join(", ", p.Authors));
                    }

                    Reporter.Output.WriteLine("\tDownloads: " + p.TotalDownloads);
                    Reporter.Output.WriteLine("\tVerified: " + p.Verified.ToString());
                    Reporter.Output.WriteLine("\tSummary: " + p.Summary);
                    Reporter.Output.WriteLine("\tDescription: " + p.Description);
                    Reporter.Output.WriteLine();
                }
            }

            return 0;
        }
    }
}
