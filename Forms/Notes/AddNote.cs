using HasteNotes.Models;

namespace HasteNotes.Forms.Notes
{
    public partial class AddNote : Form
    {
        public event EventHandler<NoteEventArgs>? NoteAdded;

        public AddNote()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string noteTitleStr = noteTitle.Text;
            string noteStr = noteTextBox.Text;

            var note = new Note(noteTitleStr, noteStr);

            NoteAdded?.Invoke(this, new NoteEventArgs(note));

            this.Close(); 
        }
    }
    public class NoteEventArgs : EventArgs
    {
        public Note Note { get; }

        public NoteEventArgs(Note note)
        {
            Note = note;
        }
    }
}
