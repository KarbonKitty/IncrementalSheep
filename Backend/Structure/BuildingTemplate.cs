namespace IncrementalSheep;

public record BuildingTemplate : StructureTemplate
{
    public required SimplePrice BasePrice { get; init; }
}
