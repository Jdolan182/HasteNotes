using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HasteNotes.Models;

namespace HasteNotes.ViewModels;


public partial class AddNoteViewModel : ObservableObject
{
    // Fields bound to UI
    [ObservableProperty] private string title = "";
    [ObservableProperty] private string content = "";
    [ObservableProperty] private bool isBossNote;

    public ObservableCollection<Boss> Bosses { get; } = new();
    private Boss? _selectedBoss;
    public Boss? SelectedBoss
    {
        get => _selectedBoss;
        set => SetProperty(ref _selectedBoss, value);
    }

    // Commands
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddNoteViewModel(IEnumerable<Boss> bosses)
    {
        foreach (var b in bosses)
            Bosses.Add(b);

        SaveCommand = new RelayCommand(OnSave);
        CancelCommand = new RelayCommand(OnCancel);
    }

    // Event to notify the window
    public event Action<bool>? RequestClose;

    private void OnSave() => RequestClose?.Invoke(true);
    private void OnCancel() => RequestClose?.Invoke(false);
}
