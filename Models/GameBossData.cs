using System.Collections.Generic;

namespace HasteNotes.Models;

public class GameBossData
{
    public string GameId { get; set; } = "";
    public List<Boss> Bosses { get; set; } = new();
}

public class Boss
{
    public string BossName { get; set; } = "";
    public string Hp { get; set; } = "";
    public List<Loot> Steals { get; set; } = new();
    public List<Loot> Items { get; set; } = new();
    public List<Loot> Cards { get; set; } = new();
    public bool IsVisible { get; set; } = true;
    public override string ToString() => BossName;
}

public class Loot
{
    public string ItemName { get; set; } = "";
    public bool IsVisible { get; set; } = true;
    public string? Chance { get; set; }
}