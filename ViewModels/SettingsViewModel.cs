using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HasteNotes.Models;
using HasteNotes.Services;
using HasteNotes.Views;
using Keys = System.Windows.Forms.Keys;

namespace HasteNotes.ViewModels;

public class SettingsViewModel : ObservableObject
{
    private Settings _settings = App.SettingsService?.Current
             ?? throw new InvalidOperationException("SettingsService is not initialized.");

    private readonly SettingsService settingsService = App.SettingsService;

    public IAsyncRelayCommand<DefaultNoteFile> SelectDefaultFileCommand { get; }

    public SettingsViewModel()
    {
        _settings = App.SettingsService.Current;

        SaveSettingsCommand = new RelayCommand(SaveSettings);
        ResetDefaultsCommand = new RelayCommand(ResetDefaults);

        // Expose all Keys for ComboBoxes
        AllKeys = (Keys[])Enum.GetValues(typeof(Keys));

        SelectDefaultFileCommand = new AsyncRelayCommand<DefaultNoteFile>(SelectDefaultFile);
    }

    // Keybinds
    public Keys NextKey
    {
        get => _settings.NextKey;
        set => SetProperty(_settings.NextKey, value, _settings, (s, v) => s.NextKey = v);
    }

    public Keys PrevKey
    {
        get => _settings.PrevKey;
        set => SetProperty(_settings.PrevKey, value, _settings, (s, v) => s.PrevKey = v);
    }

    // Checklist visibility
    public bool ShowChecklist
    {
        get => _settings.ShowChecklist;
        set => SetProperty(_settings.ShowChecklist, value, _settings, (s, v) => s.ShowChecklist = v);
    }

    // Default notes per game
    public ObservableCollection<DefaultNoteFile> DefaultNotesFiles => _settings.DefaultNotesFiles;

    // All available keys for ComboBoxes
    public Keys[] AllKeys { get; }

    // Commands
    public ICommand SaveSettingsCommand { get; }
    public ICommand ResetDefaultsCommand { get; }

    private void SaveSettings()
    {
        settingsService.Update(s =>
        {
            s.NextKey = this.NextKey;
            s.PrevKey = this.PrevKey;
            s.ShowChecklist = this.ShowChecklist;
        });
        settingsService.Save();
    }

    private void ResetDefaults()
    {
        settingsService.ResetDefaults();
        OnPropertyChanged(nameof(NextKey));
        OnPropertyChanged(nameof(PrevKey));
        OnPropertyChanged(nameof(ShowChecklist));
        OnPropertyChanged(nameof(DefaultNotesFiles));
        _settings = settingsService.Current;
        OnPropertyChanged(string.Empty);
    }

    public async Task SelectDefaultFile(DefaultNoteFile? file)
    {
        if (file is null) return;

        var desktop = Avalonia.Application.Current?.ApplicationLifetime
                      as Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime;

        var owner = desktop?.Windows.OfType<SettingsWindow>().FirstOrDefault(w => w.IsVisible);
        if (owner?.StorageProvider == null) return;

        var files = await owner.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("JSON Files") { Patterns = ["*.json"] }
            ]
        });

        if (files.Count > 0)
        {
            file.FileName = files[0].Path.LocalPath;
            settingsService.Save();
            OnPropertyChanged(nameof(DefaultNotesFiles));
        }
    }
    private static Window? GetMainWindow()
    {
        if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            return desktop.MainWindow;

        return null;
    }
}