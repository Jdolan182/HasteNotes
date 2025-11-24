using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HasteNotes.Models;
using HasteNotes.Services;

namespace HasteNotes.ViewModels;

public class SettingsViewModel : ObservableObject
{
    private readonly Settings _settings;

    public SettingsViewModel()
    {
        _settings = App.SettingsService.Current;

        SaveSettingsCommand = new RelayCommand(SaveSettings);
        ResetDefaultsCommand = new RelayCommand(ResetDefaults);

        // Expose all Keys for ComboBoxes
        AllKeys = (Keys[])Enum.GetValues(typeof(Keys));
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
}
