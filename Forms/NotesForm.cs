using System.Diagnostics;
using HasteNotes.Models;
using HasteNotes.Utilities;

namespace HasteNotes.Forms
{
    public partial class NotesForm : Form
    {

        private List<Note> notes = new List<Note>();
        private int noteIndex = 0;
        GlobalKeyboardHook gkh = new GlobalKeyboardHook();

        public NotesForm(string title)
        {
            InitializeComponent();

            this.Text = title;


            gkh.HookedKeys.Add(Keys.I);
            gkh.HookedKeys.Add(Keys.O);
            gkh.KeyUp += new KeyEventHandler(Gkh_KeyUp);
        }

       private void Gkh_KeyUp(object? sender, KeyEventArgs e)
        {
            Trace.WriteLine(e.KeyCode.ToString());

            if(e.KeyCode.ToString() == Constants.Keys.NEXT_KEY)
            {
                NextNote();
            }
            else if (e.KeyCode.ToString() == Constants.Keys.PREV_KEY)
            {
                PrevNote();
            }


            e.Handled = true;
        }

        public void AddNote(string title, string note)
        {
            // Trace.WriteLine("notes");

            notes.Add(new Note(title, note));

            noteIndex = notes.Count() - 1;

            UpdateNotes();

        }

        private void UpdateNotes()
        {
            if (notes.Count() > 0)
            {
                var note = notes.ElementAt(noteIndex);

                this.noteTitle.Text = note.NoteTitle;
                this.noteText.Text = note.NoteText;
            }

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var gameSelection = new GameSelection
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = this.Location
                };
                gameSelection.Show();
            }
            base.OnFormClosing(e);
        }

        private void AddNotesButton_Click(object sender, EventArgs e)
        {
            var addNote = new Notes.AddNote();

            addNote.NoteAdded += (s, args) =>
            {
                notes.Add(args.Note);
                noteIndex = notes.Count - 1;
                UpdateNotes();
            };

            addNote.StartPosition = FormStartPosition.Manual;
            addNote.Location = this.Location;
            addNote.Show();
            addNote.Select();
        }

        private void NextNote()
        {

            if (noteIndex != notes.Count() - 1)
            {
                noteIndex++;
                UpdateNotes();
            }
        }

        private void PrevNote()
        {
            Trace.WriteLine(noteIndex);

            if (noteIndex > 0)
            {
                noteIndex--;
                UpdateNotes();
            }
        }

        private void NotesNext_Click(object sender, EventArgs e)
        {
            NextNote();
        }

        private void NotesPrev_Click(object sender, EventArgs e)
        {
            PrevNote();
        }

    }
}
