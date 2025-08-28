using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HasteNotes.Data;
using HasteNotes.Models;

namespace HasteNotes.ViewModels;

// Inherits ObservableObject so bindings update automatically
public partial class NotesViewModel : ObservableObject
{
    // ===== top controls =====
    [ObservableProperty] private bool isBossNote;

    // JSON-backed bosses list
    public ObservableCollection<Boss> Bosses { get; } = new();

    private Boss? _selectedBoss;
    public Boss? SelectedBoss
    {
        get => _selectedBoss;
        set
        {
            if (SetProperty(ref _selectedBoss, value))
                UpdateBossInfo();
        }
    }

    // ===== boss detail area (computed from SelectedBoss) =====
    [ObservableProperty] private string bossInfo = "";   // we’ll show HP here, or add a description later
    [ObservableProperty] private string bossSteals = "";  // pretty-printed steals list
    [ObservableProperty] private string bossItems = "";  // pretty-printed drops

    // ===== notes text area =====
    [ObservableProperty] private string notesText = "";

    // ===== right-side checklist =====
    public ObservableCollection<ChecklistItem> Checklist { get; } = new();

    // ===== nav state =====
    [ObservableProperty] private int pageIndex = 0;

    // ===== title (window title) =====
    public string Title { get; }

    // ===== commands (kept from your version) =====
    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand PrevCommand { get; }
    public ICommand NextCommand { get; }

    public NotesViewModel(string gameTitleOrId)
    {
        Title = gameTitleOrId;

        // Load bosses for this game from JSON
        var gameId = ToGameId(gameTitleOrId);
        foreach (var b in BossLoader.LoadFromAssets(gameId).Where(b => b.isVisible))
            Bosses.Add(b);

        // Sample checklist (keep yours)
        Checklist.Add(new ChecklistItem("Did sidequest X?"));
        Checklist.Add(new ChecklistItem("Found hidden item?"));
        Checklist.Add(new ChecklistItem("Learned ability Y?"));

        // Commands
        AddCommand = new RelayCommand(OnAdd);
        EditCommand = new RelayCommand(OnEdit);
        PrevCommand = new RelayCommand(OnPrev);
        NextCommand = new RelayCommand(OnNext);
    }

    static string ToGameId(string title) => title.ToLowerInvariant() switch
    {
        "final fantasy ix" => "ff9",
        _ => title.ToLowerInvariant().Replace(" ", "")
    };

    void OnAdd() { /* open add dialog / commit notes */ }
    void OnEdit() { /* toggle edit mode */ }
    void OnPrev() { if (PageIndex > 0) PageIndex--; /* load prev note */ }
    void OnNext() { PageIndex++; /* load next note */ }

    // Build the three display strings when selection changes
    void UpdateBossInfo()
    {
        if (SelectedBoss is null)
        {
            BossInfo = BossSteals = BossItems = "";
            return;
        }

        // For now BossInfo shows HP; add a "description" field later if needed
        BossInfo = $"HP: {SelectedBoss.hp}";

        string FormatLoot(IEnumerable<Loot> list) =>
            string.Join(", ",
                list.Where(x => x.isVisible)
                    .Select(x => string.IsNullOrWhiteSpace(x.chance)
                        ? x.itemName
                        : $"{x.itemName} ({x.chance})"));

        BossSteals = FormatLoot(SelectedBoss.steal);
        BossItems = FormatLoot(SelectedBoss.dropped); // or SelectedBoss.card if you prefer
    }
}

public partial class ChecklistItem : ObservableObject
{
    public ChecklistItem(string text) { Text = text; }
    public string Text { get; }

    [ObservableProperty] private bool isChecked;
}