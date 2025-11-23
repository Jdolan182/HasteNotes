using System;
using System.Collections.ObjectModel;
using Avalonia.Input;

namespace HasteNotes.Models;

public class Settings
{
    public Key NextKey { get; set; } = Key.P;
    public Key PrevKey { get; set; } = Key.O;
    public bool ShowChecklist { get; set; } = true;

    public ObservableCollection<DefaultNoteFile> DefaultNotesFiles { get; } = new ObservableCollection<DefaultNoteFile>();

    public Settings()
    {
        // Initialize 16 games
        for (int i = 0; i < 16; i++)
        {
            DefaultNotesFiles.Add(new DefaultNoteFile { GameIndex = i + 1 });
        }
    }
}
