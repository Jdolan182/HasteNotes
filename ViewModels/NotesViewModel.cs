using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HasteNotes.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using static System.Net.Mime.MediaTypeNames;
using MsBoxIcon = MsBox.Avalonia.Enums.Icon;


namespace HasteNotes.ViewModels;

public partial class NotesViewModel : ObservableObject
{
    public string Title { get; }

    public ObservableCollection<Note> Notes { get; } = new();
    public ObservableCollection<Boss> Bosses { get; } = new();

    [ObservableProperty] private string notesText = "";

    [ObservableProperty] private int pageIndex = 0;
    public Note? SelectedNote => (PageIndex >= 0 && PageIndex < Notes.Count)
        ? Notes[PageIndex]
        : null;
    public bool IsBossNoteVisible => SelectedNote?.IsBossNote ?? false;

    [ObservableProperty]
    private string newChecklistText = "";

    private bool _hasUnsavedChanges;
    private void MarkDirty() => _hasUnsavedChanges = true;
    public ObservableCollection<ChecklistItem> Checklist { get; } = new();

    private readonly GlobalKeyService _keyService;

    // Event requests
    public event Action? RequestAddNoteDialog;
    public event Action? RequestEditNoteDialog;

    private async Task SaveAsync() => Save();


    public NotesViewModel(string title)
    {
        Title = title;

        Notes.CollectionChanged += (_, __) => MarkDirty();
        Checklist.CollectionChanged += (_, __) => MarkDirty();

        // var gameId = ToGameId(title);

        var gameData = new GameBossData
        {
            GameId = ToGameId(title),
            Bosses = new ObservableCollection<Boss>(BossLoader.LoadFromAssets(ToGameId(title)))
        };
        foreach (var b in gameData.Bosses.Where(b => b.IsVisible))
            Bosses.Add(b);

        // To allow for keys to register when window isn't focused
        _keyService = new GlobalKeyService();

        _keyService.RegisterKey(Keys.P, Next);
        _keyService.RegisterKey(Keys.O, Prev);

        // TODO: load from user settings
        // var settings = LoadUserSettings(); 
        // hotkeys.Register(settings.NextNoteKey, () => NextCommand.Execute(null));
        // hotkeys.Register(settings.PrevNoteKey, () => PrevCommand.Execute(null));
    }

    #region Commands

    // Command bound to Add button
    [RelayCommand]
    private void OnAdd() => RequestAddNoteDialog?.Invoke();

    [RelayCommand]
    private void OnEdit() => RequestEditNoteDialog?.Invoke();
    private bool CanEdit() => SelectedNote != null;

    [RelayCommand]
    private async Task DeleteNote()
    {
        if (SelectedNote == null) return;

        var box = MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
        {
            ContentTitle = "Delete Note?",
            ContentMessage = "Are you sure you want to delete this note?",
            Icon = MsBoxIcon.Warning,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            ButtonDefinitions = new[]
            {
                new ButtonDefinition { Name = "Yes" },
                new ButtonDefinition { Name = "No" }
            }
        });

        var result = await box.ShowAsync();

        if (result == "Yes")
        {
            var index = PageIndex;
            Notes.Remove(SelectedNote);

            if (Notes.Count == 0)
            {
                PageIndex = 0;
            }
            else if (index >= Notes.Count)
            {
                PageIndex = Notes.Count - 1;
            }
            else
            {
                PageIndex = index;
            }

            RefreshSelectedNote();
        }

      
    }

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

    [RelayCommand]
    private void AddChecklist()
    {
        if (!string.IsNullOrWhiteSpace(NewChecklistText))
        {
            Checklist.Add(new ChecklistItem { Text = NewChecklistText });
            NewChecklistText = "";
        }
    }

    [RelayCommand]
    private void RemoveChecklist(ChecklistItem item)
    {
        if (item != null && Checklist.Contains(item))
            Checklist.Remove(item);
    }

    [RelayCommand]
    private async void New()
    {
        if (_hasUnsavedChanges)
        {
            var result = await ShowSavePromptAsync();
            if (result == ButtonResult.Cancel) return;
            if (result == ButtonResult.Yes) await SaveAsync();
        }

        Notes.Clear();
        Checklist.Clear();

        PageIndex = 0;
        OnPropertyChanged(nameof(SelectedNote));
        OnPropertyChanged(nameof(IsBossNoteVisible));

        _hasUnsavedChanges = false;
    }

    [RelayCommand]
    private async void Open()
    {
        if (_hasUnsavedChanges)
        {
            var result = await ShowSavePromptAsync();
            if (result == ButtonResult.Cancel) return;
            if (result == ButtonResult.Yes) await SaveAsync();
        }

        var dlg = new Avalonia.Controls.OpenFileDialog();
        dlg.Filters.Add(new FileDialogFilter
        {
            Name = "JSON Files",
            Extensions = { "json" }
        });

        var path = await dlg.ShowAsync(GetMainWindow());
        if (path == null || path.Length == 0) return;

        try
        {
            var json = await File.ReadAllTextAsync(path[0]);
            var loaded = JsonSerializer.Deserialize<NotesFile>(json);

            if (loaded != null)
            {
                Notes.Clear();
                foreach (var n in loaded.Notes) Notes.Add(n);

                Checklist.Clear();
                foreach (var c in loaded.Checklist) Checklist.Add(c);

                // Reset page index to first note
                PageIndex = 0;
                OnPropertyChanged(nameof(SelectedNote));
                OnPropertyChanged(nameof(IsBossNoteVisible));

                _hasUnsavedChanges = false;
            }
        }
        catch (Exception ex)
        {
            await ShowMessageAsync("Error", $"Failed to load file:\n{ex.Message}");
        }
    }

    [RelayCommand]
    private async Task Save()
    {
        var dlg = new Avalonia.Controls.SaveFileDialog();
        dlg.Filters.Add(new FileDialogFilter
        {
            Name = "JSON Files",
            Extensions = { "json" }
        });

        var path = await dlg.ShowAsync(GetMainWindow());
        if (string.IsNullOrEmpty(path)) return;

        try
        {
            var data = new NotesFile
            {
                Notes = new List<Note>(Notes),
                Checklist = new List<ChecklistItem>(Checklist)
            };

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(path, json);
            _hasUnsavedChanges = false;
        }
        catch (Exception ex)
        {
            await ShowMessageAsync("Error", $"Failed to save file:\n{ex.Message}");
        }
    }

    [RelayCommand]
    private void Exit()
    {
        GetMainWindow().Close();
    }
    #endregion

    #region Helpers

    private async Task<ButtonResult> ShowSavePromptAsync()
    {
        var box = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentHeader = "Unsaved Changes",
            ContentMessage = "You have unsaved changes. Save before continuing?",
            ButtonDefinitions = ButtonEnum.YesNoCancel,
            Icon = MsBoxIcon.Warning,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        });

        var result = await box.ShowAsync();
        return result;
    }

    private async Task ShowMessageAsync(string title, string message)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentHeader = title,
            ContentMessage = message,
            ButtonDefinitions = ButtonEnum.Ok,
            Icon = MsBoxIcon.Info,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        });

        await box.ShowAsync();
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
    private Window GetMainWindow()
    {
        var mainWindow = Avalonia.Application.Current!.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;

        return mainWindow;
    }

    #endregion
}

public partial class ChecklistItem : ObservableObject
{
    public ChecklistItem() { }
    public ChecklistItem(string text) => Text = text;

    public string Text { get; set; } = "";
    [JsonIgnore]
    [ObservableProperty] private bool isChecked;
}