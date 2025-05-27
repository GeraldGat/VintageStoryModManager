using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models
{
    public class ModpackInfos
    {
        public required string Name { get; set; }
        public required VersionInfos Version { get; set; }
        [JsonIgnore]
        public Dictionary<string, ModInfos>? Mods { get; set; }

        [JsonIgnore]
        public string GameVersion => Version.Name;
        [JsonIgnore]
        public string FolderName { get; set; } = string.Empty;
    }
}
