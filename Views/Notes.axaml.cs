using System;
using Avalonia.Controls;
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

            // Unsubscribe from previous VM event to avoid multiple subscriptions
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
                if (r == true)
                {
                    System.Diagnostics.Debug.WriteLine(dialogVm.Title);

                    selectedNote.Title = dialogVm.Title;
                    selectedNote.Content = dialogVm.Content;
                    selectedNote.IsBossNote = dialogVm.IsBossNote;
                    selectedNote.SelectedBoss = dialogVm.SelectedBoss;

                    System.Diagnostics.Debug.WriteLine(selectedNote);

                    // Optional: trigger refresh in viewmodel
                    mainVm.RefreshSelectedNote();
                }
            };

            dlg.ShowDialog<bool?>(this);
        }

    }
}
