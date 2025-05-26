namespace VintageStoryModManager.Models.VintageStoryApi
{
    public class ReleaseInfosApi
    {
        public required int ReleaseId { get; set; }
        public required string MainFile { get; set; }
        public string[]? Tags { get; set; }
        public string? ModVersion { get; set; }
        public DateTime? Created { get; set; }
    }
}
