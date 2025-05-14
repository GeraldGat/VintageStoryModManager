using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Text.Json;
using System.Windows;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services;
using VintageStoryModManager.Services.Interfaces;
using VintageStoryModManager.ViewModels;
using VintageStoryModManager.Views;
using VintageStoryModManager.Views.Controls;

namespace VintageStoryModManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public static string AppSettingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        public static string AppAvailableVersionsPath = Path.Combine(AppContext.BaseDirectory, "availableversions.json");

        public App()
        {
            EnsureAppSettingsExists();

            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile(AppSettingsPath, optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    // Views
                        // Windows
                    services.AddTransient<MainWindow>();
                        // Controls
                    services.AddTransient<BrowseModpacksPage>();
                    services.AddTransient<ManageVersionsPage>();
                    services.AddTransient<MyModpacksPage>();
                    services.AddTransient<SettingsPage>();

                    // Services
                    services.AddSingleton<IConfigurationService, ConfigurationService>();
                    services.AddSingleton<IGameVersionManager, GameVersionManager>();
                    services.AddSingleton<IMainWindowUiService, MainWindowUiService>();
                    services.AddSingleton<IModpackManager, ModpackManager>();
                    services.AddSingleton<INavigationService, NavigationService>();
                    services.AddSingleton<IPopupManager, PopupManager>();
                    services.AddSingleton<IThemeManager, ThemeManager>();
                    services.AddHttpClient<IVersionDownloadService, VersionDownloadService>();
                    services.AddHttpClient<IVintageStoryApiService, VintageStoryApiService>(client =>
                    {
                        client.BaseAddress = new Uri("https://mods.vintagestory.at/api/");
                    });

                    // ViewModels
                    services.AddTransient<MainWindowViewModel>();
                    services.AddTransient<ManageVersionPageViewModel>();
                    services.AddTransient<MyModpacksPageViewModel>();
                    services.AddTransient<SettingsPageViewModel>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            var mainWindowUiService = AppHost.Services.GetRequiredService<IMainWindowUiService>();
            mainWindowUiService.MainWindow = mainWindow;

            var themeManager = AppHost.Services.GetRequiredService<IThemeManager>();
            var configurationService = AppHost.Services.GetRequiredService<IConfigurationService>();
            themeManager.ApplyTheme(configurationService.AppConfig.AppTheme);

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }

        protected static void EnsureAppSettingsExists()
        {
            if (!File.Exists(AppSettingsPath))
            {
                var defaultConfig = new
                {
                    AppConfig = new AppConfig()
                };

                var json = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions());

                File.WriteAllText(AppSettingsPath, json);
            }
        }
    }
}
