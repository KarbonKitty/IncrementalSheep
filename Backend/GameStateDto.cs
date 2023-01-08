namespace IncrementalSheep;

public class GameStateDto
{
    public long LastTick { get; set; }
    public double LastDiff { get; set; }
    public IReadOnlyDictionary<ResourceId, (double, double?)> Resources { get; set; }
    public BuildingId? SelectedBuilding { get; set; }
    public Branch SelectedBranch { get; set; }
    public BuildingState[] Buildings { get; set; }
    public SheepState[] Sheep { get; set; }
}
