using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using HasteNotes.Models;
using HasteNotes.ViewModels;

namespace HasteNotes.Views
{
    public partial class Notes : Window
    {
        private NotesViewModel? _lastVm;

        public Notes()
        {
            InitializeComponent();
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);

            // Unsubscribe previous VM
            if (_lastVm != null)
            {
                _lastVm.RequestAddNoteDialog -= OpenAddNoteDialog;
                _lastVm.RequestEditNoteDialog -= OpenEditNoteDialog;
            }

            if (DataContext is NotesViewModel vm)
            {
                vm.RequestAddNoteDialog += OpenAddNoteDialog;
                vm.RequestEditNoteDialog += OpenEditNoteDialog;
                _lastVm = vm;
            }
        }

        private void NotesListBox_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (DataContext is not NotesViewModel vm)
                return;

            // Get current visual ordering of items
            var newOrder = new List<Note>();

            foreach (var item in NotesListBox.Items)
            {
                if (item is Note note)
                    newOrder.Add(note);
            }

            // Replace the ViewModel's Notes collection
            vm.Notes.Clear();
            foreach (var note in newOrder)
                vm.Notes.Add(note);

            // Optional: sync to selected note’s new index
            if (vm.SelectedNote != null)
                vm.PageIndex = vm.Notes.IndexOf(vm.SelectedNote);

            vm.RefreshSelectedNote();
        }

        #region Add/Edit Note Dialogs

        private void OpenAddNoteDialog()
        {
            if (DataContext is not NotesViewModel mainVm) return;

            var dlg = new AddNoteWindow
            {
                DataContext = new AddNoteViewModel(mainVm.Bosses)
            };

            var dialogVm = (AddNoteViewModel)dlg.DataContext!;
            dialogVm.RequestClose += r =>
            {
                dlg.Close(r);
                mainVm.IsEditing = false;
                if (r == true)
                {
                    var newNote = new Note
                    {
                        Title = dialogVm.Title,
                        Content = dialogVm.Content,
                        IsBossNote = dialogVm.IsBossNote,
                        SelectedBoss = dialogVm.SelectedBoss
                    };

                    mainVm.Notes.Add(newNote);
                    mainVm.PageIndex = mainVm.Notes.Count - 1;
                    mainVm.RefreshSelectedNote();
                }
            };

            dlg.Closing += (sender, args) =>
            {
                mainVm.IsEditing = false;
            };


            dlg.ShowDialog<bool?>(this);
        }

        private void OpenEditNoteDialog()
        {
            if (DataContext is not NotesViewModel mainVm) return;
            if (mainVm.SelectedNote is not Note selectedNote) return;

            var dlg = new AddNoteWindow
            {
                DataContext = new AddNoteViewModel(mainVm.Bosses)
                {
                    Title = selectedNote.Title,
                    Content = selectedNote.Content,
                    IsBossNote = selectedNote.IsBossNote,
                    SelectedBoss = selectedNote.SelectedBoss
                }
            };

            var dialogVm = (AddNoteViewModel)dlg.DataContext!;
            dialogVm.RequestClose += r =>
            {
                dlg.Close(r);
                mainVm.IsEditing = false;
                if (r == true)
                {
                    selectedNote.Title = dialogVm.Title;
                    selectedNote.Content = dialogVm.Content;
                    selectedNote.IsBossNote = dialogVm.IsBossNote;
                    selectedNote.SelectedBoss = dialogVm.SelectedBoss;

                    mainVm.RefreshSelectedNote();
                }
            };

            dlg.Closing += (sender, args) =>
            {
                mainVm.IsEditing = false;
            };


            dlg.ShowDialog<bool?>(this);
        }

        #endregion

        #region Checklist

        private void RemoveChecklist_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is not NotesViewModel vm) return;

            if (sender is Button btn && btn.CommandParameter is ChecklistItem item)
            {
                if (vm.RemoveChecklistCommand != null && vm.RemoveChecklistCommand.CanExecute(item))
                    vm.RemoveChecklistCommand.Execute(item);
            }
        }

        #endregion

        private void GoToNote_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is not NotesViewModel vm) return;

            if (sender is Button btn && btn.CommandParameter is Note note)
            {
                if (vm.GoToNoteCommand != null && vm.GoToNoteCommand.CanExecute(note))
                    vm.GoToNoteCommand.Execute(note);
            }
        }

        private void AutoScrollDuringDragBehavior_ActualThemeVariantChanged(object? sender, EventArgs e)
        {
        }
    }
}
