using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using HasteNotes.Models;
using HasteNotes.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBoxIcon = MsBox.Avalonia.Enums.Icon;

namespace HasteNotes.Views
{
    public partial class Notes : Window
    {
        private NotesViewModel? _lastVm;
        private bool _isForceClosing;

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

            if (DataContext is NotesViewModel mainVm)
            {
                mainVm.RequestAddNoteDialog += OpenAddNoteDialog;
                mainVm.RequestEditNoteDialog += OpenEditNoteDialog;
                _lastVm = mainVm;
            }
        }

        private void NotesListBox_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (DataContext is not NotesViewModel mainVm)
                return;

            // Get current visual ordering of items
            var newOrder = new List<Note>();

            foreach (var item in NotesListBox.Items)
            {
                if (item is Note note)
                    newOrder.Add(note);
            }

            // Replace the ViewModel's Notes collection
            mainVm.Notes.Clear();
            foreach (var note in newOrder)
                mainVm.Notes.Add(note);

            // Optional: sync to selected note’s new index
            mainVm.PageIndex = mainVm.Notes.IndexOf(mainVm.SelectedNote);

            mainVm.RefreshSelectedNote();
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
                    mainVm.MarkDirty();
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
                    mainVm.MarkDirty();
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
            if (DataContext is not NotesViewModel mainVm) return;

            if (sender is Button btn && btn.CommandParameter is ChecklistItem item)
            {
                if (mainVm.RemoveChecklistCommand != null && mainVm.RemoveChecklistCommand.CanExecute(item))
                    mainVm.RemoveChecklistCommand.Execute(item);
                    mainVm.MarkDirty();

            }
        }

        #endregion

        private void GoToNote_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is not NotesViewModel mainVm) return;

            if (sender is Button btn && btn.CommandParameter is Note note)
            {
                if (mainVm.GoToNoteCommand != null && mainVm.GoToNoteCommand.CanExecute(note))
                    mainVm.GoToNoteCommand.Execute(note);
            }
        }

        private async void DeleteNote_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (DataContext is not NotesViewModel mainVm)
                return;

            var btn = e.Source as Avalonia.Controls.Button;
            if (btn?.CommandParameter is not Note note)
                return;

            await mainVm.DeleteNote(note);
        }

        protected override async void OnClosing(WindowClosingEventArgs e)
        {
            // If we're already closing intentionally skip logic
            if (_isForceClosing)
                return;

            base.OnClosing(e);

            if (DataContext is not NotesViewModel mainVm)
                return;

            if (!mainVm.HasUnsavedChanges)
            {
                ShutdownApp();
                return;
            }

            // Cancel close until user chooses
            e.Cancel = true;

            var result = await ShowSavePromptAsync();

            switch (result)
            {
                case ButtonResult.Yes:
                    bool saved = await mainVm.SaveAsync();
                    if (saved)
                        ShutdownApp(); // only shutdown after save completes
                    break;

                case ButtonResult.No:
                    ShutdownApp();
                    break;

                case ButtonResult.Cancel:
                    // Just stay open
                    break;
            }
        }

        private void ShutdownApp()
        {
            _isForceClosing = true;  // prevents recursion
            Close();
        }

        private async Task<ButtonResult> ShowSavePromptAsync()
        {
            var box = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ContentHeader = "Unsaved Changes",
                ContentMessage = "You have unsaved changes. Save before exiting?",
                ButtonDefinitions = ButtonEnum.YesNoCancel,
                Icon = MsBoxIcon.Warning,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false
            });

            return await box.ShowAsync();
        }
    }
}
