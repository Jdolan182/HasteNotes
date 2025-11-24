using System;
using System.IO;
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

    // Event that fires whenever a setting changes
    public event Action? SettingsChanged;
    public void Save()
    {
        var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    private Settings LoadSettings()
    {
        if (!File.Exists(_filePath))
            return new Settings(); // defaults

        var json = File.ReadAllText(_filePath);
        try
        {
            return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
        }
        catch
        {
            return new Settings(); // fallback if corrupt
        }
    }

    // Use this method to update a setting
    public void Update(Action<Settings> updater)
    {
        updater(_settings);      // modify the settings
        SettingsChanged?.Invoke(); // notify all listeners
    }

    public void ResetDefaults()
    {
        _settings = new Settings();
        SettingsChanged?.Invoke();
        Save();
    }
}
