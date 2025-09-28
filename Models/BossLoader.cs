using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Avalonia.Platform;
using HasteNotes.Models;

namespace HasteNotes.Models;
public static class BossLoader
{
    public static List<Boss> LoadFromAssets(string gameId)
    {
        var uri = new Uri($"avares://HasteNotes/Assets/Data/{gameId}.json");
        using var s = AssetLoader.Open(uri);
        return JsonSerializer.Deserialize<List<Boss>>(s, Options())
               ?? new List<Boss>();
    }

    public static List<Boss> LoadFromFile(string path)
        => JsonSerializer.Deserialize<List<Boss>>(File.ReadAllText(path), Options())
           ?? new List<Boss>();

    static JsonSerializerOptions Options() => new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip
    };
}
