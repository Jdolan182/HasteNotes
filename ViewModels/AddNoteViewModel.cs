using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HasteNotes.Models;

namespace HasteNotes.ViewModels;
public partial class AddNoteViewModel : ObservableObject
{
    [ObservableProperty] private string title = "";
    [ObservableProperty] private string content = "";
    [ObservableProperty] private bool isBossNote;
    [ObservableProperty] private Boss? selectedBoss;

    public ObservableCollection<Boss> Bosses { get; } = [];
    public ObservableCollection<Loot> CurrentSteals { get; } = [];
    public ObservableCollection<Loot> CurrentDropped { get; } = [];

    public RelayCommand SaveCommand { get; }
    public RelayCommand CancelCommand { get; }

    public event Action<bool>? RequestClose;

    public AddNoteViewModel(IEnumerable<Boss> bosses)
    {
        foreach (var b in bosses)
            Bosses.Add(b);

        SaveCommand = new RelayCommand(OnSave, CanSave);
        CancelCommand = new RelayCommand(OnCancel);
    }

    private bool CanSave() => !string.IsNullOrWhiteSpace(Title);
    private void OnSave() => RequestClose?.Invoke(true);
    private void OnCancel() => RequestClose?.Invoke(false);

    partial void OnTitleChanged(string? oldValue, string newValue)
    {
        SaveCommand.NotifyCanExecuteChanged();
    }

    // Called automatically when SelectedBoss changes
    partial void OnSelectedBossChanged(Boss? oldValue, Boss? newValue)
    {
        CurrentSteals.Clear();
        CurrentDropped.Clear();

        if (newValue != null)
        {
            foreach (var loot in newValue.Steals)
                CurrentSteals.Add(loot);

            foreach (var loot in newValue.Items)
                CurrentDropped.Add(loot);
        }
    }

    // Called automatically when IsBossNote changes
    partial void OnIsBossNoteChanged(bool oldValue, bool newValue)
    {
        if (!newValue)
            SelectedBoss = null;
    }
}