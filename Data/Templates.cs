namespace IncrementalSheep;

public static class Templates
{
    public static readonly Dictionary<StructureId, StructureTemplate> Buildings = new()
    {
        { StructureId.GreenPastures, new() {
            Id = StructureId.GreenPastures,
            Name = "Green Pastures",
            Description = "A land that the sheep grazed on for generations, providing them with free food.",
            ProductionPerSecond = new(ResourceId.Food, 5),
            AdditionalStorage = null
        } },
        { StructureId.FoodTent, new BuildingTemplate() {
            Id = StructureId.FoodTent,
            Name = "Food tent",
            Description = "Description of food tent",
            BasePrice = new(ResourceId.Food, 30),
            ProductionPerSecond = new(),
            AdditionalStorage = new(ResourceId.Food, 50),
            LockToRemove = null
        } },
        { StructureId.WoodGatherer, new BuildingTemplate() {
            Id = StructureId.WoodGatherer,
            Name = "Wood Gatherer",
            Description = "Description of wood gatherer",
            BasePrice = new(ResourceId.Wood, 25),
            ProductionPerSecond = new(ResourceId.Wood, 1),
            AdditionalStorage = null,
            LockToRemove = new() { Id = LockId.test1 }
        } }
    };

    public static readonly Dictionary<SheepJobId, SheepJob> Jobs = new()
    {
        { SheepJobId.Gatherer, new(SheepJobId.Gatherer, "Food gatherer", "Sheep with no training, wandering around the familiar territory, gathering food for herself and others.", new(ResourceId.Food, 2.25), null) },
        { SheepJobId.Hunter, new(SheepJobId.Hunter, "Hunter", "One would expect sheep to be rather mellow, but needs must, and they too can pick up sticks and stones to hunt.", new(), null) }
    };

    public static readonly List<HuntTemplate> Hunts = new()
    {
        new(HuntId.SquirrelHunt, "Squirrel Hunt", "Description of squirrel hunt", new(), new(NumberOfHunters: 1), new(ResourceId.Food, 50), new TimeSpan(0, 0, 15), new () { }),
        new(HuntId.DeerHunt, "Deer Hunt", "Description of deer hunt", new(), new(NumberOfHunters: 2), new(ResourceId.Food, 300), new TimeSpan(0, 1, 0), new() { LockId.test1 }),
        new(HuntId.MammothHunt, "Mammoth Hunt", "Description of mammoth hunt", new(), new(NumberOfHunters: 4), new(ResourceId.Food, 1000), new TimeSpan(0, 3, 0), new() { LockId.test1, LockId.test2 })
    };
}
