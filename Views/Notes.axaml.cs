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
                _lastVm.RequestAddNoteDialog -= OpenAddNoteDialog;

            if (DataContext is NotesViewModel vm)
            {
                vm.RequestAddNoteDialog += OpenAddNoteDialog;
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
    }
}
