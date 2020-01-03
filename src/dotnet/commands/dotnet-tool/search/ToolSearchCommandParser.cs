// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.Cli.CommandLine;
using Microsoft.DotNet.Tools.Tool.Common;
using LocalizableStrings = Microsoft.DotNet.Tools.Tool.Search.LocalizableStrings;

namespace Microsoft.DotNet.Cli
{
    internal static class ToolSearchCommandParser
    {
        public static Command ToolSearch()
        {
            return Create.Command(
                "search",
                LocalizableStrings.CommandDescription,
                Accept.ZeroOrMoreArguments()
                    .With(name: LocalizableStrings.SearchTerm,
                          description: LocalizableStrings.SearchTermDescription),
                Create.Option(
                    $"--detail",
                    "Show detail of the query",
                    Accept.NoArguments()),
                CommonOptions.HelpOption());
        }
    }
}
