using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HasteNotes.Models;


namespace HasteNotes.ViewModels;

public partial class NotesViewModel : ObservableObject
{
    public string Title { get; }

    public ObservableCollection<Note> Notes { get; } = new();
    public ObservableCollection<Boss> Bosses { get; } = new();
    public ObservableCollection<ChecklistItem> Checklist { get; } = new();

    [ObservableProperty] private string notesText = "";

    [ObservableProperty] private int pageIndex = 0;
    public Note? SelectedNote => (PageIndex >= 0 && PageIndex < Notes.Count)
        ? Notes[PageIndex]
        : null;
    public bool IsBossNoteVisible => SelectedNote?.IsBossNote ?? false;

    private readonly GlobalKeyService _keyService;

    // Event requests
    public event Action? RequestAddNoteDialog;
    public event Action? RequestEditNoteDialog;

    public NotesViewModel(string title)
    {
        Title = title;

        // var gameId = ToGameId(title);

        var gameData = new GameBossData
        {
            GameId = ToGameId(title),
            Bosses = new ObservableCollection<Boss>(BossLoader.LoadFromAssets(ToGameId(title)))
        };
        foreach (var b in gameData.Bosses.Where(b => b.IsVisible))
            Bosses.Add(b);

        _keyService = new GlobalKeyService();  // <-- Initialize before use

        _keyService.RegisterKey(Keys.P, Next);
        _keyService.RegisterKey(Keys.O, Prev);

        //TODO: load from user settings
        //var settings = LoadUserSettings(); 
        // hotkeys.Register(settings.NextNoteKey, () => NextCommand.Execute(null));
        //  hotkeys.Register(settings.PrevNoteKey, () => PrevCommand.Execute(null));
    }

    // Command bound to Add button
    [RelayCommand]
    private void OnAdd() => RequestAddNoteDialog?.Invoke();

    [RelayCommand]
    private void OnEdit() => RequestEditNoteDialog?.Invoke();
    private bool CanEdit() => SelectedNote != null;

    [RelayCommand]
    private void Next()
    {
        if (PageIndex < Notes.Count - 1)
        {
            PageIndex++;
            OnPropertyChanged(nameof(SelectedNote));
            OnPropertyChanged(nameof(IsBossNoteVisible));
        }
    }

    [RelayCommand]
    private void Prev()
    {
        if (PageIndex > 0)
        {
            PageIndex--;
            OnPropertyChanged(nameof(SelectedNote));
            OnPropertyChanged(nameof(IsBossNoteVisible));
        }
    }

    static string ToGameId(string title) => title.ToLowerInvariant() switch
    {
        "final fantasy ix" => "ff9",
        _ => title.ToLowerInvariant().Replace(" ", "")
    };

    public void RefreshSelectedNote()
    {
        OnPropertyChanged(nameof(SelectedNote));
        OnPropertyChanged(nameof(IsBossNoteVisible));
    }
    public void Dispose()
    {
        _keyService.Dispose();
    }
}

public partial class ChecklistItem : ObservableObject
{
    public ChecklistItem(string text) { Text = text; }
    public string Text { get; }

    [ObservableProperty] private bool isChecked;
}