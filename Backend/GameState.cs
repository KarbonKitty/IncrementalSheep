namespace IncrementalSheep;

public class GameState
{
    public DateTime LastTick { get; set; }
    public double LastDiff { get; set; }
    public double Cash { get; set; }
    public Building[] Buildings { get; set; }
    public Building? SelectedBuilding { get; set; }
}
