using CommunityToolkit.Mvvm.ComponentModel;

namespace VintageStoryModManager.Models
{
    public class ModTag
    {
        public required int TagId { get; set; }
        public required string Name { get; set; }

        public bool IsSelected = false;
    }
}
