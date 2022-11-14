namespace Overlay.Data.Models;

public class SC2Game
{
    public bool IsReplay { get; set; }
    public double DisplayTime { get; set; }
    public List<SC2Player> Players { get; set; } = new List<SC2Player>();
}