using System;
using System.Collections.Generic;
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

    public void ResetDefaults()
    {
        _settings = new Settings();
        Save();
    }
}
