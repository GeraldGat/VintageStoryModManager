using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Windows;
using System.Xml.Linq;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;

namespace VintageStoryModManager.Services
{
    internal class ModpackManager : IModpackManager
    {
        private readonly IConfigurationService _configurationService;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new();

        public ModpackManager(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public ModpackInfos? AddModpack(ModpackInfos modpack)
        {
            string folderName = Guid.NewGuid().ToString();
            string modpackDir = Path.Combine(_configurationService.AppConfig.ModpacksPath, folderName);

            try
            {
                Directory.CreateDirectory(modpackDir);

                modpack.FolderName = folderName;

                string json = JsonSerializer.Serialize(modpack, _jsonSerializerOptions);
                string jsonPath = Path.Combine(modpackDir, "modpack.json");
                File.WriteAllText(jsonPath, json);

                return modpack;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while creating the modpack.", "Installation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }

        public ModpackInfos? ImportModpack(ModpackInfos modpack, ZipArchive modpackArchive)
        {
            throw new NotImplementedException();
        }

        public List<ModpackInfos> GetInstalledModpacks()
        {
            List<ModpackInfos> modpacksInfos = [];
            string modpacksPath = _configurationService.AppConfig.ModpacksPath;

            if (!Directory.Exists(modpacksPath))
                return modpacksInfos;

            foreach (var dir in Directory.GetDirectories(modpacksPath))
            {
                string modpackJsonPath = Path.Combine(dir, "modpack.json");
                if (!File.Exists(modpackJsonPath))
                    continue;

                try
                {
                    string json = File.ReadAllText(modpackJsonPath);
                    ModpackInfos? modpackInfos = JsonSerializer.Deserialize<ModpackInfos>(json, _jsonSerializerOptions);

                    if (modpackInfos != null)
                    {
                        modpackInfos.FolderName = Path.GetFileName(dir);
                        modpacksInfos.Add(modpackInfos);
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return modpacksInfos;
        }
    }
}
