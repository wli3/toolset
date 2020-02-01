using System.Linq;
using Microsoft.DotNet.Cli;
using Microsoft.DotNet.Cli.CommandLine;
using Microsoft.DotNet.Cli.Utils;

namespace Microsoft.DotNet.NugetSearch
{
    internal class NugetSearchApiParameter
    {
        public NugetSearchApiParameter(
            string searchTerm = null,
            int? skip = null,
            int? take = null,
            bool prerelease = false,
            string semverLevel = null)
        {
            SearchTerm = searchTerm;
            Skip = skip;
            Take = take;
            Prerelease = prerelease;
            SemverLevel = semverLevel;
        }

        public string SearchTerm { get; }
        public int? Skip { get; }
        public int? Take { get; }
        public bool Prerelease { get; }
        public string SemverLevel { get; }

        public NugetSearchApiParameter(AppliedOption appliedOption)
        {
            var searchTerm = appliedOption.Arguments.Single();

            var skip = GetParsedResultAsInt("skip");
            var take = GetParsedResultAsInt("take");
            var prerelease = appliedOption.ValueOrDefault<bool>("prerelease");
            var semverLevel = appliedOption.ValueOrDefault<string>("semver-level");

            SearchTerm = searchTerm;
            Skip = skip;
            Take = take;
            Prerelease = prerelease;
            SemverLevel = semverLevel;
        }

        private static int? GetParsedResultAsInt(string alias)
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                return null;
            }

            if (int.TryParse(alias, out int i))
            {
                return i;
            }
            else
            {
                throw new GracefulException(string.Format(Tools.Tool.Search.LocalizableStrings.InvalidInputTypeInteger,
                    alias));
            }
        }
    }
}
