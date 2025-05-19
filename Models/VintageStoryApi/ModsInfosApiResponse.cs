namespace VintageStoryModManager.Models.VintageStoryApi
{
    public class ModsInfosApiResponse
    {
        public required string StatusCode { get; set; }
        public required IEnumerable<ModInfosApi> Mods { get; set; }
    }
}
