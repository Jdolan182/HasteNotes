using System.Collections.ObjectModel;

namespace HasteNotes.Models;

public class GameBossData
{
    public string GameId { get; set; } = "";
    public ObservableCollection<Boss> Bosses { get; set; } = [];
}
