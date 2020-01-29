// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.DotNet.Cli;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.DotNet.ToolPackage;

namespace Microsoft.DotNet.Tools.Tool.Search
{
    internal class SearchResultPrinter
    {
        private readonly IReporter _reporter;

        public SearchResultPrinter(IReporter reporter)
        {
            _reporter = reporter ?? throw new ArgumentNullException(nameof(reporter));
        }

        public void Print(bool isDetailed, IReadOnlyCollection<SearchResultPackage> searchResultPackages)
        {
            if (!isDetailed)
            {
                var table = new PrintableTable<SearchResultPackage>();
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

                table.PrintRows(searchResultPackages, l => _reporter.WriteLine(l));
            }
            else
            {
                foreach (var p in searchResultPackages)
                {
                    _reporter.WriteLine(p.Id.ToString());
                    _reporter.WriteLine("\tVersion: " + p.LatestVersion);
                    if (p.Authors != null)
                    {
                        _reporter.WriteLine("\tAuthors: " + string.Join(", ", p.Authors));
                    }

                    _reporter.WriteLine("\tDownloads: " + p.TotalDownloads);
                    _reporter.WriteLine("\tVerified: " + p.Verified.ToString());
                    _reporter.WriteLine("\tSummary: " + p.Summary);
                    _reporter.WriteLine("\tDescription: " + p.Description);
                    _reporter.WriteLine();
                }
            }
        }

}
