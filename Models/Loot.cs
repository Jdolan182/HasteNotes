using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HasteNotes.Models;

public partial class Loot : ObservableObject
{
    [JsonPropertyName("itemName")] public string ItemName { get; set; } = "";
    [JsonPropertyName("chance")] public string? Chance { get; set; }

    [ObservableProperty] [JsonPropertyName("isVisible")] private bool isVisible = true;
    public string Display => string.IsNullOrWhiteSpace(Chance)
        ? ItemName
        : $"{ItemName} ({Chance})";

    public Loot Clone() => new()
    {
        ItemName = this.ItemName,
        Chance = this.Chance,
        IsVisible = this.IsVisible
    };
}