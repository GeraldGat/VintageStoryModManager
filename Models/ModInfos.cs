using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace VintageStoryModManager.Models
{
    public class ModInfos
    {
        public required string ModId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public BitmapImage? Image { get; set; }
        [JsonIgnore]
        public string? ArchiveName { get; set; }
    }
}
