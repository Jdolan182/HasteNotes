using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HasteNotes.Models;
using HasteNotes.Services;
using HasteNotes.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using MsBoxIcon = MsBox.Avalonia.Enums.Icon;

namespace HasteNotes.ViewModels;

public partial class NotesViewModel : ObservableObject
{
    public string Title { get; }
    public bool HasBosses { get; }

    private bool _hasUnsavedChanges;
    public bool HasUnsavedChanges
    {
        get => _hasUnsavedChanges;
    }
    public void MarkDirty() => _hasUnsavedChanges = true;
    public bool IsEditing { get; set; }

    private readonly Settings settings = App.SettingsService?.Current
               ?? throw new InvalidOperationException("SettingsService is not initialized.");

    private readonly SettingsService settingsService = App.SettingsService;

    public bool ShowChecklist
    {
        get => settings.ShowChecklist;
        set
        {
            settingsService.Update(s => s.ShowChecklist = value);
            OnPropertyChanged();
        }
    }
    private readonly GlobalKeyService _keyService;

    public Note? SelectedNote => (PageIndex >= 0 && PageIndex < Notes.Count)
       ? Notes[PageIndex]
       : null;
    public bool IsBossNoteVisible => SelectedNote?.IsBossNote ?? false;

    private bool _isChecklistTextBoxFocused;
    public bool IsChecklistTextBoxFocused
    {
        get => _isChecklistTextBoxFocused;
        set => SetProperty(ref _isChecklistTextBoxFocused, value);
    }

    [ObservableProperty] private string notesText = "";
    [ObservableProperty] private int pageIndex = 0;
    [ObservableProperty] private string newChecklistText = "";
    [ObservableProperty] private bool isNotesListVisible = false;

    public ObservableCollection<Note> Notes { get; } = [];
    public ObservableCollection<Boss> Bosses { get; } = [];
    public ObservableCollection<ChecklistItem> Checklist { get; } = [];

    public event Action? RequestAddNoteDialog;
    public event Action? RequestEditNoteDialog;

    public async Task<bool> SaveAsync() => await Save();

    public ICommand GoToNoteCommand { get; }

    public NotesViewModel(string title, bool hasBosses = false)
    {
        Title = title;
        HasBosses = hasBosses;

        Notes.CollectionChanged += (_, e) =>
        {
            if (Notes.Count == 0)
            {
                PageIndex = 0;
            }
            else if (PageIndex >= Notes.Count)
            {
                // If PageIndex is out of bounds, clamp to last note
                PageIndex = Notes.Count - 1;
            }
            RefreshSelectedNote();
            MarkDirty();
        };
        Checklist.CollectionChanged += (_, __) => MarkDirty();


        if (hasBosses)
        {
            var GameId = ToGameId(title);
            var gameData = new GameBossData
            {
                GameId = ToGameId(title),
                Bosses = new ObservableCollection<Boss>(BossLoader.LoadFromAssets(ToGameId(title)))
            };
            foreach (var b in gameData.Bosses.Where(b => b.IsVisible))
                Bosses.Add(b);
        }

        var settings = App.SettingsService.Current;

        // Load default notes file if exists
        var defaultFile = settings.DefaultNotesFiles.FirstOrDefault(f => f.GameIndex.Equals(ToGameId(title), StringComparison.OrdinalIgnoreCase)
        && !string.IsNullOrEmpty(f.FileName));

        if (defaultFile != null && File.Exists(defaultFile.FileName))
        {
            _ = LoadNotesFileAsync(defaultFile.FileName);
        }

        // To allow for keys to register when window isn't focused
        _keyService = new GlobalKeyService();

        _keyService.RegisterKey(settings.NextKey, Next);
        _keyService.RegisterKey(settings.PrevKey, Prev);

        GoToNoteCommand = new RelayCommand<Note>(GoToNote);

        // Subscribe to settings changes
        App.SettingsService.SettingsChanged += OnSettingsChanged;

        // Initial key registration
        RegisterKeys(App.SettingsService.Current.NextKey, App.SettingsService.Current.PrevKey);
    }

    #region Commands

    [RelayCommand]
    private void OnAdd()
    {
        IsEditing = true;
        RequestAddNoteDialog?.Invoke();
    }

    [RelayCommand]
    private void OnEdit()
    {
        IsEditing = true;
        RequestEditNoteDialog?.Invoke();
    }
    private bool CanEdit() => SelectedNote != null;

    [RelayCommand]
    private void ToggleNotesList() => IsNotesListVisible = !IsNotesListVisible;

    [RelayCommand]
    public async Task DeleteNote(Note? note = null)
    {
        var targetNote = note ?? SelectedNote;
        if (targetNote == null) return;
        _ = MainWindow;

        var box = MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
        {
            ContentTitle = "Delete Note?",
            ContentMessage = "Are you sure you want to delete this note?",
            Icon = MsBoxIcon.Warning,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            ButtonDefinitions =
            [
                new ButtonDefinition { Name = "Yes" },
                new ButtonDefinition { Name = "No" }
            ]
        });

        var result = await box.ShowAsync();

        if (result == "Yes")
        {
            var deletedIndex = Notes.IndexOf(targetNote);
            Notes.Remove(targetNote);
            if (Notes.Count == 0)
            {
                PageIndex = 0;
            }
            else if (deletedIndex == PageIndex)
            {
                // Deleted note was selected
                // If there is a previous note, select it; otherwise select first remaining note
                PageIndex = deletedIndex - 1 >= 0 ? deletedIndex - 1 : 0;
            }
            else if (deletedIndex < PageIndex)
            {
                // Deleted note was before selected note, shift PageIndex back to selected note
                PageIndex = deletedIndex;
            }
            RefreshSelectedNote();
            MarkDirty();
        }
    }

    [RelayCommand]
    private void Next()
    {
        if (IsEditing || IsChecklistTextBoxFocused) return;
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
        if (IsEditing || IsChecklistTextBoxFocused) return;
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
    private async Task New()
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
    private async Task Open()
    {
        if (_hasUnsavedChanges)
        {
            var result = await ShowSavePromptAsync();
            if (result == ButtonResult.Cancel) return;
            if (result == ButtonResult.Yes) await SaveAsync();
        }

        var owner = MainWindow;
        if (owner?.StorageProvider == null) return;

        // Open file picker
        var files = await owner.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = false,
            FileTypeFilter =
            [
                new("JSON Files")
                {
                    Patterns = ["*.json"]
                }
            ]
        });

        if (files == null || files.Count == 0) return;

        var file = files[0];
        try
        {
            await using var stream = await file.OpenReadAsync();
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();

            var loaded = JsonSerializer.Deserialize<NotesFile>(json);
            if (loaded != null)
            {
                Notes.Clear();
                foreach (var n in loaded.Notes)
                    Notes.Add(n);

                Checklist.Clear();
                foreach (var c in loaded.Checklist)
                    Checklist.Add(c);

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
    public async Task<bool> Save()
    {
        var owner = MainWindow;
        if (owner?.StorageProvider == null) return false;

        var files = await owner.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            DefaultExtension = "json",
            FileTypeChoices =
            [
                new("JSON Files")
                {
                    Patterns = ["*.json"]
                }
            ]
        });

        if (files == null) return false;

        var file = files; // IStorageFile
        try
        {
            var data = new NotesFile
            {
                Notes = [.. Notes],
                Checklist = [.. Checklist]
            };

            var json = JsonSerializer.Serialize(data, _jsonOptions);

            await using var stream = await file.OpenWriteAsync();
            using var writer = new StreamWriter(stream);
            await writer.WriteAsync(json);

            _hasUnsavedChanges = false;
            return true;
        }
        catch (Exception ex)
        {
            await ShowMessageAsync("Error", $"Failed to save file:\n{ex.Message}");
            return false;
        }
    }


    [RelayCommand]
    private void Settings()
    {
        IsEditing = true;

        var window = new Views.SettingsWindow
        {
            DataContext = new SettingsViewModel(),
            OwnerViewModel = this
        };
        window.Show();
    }

    [RelayCommand]
    private static void Exit()
    {
        MainWindow?.Close();
    }
    #endregion

    #region Helpers
    private void GoToNote(Note? note)
    {
        if (note == null) return;

        PageIndex = Notes.IndexOf(note);
        RefreshSelectedNote();
        ToggleNotesList();
    }

    private static async Task<ButtonResult> ShowSavePromptAsync()
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

    private static async Task ShowMessageAsync(string title, string message)
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
    private static string ToGameId(string title) => title.ToLowerInvariant() switch
    {
        "final fantasy i" => "ff1",
        "final fantasy ii" => "ff2",
        "final fantasy iii" => "ff3",
        "final fantasy iv" => "ff4",
        "final fantasy v" => "ff5",
        "final fantasy vi" => "ff6",
        "final fantasy vii" => "ff7",
        "final fantasy viii" => "ff8",
        "final fantasy ix" => "ff9",
        "final fantasy x" => "ff10",
        "final fantasy x-2" => "ff10-2",
        "final fantasy xii" => "ff12",
        "final fantasy xiii" => "ff13",
        "final fantasy xv" => "ff15",
        "final fantasy xvi" => "ff16",
        "haste notes" => "none",
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
    private static Window? MainWindow
    {
        get
        {
            var app = Avalonia.Application.Current;
            if (app?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.Windows.OfType<Notes>().FirstOrDefault(w => w.IsVisible);
            }
            return null;
        }
    }

    private void OnSettingsChanged()
    {
        // Refresh checklist visibility
        OnPropertyChanged(nameof(ShowChecklist));

        // Re-register keys if they changed
        RegisterKeys(settings.NextKey, settings.PrevKey);
    }

    private void RegisterKeys(Keys next, Keys prev)
    {
        _keyService.UnregisterAll();
        _keyService.RegisterKey(next, Next);
        _keyService.RegisterKey(prev, Prev);
    }

    private async Task LoadNotesFileAsync(string path)
    {
        if (!File.Exists(path))
            return;

        try
        {
            var json = await File.ReadAllTextAsync(path);
            var loaded = JsonSerializer.Deserialize<NotesFile>(json);

            if (loaded != null)
            {
                Notes.Clear();
                foreach (var n in loaded.Notes)
                    Notes.Add(n);

                Checklist.Clear();
                foreach (var c in loaded.Checklist)
                    Checklist.Add(c);

                PageIndex = 0;
                RefreshSelectedNote();
                _hasUnsavedChanges = false;
            }
        }
        catch (Exception ex)
        {
            await ShowMessageAsync("Error", $"Failed to load default notes file:\n{ex.Message}");
        }
    }

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };
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