using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HasteNotes.Models;

public class Boss : ObservableObject, IJsonOnDeserialized
{
    public string BossName { get; set; } = "";
    public string Hp { get; set; } = "";
    public bool IsVisible { get; set; } = true;

    [JsonPropertyName("steal")] public ObservableCollection<Loot> Steals { get; set; } = [];
    [JsonPropertyName("dropped")] public ObservableCollection<Loot> Items { get; set; } = [];
    [JsonPropertyName("card")] public ObservableCollection<Loot> Cards { get; set; } = [];
    [JsonIgnore] public ObservableCollection<Loot> VisibleSteals { get; } = [];
    [JsonIgnore] public ObservableCollection<Loot> VisibleItems { get; } = [];

    public void OnDeserialized()
    {
        HookCollections();
        RefreshVisibleLoot(VisibleSteals, Steals);
        RefreshVisibleLoot(VisibleItems, Items);
    }

    private void HookCollections()
    {
        Steals.CollectionChanged += (s, e) => RefreshVisibleLoot(VisibleSteals, Steals);
        Items.CollectionChanged += (s, e) => RefreshVisibleLoot(VisibleItems, Items);

        foreach (var loot in Steals)
            loot.PropertyChanged += Loot_PropertyChanged;

        foreach (var loot in Items)
            loot.PropertyChanged += Loot_PropertyChanged;
    }

    private void Loot_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Loot.IsVisible))
        {
            RefreshVisibleLoot(VisibleSteals, Steals);
            RefreshVisibleLoot(VisibleItems, Items);
        }
    }
    private static void RefreshVisibleLoot(ObservableCollection<Loot> visible, ObservableCollection<Loot> source)
    {
        visible.Clear();
        foreach (var loot in source.Where(l => l.IsVisible))
            visible.Add(loot);
    }
}