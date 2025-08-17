namespace HasteNotes.Models
{
    public class Note
    {
        public string NoteTitle { get; set; }
        public string NoteText { get; set; }

        public Note(String title, String note)
        {
            NoteTitle = title;
            NoteText  = note;
        }
    }
}
