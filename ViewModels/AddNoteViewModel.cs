using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

    // All bosses (for ComboBox)
    public ObservableCollection<Boss> Bosses { get; } = [];

    // Temporary clones for editing
    public ObservableCollection<Loot> EditingSteals { get; } = [];
    public ObservableCollection<Loot> EditingItems { get; } = [];

    public RelayCommand SaveCommand { get; }
    public RelayCommand CancelCommand { get; }

    public event Action<bool>? RequestClose;

    private readonly Note? _originalNote; // the note being edited

    public AddNoteViewModel(IEnumerable<Boss> bosses, Note? noteToEdit = null)
    {
        foreach (var b in bosses)
            Bosses.Add(b);

        SaveCommand = new RelayCommand(OnSave, CanSave);
        CancelCommand = new RelayCommand(OnCancel);

        if (noteToEdit != null)
        {
            _originalNote = noteToEdit;
            Title = noteToEdit.Title;
            Content = noteToEdit.Content;
            IsBossNote = noteToEdit.IsBossNote;

            if (IsBossNote && noteToEdit.SelectedBoss != null)
            {
                // Use the original boss instance for ComboBox selection
                SelectedBoss = Bosses.FirstOrDefault(b => b.BossName == noteToEdit.SelectedBoss.BossName);

                // Clone loot for temporary editing
                EditingSteals.Clear();
                EditingItems.Clear();
                foreach (var loot in noteToEdit.SelectedBoss.Steals)
                    EditingSteals.Add(loot.Clone());
                foreach (var loot in noteToEdit.SelectedBoss.Items)
                    EditingItems.Add(loot.Clone());
            }
        }
    }

    partial void OnSelectedBossChanged(Boss? oldValue, Boss? newValue)
    {
        if (newValue != null)
        {
            // Only initialize temporary loot if nothing is already in EditingSteals/EditingItems
            if (!EditingSteals.Any() && !EditingItems.Any())
            {
                EditingSteals.Clear();
                EditingItems.Clear();
                foreach (var loot in newValue.Steals)
                    EditingSteals.Add(loot.Clone());
                foreach (var loot in newValue.Items)
                    EditingItems.Add(loot.Clone());
            }
        }
    }

    private bool CanSave() => !string.IsNullOrWhiteSpace(Title);

    private void OnSave()
    {
        if (_originalNote != null)
        {
            _originalNote.Title = Title;
            _originalNote.Content = Content;
            _originalNote.IsBossNote = IsBossNote;

            if (IsBossNote && SelectedBoss != null)
            {
                // Apply the edited clones to the original note's boss
                var noteBoss = _originalNote.SelectedBoss!;
                noteBoss.Steals.Clear();
                foreach (var loot in EditingSteals)
                    noteBoss.Steals.Add(loot);

                noteBoss.Items.Clear();
                foreach (var loot in EditingItems)
                    noteBoss.Items.Add(loot);
            }
        }

        RequestClose?.Invoke(true);
    }

    private void OnCancel()
    {
        // Do nothing to original note
        RequestClose?.Invoke(false);
    }
}