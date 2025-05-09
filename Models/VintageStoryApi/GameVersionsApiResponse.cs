namespace VintageStoryModManager.Models.VintageStoryApi
{
    public class GameVersionsApiResponse
    {
        public required string StatusCode { get; set; }
        public required IEnumerable<VersionInfos> GameVersions { get; set; }
    }
}
