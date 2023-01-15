namespace IncrementalSheep;

public static class Templates
{
    public static readonly Dictionary<BuildingId, BuildingTemplate> Buildings = new()
    {
        { BuildingId.GreenPastures, new(BuildingId.GreenPastures, "Green Pastures", "Description of green pastures", new(ResourceId.Wood, 1), new(ResourceId.Food, 5), null, false) },
        { BuildingId.FoodTent, new(BuildingId.FoodTent, "Food tent", "Description of food tent", new(ResourceId.Food, 30), new(), new(ResourceId.Food, 50), true) },
        { BuildingId.WoodGatherer, new(BuildingId.WoodGatherer, "Wood Gatherer", "Descriptions of wood gatherer", new(ResourceId.Wood, 25), new(ResourceId.Wood, 1), null, true) }
    };

    public static readonly Dictionary<SheepJobId, SheepJob> Jobs = new()
    {
        { SheepJobId.Gatherer, new(SheepJobId.Gatherer, "Food gatherer", "Description of food gatherer", new(ResourceId.Food, 2.25), null) },
        { SheepJobId.Hunter, new(SheepJobId.Hunter, "Hunter", "Description of hunter", new(ResourceId.HuntPoints, 0.35), new(ResourceId.HuntPoints, 100)) }
    };

    public static readonly List<HuntTemplate> Hunts = new()
    {
        new(HuntId.SquirrelHunt, "Squirrel Hunt", "Description of squirrel hunt", new(ResourceId.HuntPoints, 100), new(ResourceId.Food, 50)),
        new(HuntId.DeerHunt, "Deer Hunt", "Description of deer hunt", new(ResourceId.HuntPoints, 400), new(ResourceId.Food, 300)),
        new(HuntId.MammothHunt, "Mammoth Hunt", "Description of mammoth hunt", new(ResourceId.HuntPoints, 1000), new(ResourceId.Food, 1000))
    };
}
