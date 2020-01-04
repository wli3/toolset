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
        private readonly string _searchTerm;
        private readonly bool _isDetailed;

        public ToolSearchCommand(
            AppliedOption options,
            ParseResult result
        )
            : base(result)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _result = result ?? throw new ArgumentNullException(nameof(result));
            _searchTerm = options.Arguments.SingleOrDefault();
            _isDetailed = options.ValueOrDefault<bool>("detail");
        }

        public override int Execute()
        {
            string queryUrl;
            if (_searchTerm == null)
            {
                queryUrl = "https://azuresearch-usnc.dev.nugettest.org/query?q=&packageType=dotnettool"; // TODO only take top 100
            }
            else
            {
                queryUrl = $"https://azuresearch-usnc.dev.nugettest.org/query?q={_searchTerm}&packageType=dotnettool";
            }

            var httpClient = new HttpClient();
            var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            HttpResponseMessage response = httpClient.GetAsync(queryUrl, cancellation.Token).Result;

            var result = response.Content.ReadAsStringAsync().Result;
            var parsed = JsonSerializer.Deserialize<NugetSearchApiContainerSerializable>(result);

            var table = new PrintableTable<NugetSearchApiPackageSerializable>();

            if (!_isDetailed)
            {
                table.AddColumn(
                    "Package ID",
                    p => p.id);
                table.AddColumn(
                    "Latest Version",
                    p => p.version);
                table.AddColumn(
                    "Authors",
                    p => p.authors == null ? "" : string.Join(", ", p.authors));
                table.AddColumn(
                    "Owners",
                    p => p.owners == null ? "" : string.Join(", ", p.owners));
                table.AddColumn(
                    "Downloads",
                    p => p.totalDownloads.ToString());
                table.AddColumn(
                    "Verified",
                    p => p.verified ? "x" : "");

                table.PrintRows(parsed.data, l => Reporter.Output.WriteLine(l));
            }
            else
            {
                foreach (var p in parsed.data)
                {
                    Reporter.Output.WriteLine(p.id);
                    Reporter.Output.WriteLine("\tLatest Version: " + p.version);
                    if (p.authors != null && p.authors.Length != 0)
                    {
                        Reporter.Output.WriteLine("\tAuthors: " + string.Join(", ", p.authors));
                    }
                    
                    if (p.owners != null && p.owners.Length != 0)
                    {
                        Reporter.Output.WriteLine("\tOwners: " + string.Join(", ", p.owners));
                    }

                    Reporter.Output.WriteLine("\tDownloads: " + p.totalDownloads);
                    Reporter.Output.WriteLine("\tVerified: " + p.verified.ToString());
                    Reporter.Output.WriteLine("\tSummary: " + p.summary);
                    Reporter.Output.WriteLine("\tDescription: " + p.description);
                    Reporter.Output.WriteLine();
                }
            }

            return 0;
        }
    }
}
