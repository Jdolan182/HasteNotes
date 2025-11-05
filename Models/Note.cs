using CommunityToolkit.Mvvm.ComponentModel;

namespace HasteNotes.Models;

public partial class Note : ObservableObject
{
    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string content = string.Empty;

    [ObservableProperty]
    private bool isBossNote;

    [ObservableProperty]
    private Boss? selectedBoss;
}
