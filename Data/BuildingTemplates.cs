namespace IncrementalSheep;

public static class Templates
{
    public static readonly Dictionary<BuildingId, BuildingTemplate> Buildings = new()
    {
        { BuildingId.GreenPastures, new(BuildingId.GreenPastures, "Green Pastures", "Description of green pastures", new(ResourceId.Wood, 1), new(ResourceId.Food, 5), false) },
        { BuildingId.WoodGatherer, new(BuildingId.WoodGatherer, "Wood Gatherer", "Descriptions of wood gatherer", new(ResourceId.Wood, 25), new(ResourceId.Wood, 1), true) }
    };

    public static readonly Dictionary<SheepJobId, SheepJob> Jobs = new()
    {
        { SheepJobId.Gatherer, new(SheepJobId.Gatherer, "Food gatherer", "Description of food gatherer", new(ResourceId.Food, 0.25)) },
        { SheepJobId.Hunter, new(SheepJobId.Hunter, "Hunter", "Description of hunter", new ResourceValue(ResourceId.HuntPoints, 0.35)) }
    };
}
