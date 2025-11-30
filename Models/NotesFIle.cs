using System.Collections.Generic;
using HasteNotes.ViewModels;

namespace HasteNotes.Models
{
    public class NotesFile
    {
        public List<Note> Notes { get; set; } = [];
        public List<ChecklistItem> Checklist { get; set; } = [];
    }
}
