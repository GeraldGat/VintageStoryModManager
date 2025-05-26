using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace VintageStoryModManager.Models.VintageStoryApi
{
    public class ModInfosApi
    {
        public required int ModId { get; set; }
        public required int AssetId { get; set; }
        public required string Name { get; set; }
        public required string Author { get; set; }
        public string? UrlAlias { get; set; }
        public string? Summary { get; set; }
        public string? Text { get; set; }
        public string? Logo { get; set; }
        public string? LogoFile { get; set; }
        public string? HomepageUrl { get; set; }
        public string? SourceCodeUrl { get; set; }
        public string? TrailerVideoUrl { get; set; }
        public string? IssueTrackerUrl { get; set; }
        public string? WikiUrl { get; set; }
        public int? Downloads { get; set; }
        public int? Follows { get; set; }
        public string? Side { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastReleased { get; set; }
        public string[]? Tags { get; set; }
        public ReleaseInfosApi[]? Releases { get; set; }
    }
}
