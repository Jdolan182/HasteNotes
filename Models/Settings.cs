using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;

namespace HasteNotes.Models;

public class Settings
{
    public Keys NextKey { get; set; } = Keys.P;
    public Keys PrevKey { get; set; } = Keys.O;
    public bool ShowChecklist { get; set; } = true;
    public ObservableCollection<DefaultNoteFile> DefaultNotesFiles { get; set; } = [];

    // Initialize default entries if empty
    public void EnsureDefaults()
    {
        var defaultNames = new[]
        {
            "FF1", "FF2", "FF3", "FF4",
            "FF5", "FF6", "FF7", "FF8",
            "FF9", "FF10", "FF10-2", "FF12",
            "FF13", "FF15", "FF16", "None"
        };

        foreach (var name in defaultNames)
        {
            if (!DefaultNotesFiles.Any(f => f.GameIndex == name))
                DefaultNotesFiles.Add(new DefaultNoteFile { GameIndex = name, FileName = "" });
        }
    }

    public Settings EnsureDefaultsAndReturn()
    {
        EnsureDefaults();
        return this;
    }
}
