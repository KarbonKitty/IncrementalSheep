namespace IncrementalSheep;

public class GameStateDto
{
    public long LastTick { get; set; }
    public double LastDiff { get; set; }
    public required IReadOnlyDictionary<ResourceId, ResourceWithStorage> Resources { get; set; }
    public StructureId? SelectedStructure { get; set; }
    public Branch SelectedBranch { get; set; }
    public required StructureState[] Structures { get; set; }
    public required SheepState[] Sheep { get; set; }
    public required HuntState[] Hunts { get; set; }
    public required IdeaState[] Ideas { get; set; }
}
