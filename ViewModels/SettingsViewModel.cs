using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HasteNotes.Models;

namespace HasteNotes.ViewModels;

public class SettingsViewModel : ObservableObject
{
    private readonly Settings _settings;

    public SettingsViewModel()
    {
        _settings = new Settings();

        SaveSettingsCommand = new RelayCommand(SaveSettings);
        ResetDefaultsCommand = new RelayCommand(ResetDefaults);

        // Expose Avalonia Key enum as array
        AllKeys = Enum.GetValues<Key>();
    }

    public Key NextKey
    {
        get => _settings.NextKey;
        set => SetProperty(_settings.NextKey, value, _settings, (s, v) => s.NextKey = v);
    }

    public Key PrevKey
    {
        get => _settings.PrevKey;
        set => SetProperty(_settings.PrevKey, value, _settings, (s, v) => s.PrevKey = v);
    }

    public bool ShowChecklist
    {
        get => _settings.ShowChecklist;
        set => SetProperty(_settings.ShowChecklist, value, _settings, (s, v) => s.ShowChecklist = v);
    }

    public ObservableCollection<DefaultNoteFile> DefaultNotesFiles => _settings.DefaultNotesFiles;

    public Key[] AllKeys { get; }

    public ICommand SaveSettingsCommand { get; }
    public ICommand ResetDefaultsCommand { get; }

    private void SaveSettings()
    {
        // Implement saving logic, e.g., JSON file
        // SettingsService.Save(_settings);
    }

    private void ResetDefaults()
    {
        _settings.NextKey = Key.P;
        _settings.PrevKey = Key.O;
        _settings.ShowChecklist = true;

        foreach (var file in _settings.DefaultNotesFiles)
        {
            file.FileName = string.Empty;
        }

        OnPropertyChanged(nameof(NextKey));
        OnPropertyChanged(nameof(PrevKey));
        OnPropertyChanged(nameof(ShowChecklist));
        OnPropertyChanged(nameof(DefaultNotesFiles));
    }
}
