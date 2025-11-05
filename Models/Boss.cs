using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HasteNotes.Models;

public class Boss : ObservableObject
{
    public string BossName { get; set; } = "";
    public string Hp { get; set; } = "";

    // All loot collections
    public ObservableCollection<Loot> Steals { get; set; } = new();
    public ObservableCollection<Loot> Items { get; set; } = new();
    public ObservableCollection<Loot> Cards { get; set; } = new();

    public bool IsVisible { get; set; } = true;

    // Observable collections of visible loot
    public ObservableCollection<Loot> VisibleSteals { get; } = new();
    public ObservableCollection<Loot> VisibleItems { get; } = new();

    public Boss()
    {
        // Listen for changes in the loot collections
        Steals.CollectionChanged += (s, e) => RefreshVisibleLoot(VisibleSteals, Steals);
        Items.CollectionChanged += (s, e) => RefreshVisibleLoot(VisibleItems, Items);

        foreach (var loot in Steals) loot.PropertyChanged += Loot_PropertyChanged;
        foreach (var loot in Items) loot.PropertyChanged += Loot_PropertyChanged;

        // Initialize visible collections
        RefreshVisibleLoot(VisibleSteals, Steals);
        RefreshVisibleLoot(VisibleItems, Items);
    }

    private void Loot_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is Loot loot && e.PropertyName == nameof(Loot.IsVisible))
        {
            // Refresh the visible collections when IsVisible changes
            RefreshVisibleLoot(VisibleSteals, Steals);
            RefreshVisibleLoot(VisibleItems, Items);
        }
    }

    private void RefreshVisibleLoot(ObservableCollection<Loot> visible, ObservableCollection<Loot> source)
    {
        visible.Clear();
        foreach (var loot in source.Where(l => l.IsVisible))
            visible.Add(loot);
    }

    public override string ToString() => BossName;
}
