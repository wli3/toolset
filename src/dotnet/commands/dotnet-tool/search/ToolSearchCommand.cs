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
    internal class ToolSearchCommand : CommandBase
    {
        private readonly AppliedOption _options;
        private readonly ParseResult _result;
        private readonly string _searchTerm;

        public ToolSearchCommand(
            AppliedOption options,
            ParseResult result
        )
            : base(result)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _result = result ?? throw new ArgumentNullException(nameof(result));
            _searchTerm = options.Arguments.SingleOrDefault();
        }

        public override int Execute()
        {
            string queryUrl;
            if (_searchTerm == null)
            {
                queryUrl = "https://azuresearch-usnc.dev.nugettest.org/query?q=&packageType=dotnettool";
            }
            else
            {
                queryUrl = $"https://azuresearch-usnc.dev.nugettest.org/query?q{_searchTerm}=&packageType=dotnettool";
            }
            var httpClient = new HttpClient();
            var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            HttpResponseMessage response = httpClient.GetAsync(queryUrl, cancellation.Token).Result;

            var result = response.Content.ReadAsStreamAsync().Result;
            return 0;
        }
    }
}
