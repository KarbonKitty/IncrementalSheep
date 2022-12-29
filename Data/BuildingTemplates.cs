namespace IncrementalSheep;

public static class Templates
{
    public static readonly Dictionary<BuildingId, BuildingTemplate> Buildings = new()
    {
        { BuildingId.WoodGatherer, new(BuildingId.WoodGatherer, "Wood Gatherer", "Descriptions of wood gatherer", new(ResourceId.Wood, 25), new(ResourceId.Wood, 1)) },
        { BuildingId.Factory, new(BuildingId.Factory, "Factory", "Factory description", new(ResourceId.Wood, 1500), new(ResourceId.Wood, 10)) }
    };
    public static readonly Dictionary<SheepJobId, SheepJob> Jobs = new()
    {
        { SheepJobId.Gatherer, new(SheepJobId.Gatherer, "Food gatherer", "Description of food gatherer", new(ResourceId.Food, 0.25)) },
        { SheepJobId.Hunter, new(SheepJobId.Hunter, "Hunter", "Description of hunter", new ResourceValue(ResourceId.Food, 0.5)) }
    };
}
