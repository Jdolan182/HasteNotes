using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HasteNotes.Models;
using Keys = System.Windows.Forms.Keys;

namespace HasteNotes.ViewModels;

public class SettingsViewModel : ObservableObject
{
    private readonly Settings _settings;
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
        App.SettingsService.Update(s =>
        {
            s.NextKey = this.NextKey;
            s.PrevKey = this.PrevKey;
            s.ShowChecklist = this.ShowChecklist;
        });
        App.SettingsService.Save();
    }

    private void ResetDefaults()
    {
        App.SettingsService.ResetDefaults();
        OnPropertyChanged(nameof(NextKey));
        OnPropertyChanged(nameof(PrevKey));
        OnPropertyChanged(nameof(ShowChecklist));
        OnPropertyChanged(nameof(DefaultNotesFiles));
    }

    private async Task SelectDefaultFile(DefaultNoteFile file)
    {
        if (file is null)
            return;

        var dlg = new OpenFileDialog();
        dlg.Filters.Add(new FileDialogFilter
        {
            Name = "JSON Files",
            Extensions = { "json" }
        });

        var window = GetMainWindow();
        var result = await dlg.ShowAsync(window);

        if (result is { Length: > 0 })
        {
            file.FileName = result[0];
            App.SettingsService.Save();
            OnPropertyChanged(nameof(DefaultNotesFiles));
        }
    }

    private Window GetMainWindow()
    {
        return (Avalonia.Application.Current.ApplicationLifetime
            as IClassicDesktopStyleApplicationLifetime)!.MainWindow!;
    }
}
