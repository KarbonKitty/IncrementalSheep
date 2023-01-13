namespace IncrementalSheep;

public class GameStateDto
{
    public long LastTick { get; set; }
    public double LastDiff { get; set; }
    public required IReadOnlyDictionary<ResourceId, ResourceWithStorage> Resources { get; set; }
    public BuildingId? SelectedBuilding { get; set; }
    public Branch SelectedBranch { get; set; }
    public required BuildingState[] Buildings { get; set; }
    public required SheepState[] Sheep { get; set; }
}
