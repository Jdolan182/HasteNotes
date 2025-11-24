using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;


namespace HasteNotes.Models;

public class Settings
{
    public Keys NextKey { get; set; } = Keys.P;
    public Keys PrevKey { get; set; } = Keys.O;
    public bool ShowChecklist { get; set; } = true;
    public ObservableCollection<DefaultNoteFile> DefaultNotesFiles { get; }
     = new ObservableCollection<DefaultNoteFile>(
         // Create 16 empty/default entries
         Enumerable.Range(0, 16)
                   .Select(i => new DefaultNoteFile { GameIndex = i, FileName = "" })
                   .ToList()
     );
}
