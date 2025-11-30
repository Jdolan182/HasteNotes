using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using HasteNotes.Models;

namespace HasteNotes.Services;

public class SettingsService
{
    private readonly string _filePath;
    private Settings _settings;

    public SettingsService(string filePath)
    {
        _filePath = filePath;
        _settings = LoadSettings();
    }

    public Settings Current => _settings;

    public event Action? SettingsChanged;
    public void Save()
    {
        var json = JsonSerializer.Serialize(_settings, _jsonOptions);
        File.WriteAllText(_filePath, json);
    }

    private Settings LoadSettings()
    {
        if (!File.Exists(_filePath))
            return new Settings().EnsureDefaultsAndReturn();

        var json = File.ReadAllText(_filePath);
        try
        {
            var loaded = JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
            loaded.EnsureDefaults();
            return loaded;
        }
        catch
        {
            return new Settings().EnsureDefaultsAndReturn();
        }
    }

    public void Update(Action<Settings> updater)
    {
        // modify the settings
        updater(_settings);
        // notify all listeners
        SettingsChanged?.Invoke();
    }

    public void ResetDefaults()
    {
        _settings = new Settings();
        _settings.EnsureDefaults();
        SettingsChanged?.Invoke();
        Save();
    }

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };
}
