using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models.VintageStoryApi
{
    public class ReleaseInfosApi
    {
        public required int ReleaseId { get; set; }
        public required string MainFile { get; set; }
        public required string Filename { get; set; }
        public int? Downloads { get; set; }
        public string[]? Tags { get; set; }
        public string? ModVersion { get; set; }
        public DateTime? Created { get; set; }
        public string? Changelog { get; set; }
        [JsonIgnore]
        public string? GameVersion => string.Join(',', Tags ?? []);
        [JsonIgnore]
        public int? ModCompatibility { get; set; }
    }
}
