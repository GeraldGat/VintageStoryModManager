using System.Text.RegularExpressions;
using VintageStoryModManager.Models.VintageStoryApi;

namespace VintageStoryModManager.Constants
{
    public static class ModVersionCompatibility
    {
        public class ParsedVersion
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public int? Rc { get; set; }
        }

        public const int Exact = 4;
        public const int High = 3;
        public const int Medium = 2;
        public const int Low = 1;

        public static ModInfosApi GetReleasesCompatibility(ModInfosApi modInfosApi, string version)
        {
            if (modInfosApi.Releases == null)
                return modInfosApi;

            foreach (var release in modInfosApi.Releases)
            {
                release.ModCompatibility = CalculateReleaseCompatibility(release, version);
            }

            return modInfosApi;
        }

        public static int CalculateReleaseCompatibility(ReleaseInfosApi releaseInfosApi, string version)
        {
            var compatibility = Low;

            if (releaseInfosApi.Tags == null)
                return compatibility;

            var parsedVersion = ParseVersion(version);

            foreach (var versionRelease in releaseInfosApi.Tags)
            {
                var checkCompatibility = CompareVersion(ParseVersion(versionRelease), parsedVersion);
                if (compatibility < checkCompatibility)
                    compatibility = checkCompatibility;
            }

            return compatibility;
        }

        public static int CompareVersion(ParsedVersion releaseVersion, ParsedVersion compareVersion)
        {
            if (releaseVersion == compareVersion)
                return Exact;
            if (releaseVersion.X == compareVersion.X && releaseVersion.Y == compareVersion.Y && compareVersion.Z == releaseVersion.Z)
                return High;
            if (releaseVersion.X == compareVersion.X && releaseVersion.Y == compareVersion.Y)
                return Medium;
            return Low;

        }

        public static ParsedVersion ParseVersion(string version)
        {
            var match = Regex.Match(version, @"v?(\d+)\.(\d+)\.(\d+)(?:-rc\.(\d+))?");
            if (!match.Success)
                throw new ArgumentException($"Version invalide : {version}");

            return new ParsedVersion
            {
                X = int.Parse(match.Groups[1].Value),
                Y = int.Parse(match.Groups[2].Value),
                Z = int.Parse(match.Groups[3].Value),
                Rc = match.Groups[4].Success ? int.Parse(match.Groups[4].Value) : null
            };
        }
    }
}
