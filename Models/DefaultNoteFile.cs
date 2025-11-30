using CommunityToolkit.Mvvm.ComponentModel;

namespace HasteNotes.Models;
public class DefaultNoteFile : ObservableObject
{
    public string GameIndex { get; set; } = string.Empty;
    private string _fileName = string.Empty;
    public string FileName
    {
        get => _fileName;
        set => SetProperty(ref _fileName, value);
    }
}