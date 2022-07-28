namespace IncrementalSheep;

public class GameStateDto
{
    public long LastTick { get; set; }
    public double LastDiff { get; set; }
    public double Cash { get; set; }
    public BuildingId? SelectedBuilding { get; set; }
    public BuildingDto[] Buildings { get; set; }
}
