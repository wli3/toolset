// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using FluentAssertions;
using Microsoft.DotNet.TestFramework;
using Microsoft.DotNet.Tools.Common;
using Microsoft.DotNet.Tools.Test.Utilities;
using Microsoft.DotNet.Tools.Tool.Search;
using Xunit;

namespace Microsoft.DotNet.Tests
{
    public class NugetSearchApiReaderTests : TestBase
    {
        [Fact]
        public void ItCanRead()
        {
            var json = File.ReadAllText("queryResultSample.json");
            var options = new JsonSerializerOptions
            {
                Converters = { new AuthorsConverter() },
                AllowTrailingCommas = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var result = JsonSerializer.Deserialize<NugetSearchApiContainerSerializable>(json, options);

            result.Should().BeNull();
        }

    }
}
