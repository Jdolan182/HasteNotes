using CommunityToolkit.Mvvm.ComponentModel;

namespace HasteNotes.Models;
public class DefaultNoteFile : ObservableObject
{
    public string GameIndex { get; set; }

    private string _fileName = string.Empty;
    public string FileName
    {
        get => _fileName;
        set => SetProperty(ref _fileName, value);
    }
}