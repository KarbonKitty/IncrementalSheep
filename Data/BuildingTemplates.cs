namespace IncrementalSheep;

public static class Templates
{
    public static readonly Dictionary<BuildingId, BuildingTemplate> Buildings = new()
    {
        { BuildingId.WoodGatherer, new(BuildingId.WoodGatherer, "Wood Gatherer", "Descriptions of wood gatherer", 10, 1) },
        { BuildingId.Factory, new(BuildingId.Factory, "Factory", "Factory description", 1000, 15) }
    };
}
