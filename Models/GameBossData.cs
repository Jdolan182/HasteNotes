using System.Collections.Generic;

namespace HasteNotes.Models;

public class GameBossData
{
    public string GameId { get; set; } = "";
    public List<Boss> Bosses { get; set; } = new();
}

public class Boss
{
    public string bossName { get; set; } = "";
    public string hp { get; set; } = "";
    public List<Loot> steal { get; set; } = new();
    public List<Loot> dropped { get; set; } = new();
    public List<Loot> card { get; set; } = new();
    public bool isVisible { get; set; } = true;
    public override string ToString() => bossName;
}

public class Loot
{
    public string itemName { get; set; } = "";
    public bool isVisible { get; set; } = true;
    public string? chance { get; set; }
}