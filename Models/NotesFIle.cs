using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HasteNotes.ViewModels;

namespace HasteNotes.Models
{
    public class NotesFile
    {
        public List<Note> Notes { get; set; } = new();
        public List<ChecklistItem> Checklist { get; set; } = new();
    }
}
