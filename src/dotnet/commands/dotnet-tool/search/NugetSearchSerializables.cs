// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.DotNet.ToolPackage;

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
        public NugetSearchApiAuthorsSerializable Authors { get; set; }
        public int TotalDownloads { get; set; }
        public bool Verified { get; set; }
        public NugetSearchApiVersionSerializable[] Versions { get; set; }
    }
    
    internal class NugetSearchApiAuthorsSerializable
    {
        public string[] Authors { get; set; }
    }

    /// <summary>
    /// All fields are possibly null other than Id, Version, Tags, Authors, Versions
    /// </summary>
    internal class SearchResultPackage
    {
        public SearchResultPackage(
            PackageId id, 
            string version, 
            string description,
            string summary,
            IReadOnlyCollection<string> tags,
            IReadOnlyCollection<string> authors, 
            int totalDownloads,
            bool verified,
            IReadOnlyCollection<SearchResultPackageVersion> versions)
        {
            Id = id;
            LatestVersion = version ?? throw new ArgumentNullException(nameof(version));
            Description = description;
            Summary = summary;
            Tags = tags ?? throw new ArgumentNullException(nameof(tags));
            Authors = authors ?? throw new ArgumentNullException(nameof(authors));
            TotalDownloads = totalDownloads;
            Verified = verified;
            Versions = versions ?? throw new ArgumentNullException(nameof(versions));
        }

        public PackageId Id { get; }
        public string LatestVersion { get; }
        public string Description { get; }
        public string Summary { get; }
        public IReadOnlyCollection<string> Tags { get; }
        public IReadOnlyCollection<string> Authors { get; }
        public int TotalDownloads { get; }
        public bool Verified { get; }
        public IReadOnlyCollection<SearchResultPackageVersion> Versions { get; }
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
    
    /// <summary>
    /// Author field could be a string or a string array
    /// </summary>
    internal class AuthorsConverter : JsonConverter<NugetSearchApiAuthorsSerializable>
    {
        public override NugetSearchApiAuthorsSerializable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var doc = JsonDocument.ParseValue(ref reader);
                var resultAuthors = doc.RootElement.EnumerateArray().Select(author => author.GetString()).ToArray();
                return new NugetSearchApiAuthorsSerializable() { Authors = resultAuthors };
            }
            else
            {
                var s = reader.GetString();
                return new NugetSearchApiAuthorsSerializable() { Authors = new string[] { s } };
            }
        }

        public override void Write(Utf8JsonWriter writer, NugetSearchApiAuthorsSerializable value, JsonSerializerOptions options)
        {
            // only deserialize is used
            throw new NotImplementedException();
        }
    }

    internal static class NugetSearchApiResultDeserializer
    {
        public static ReadOnlyCollection<SearchResultPackage> Deserializer(string json)
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new AuthorsConverter() },
                AllowTrailingCommas = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var deserialized = JsonSerializer.Deserialize<NugetSearchApiContainerSerializable>(json, options);
            var resultPackages = new List<SearchResultPackage>();
            foreach (var deserializedPackage in deserialized.Data)
            {
                var versions =
                    deserializedPackage.Versions.Select(v => new SearchResultPackageVersion(v.Version, v.Downloads))
                        .ToArray();
                
                string[] authors;
                if (deserializedPackage.Authors == null)
                {
                    authors
                }

                var searchResultPackage = new SearchResultPackage(new PackageId(deserializedPackage.Id), deserializedPackage.Version, deserializedPackage.Description, deserializedPackage.Summary, deserializedPackage.Tags, deserializedPackage.Authors)

            }
        }
    }
}
