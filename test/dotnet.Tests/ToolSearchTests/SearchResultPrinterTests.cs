// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using FluentAssertions;
using Microsoft.DotNet.NugetSearch;
using Microsoft.DotNet.ToolPackage;
using Microsoft.DotNet.Tools.Test.Utilities;
using Microsoft.DotNet.Tools.Tool.Search;
using Xunit;

namespace dotnet.Tests.ToolSearchTests
{
    public class SearchResultPrinterTests
    {
        [Fact]
        public void WhenDetailedIsFalseResultHasNecessaryInfo()
        {
            var reporter = new BufferedReporter();
            var searchResultPrinter = new SearchResultPrinter(reporter);

            var filledSearchResultPackage = new SearchResultPackage(
                new PackageId("my.tool"),
                "1.0.0",
                "my tool description",
                "my tool summary",
                new List<string>() {"tag1", "tag2"},
                new List<string>() {"author1", "author2"},
                10,
                true,
                new List<SearchResultPackageVersion>() 
                    {new SearchResultPackageVersion("1.0.0", 10)});
            var mostEmptyToCheckNullException = new SearchResultPackage(
                new PackageId("my.tool"),
                null,
                null,
                null,
                new List<string>(),
                new List<string>() {"author1", "author2"},
                10,
                true,
                new List<SearchResultPackageVersion>() 
                    {new SearchResultPackageVersion("1.0.0", 10)});
            searchResultPrinter.Print();
        }

        [Fact]
        public void WhenPassedWithoutParameterItCanConstructTheUrl()
        {
            NugetSearchApiRequest.ConstructUrl()
                .AbsoluteUri
                .Should().Be(
                    "https://azuresearch-usnc.nuget.org/query?packageType=dotnettool");
        }
    }
}
