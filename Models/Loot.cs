using CommunityToolkit.Mvvm.ComponentModel;

namespace HasteNotes.Models;

public partial class Loot : ObservableObject
{
    public string ItemName { get; set; } = "";
    public string? Chance { get; set; }

    [ObservableProperty]
    private bool isVisible = true;

    public string Display => string.IsNullOrWhiteSpace(Chance)
        ? ItemName
        : $"{ItemName} ({Chance})";
}
