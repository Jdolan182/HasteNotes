using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
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
        private bool _openGameSelectionAfterClose = false;
        private bool _isClosingAfterPrompt = false;

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

            mainVm.PageIndex = mainVm.SelectedNote is Note sel
             ? mainVm.Notes.IndexOf(sel)
             : -1;


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

            // Pass the original note to the ViewModel
            var dlg = new AddNoteWindow
            {
                DataContext = new AddNoteViewModel(mainVm.Bosses, selectedNote)
                {
                    SelectedBoss = selectedNote.SelectedBoss // use the original instance
                }
            };

            var dialogVm = (AddNoteViewModel)dlg.DataContext!;
            dialogVm.RequestClose += r =>
            {
                dlg.Close(r);
                mainVm.IsEditing = false;

                if (r == true)
                {
                    // Changes are already applied inside the ViewModel OnSave
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

        private void GameSelection_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            _openGameSelectionAfterClose = true;
            this.Close();
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            base.OnClosing(e);

            if (_isClosingAfterPrompt)
                return; // Already handled prompt, allow close

            if (DataContext is not NotesViewModel mainVm)
                return;

            if (!mainVm.HasUnsavedChanges)
            {
                // No unsaved changes, allow close normally
                return;
            }

            // Unsaved changes: cancel closing and handle prompt
            e.Cancel = true;
            HandleUnsavedChanges(mainVm);
        }

        private async void HandleUnsavedChanges(NotesViewModel mainVm)
        {
            var result = await ShowSavePromptAsync();

            switch (result)
            {
                case ButtonResult.Yes:
                    if (await mainVm.SaveAsync())
                    {
                        _isClosingAfterPrompt = true;
                        Close(); // now close without triggering prompt again
                    }
                    break;

                case ButtonResult.No:
                    _isClosingAfterPrompt = true;
                    Close(); // close without saving
                    break;

                case ButtonResult.Cancel:
                    // do nothing, leave window open
                    break;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (_openGameSelectionAfterClose)
            {
                _openGameSelectionAfterClose = false;
                var gameSelectionWindow = new MainWindow();
                gameSelectionWindow.Show();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private static async Task<ButtonResult> ShowSavePromptAsync()
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