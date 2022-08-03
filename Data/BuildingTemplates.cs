namespace IncrementalSheep;

public static class Templates
{
    public static readonly Dictionary<BuildingId, BuildingTemplate> Buildings = new()
    {
        { BuildingId.WoodGatherer, new(BuildingId.WoodGatherer, "Wood Gatherer", "Descriptions of wood gatherer", new(ResourceId.Wood, 25), new(ResourceId.Wood, 1)) },
        { BuildingId.Factory, new(BuildingId.Factory, "Factory", "Factory description", new(ResourceId.Wood, 1500), new(ResourceId.Wood, 10)) }
    };
}
