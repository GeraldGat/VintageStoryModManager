using System.Reflection;

namespace VintageStoryModManager.Constants
{
    public static class ModApiFilters
    {
        public static class OrderBy
        {
            public const string Created = "asset.created";
            public const string Release = "lastreleased";
            public const string Downloads = "downloads";
            public const string Follows = "follows";
            public const string Comments = "comments";
            public const string Trending = "trendingpoints";
        }

        public static class OrderDirection
        {
            public const string Ascending = "asc";
            public const string Descending = "desc";
        }

        public static List<KeyValuePair<string, string?>> GetConstantItems(Type type)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly)
                .Select(f => new KeyValuePair<string, string?>(f.Name, f.GetRawConstantValue()?.ToString()))
                .ToList<KeyValuePair<string, string?>>();
        }
    }
}
