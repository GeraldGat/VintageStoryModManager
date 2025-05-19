using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models
{
    public partial class VersionInfos : ObservableObject
    {
        public required int? TagId { get; set; }
        public required string Name { get; set; }
        public string? DownloadUrl { get; set; }

        [ObservableProperty]
        [JsonIgnore]
        private bool isInstalling = false;

        [ObservableProperty]
        [JsonIgnore]
        private string? folderName;
        [JsonIgnore]
        public bool IsInstalled => FolderName != null && Directory.Exists(FolderName);

        partial void OnFolderNameChanged(string? oldValue, string? newValue)
        {
            OnPropertyChanged(nameof(IsInstalled));
        }
    }
}
