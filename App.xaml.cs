using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;
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

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Views
                        // Windows
                    services.AddTransient<MainWindow>();
                        // Controls
                    services.AddTransient<BrowseModpacksPage>();
                    services.AddTransient<InstalledVersionsPage>();
                    services.AddTransient<MyModpacksPage>();
                    services.AddTransient<SettingsPage>();

                    // Services
                    services.AddSingleton<INavigationService, NavigationService>();

                    // ViewModels
                    services.AddTransient<MainWindowViewModel>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}
