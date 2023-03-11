namespace IncrementalSheep;

public record StructureTemplate
{
    public required GameObjectId Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required SimplePrice ProductionPerSecond { get; init; }
    public required SimplePrice? ConsumptionPerSecond { get; init; }
    public required SimplePrice? AdditionalStorage { get; init; }
    public required Lock[] Locks { get; init; }
}
