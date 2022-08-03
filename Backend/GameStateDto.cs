namespace IncrementalSheep;

public class GameStateDto
{
    public long LastTick { get; set; }
    public double LastDiff { get; set; }
    public IReadOnlyDictionary<ResourceId, double> Resources { get; set; }
    public BuildingId? SelectedBuilding { get; set; }
    public BuildingState[] Buildings { get; set; }
}
