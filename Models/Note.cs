
namespace HasteNotes.Models;
public class Note
{
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public bool IsBossNote { get; set; }
    public Boss? SelectedBoss { get; set; }
}
