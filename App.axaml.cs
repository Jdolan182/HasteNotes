using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HasteNotes.Services;

namespace HasteNotes
{
    public partial class App : Application
    {
        public static SettingsService? SettingsService { get; private set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            string appFolder = Path.Combine(
               Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
               "HasteNotes"
           );

            if (!Directory.Exists(appFolder))
                Directory.CreateDirectory(appFolder);

            string settingsFile = Path.Combine(appFolder, "settings.json");

            // ---------------------------
            // Create SettingsService
            // ---------------------------
            SettingsService = new SettingsService(settingsFile);

            // If settings file didn't exist, save defaults immediately
            if (!File.Exists(settingsFile))
                SettingsService.Save();


            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}