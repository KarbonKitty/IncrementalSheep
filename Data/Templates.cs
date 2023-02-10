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
            AdditionalStorage = null,
            Locks = Array.Empty<Lock>()
        } },
        { StructureId.FoodTent, new BuildingTemplate() {
            Id = StructureId.FoodTent,
            Name = "Food tent",
            Description = "Description of food tent",
            BasePrice = new(ResourceId.Food, 30),
            ProductionPerSecond = new(),
            AdditionalStorage = new(ResourceId.Food, 50),
            Locks = Array.Empty<Lock>()
        } },
        { StructureId.WoodGatherer, new BuildingTemplate() {
            Id = StructureId.WoodGatherer,
            Name = "Wood Gatherer",
            Description = "Description of wood gatherer",
            BasePrice = new(ResourceId.Wood, 25),
            ProductionPerSecond = new(ResourceId.Wood, 1),
            AdditionalStorage = null,
            Locks = Array.Empty<Lock>()
        } }
    };

    public static readonly Dictionary<SheepJobId, SheepJob> Jobs = new()
    {
        { SheepJobId.Gatherer, new(SheepJobId.Gatherer, "Food gatherer", "Sheep with no training, wandering around the familiar territory, gathering food for herself and others.", new(ResourceId.Food, 2.25), null) },
        { SheepJobId.Hunter, new(SheepJobId.Hunter, "Hunter", "One would expect sheep to be rather mellow, but needs must, and they too can pick up sticks and stones to hunt.", new(), null) },
        { SheepJobId.Elder, new(SheepJobId.Elder, "Elder", "An old and wise sheep, that can teach the tribe about wisdom of the ages. Perhaps this can help with learning something new?", new(ResourceId.Folklore, 0.66), new(ResourceId.Folklore, 100))}
    };

    public static readonly List<HuntTemplate> Hunts = new()
    {
        new(
            HuntId.SquirrelHunt,
            "Squirrel Hunt",
            "Description of squirrel hunt",
            new(),
            new(NumberOfHunters: 1),
            new() { Items = new RandomRewardItem[] { new(ResourceId.Food, 25, 50, 1.0) } },
            new TimeSpan(0, 0, 15),
            new () { }),
        new(
            HuntId.DeerHunt,
            "Deer Hunt",
            "Description of deer hunt",
            new(),
            new(NumberOfHunters: 2),
            new() { Items = new RandomRewardItem[] { new(ResourceId.Food, 100, 100, 1.0), new(ResourceId.Food, 0, 100, 0.5) } },
            new TimeSpan(0, 1, 0),
            new() { LockId.Atlatl }),
        new(
            HuntId.MammothHunt,
            "Mammoth Hunt",
            "Description of mammoth hunt",
            new(),
            new(NumberOfHunters: 4),
            new() { Items = new RandomRewardItem[] {
                new(ResourceId.Food, 300, 300, 1.0),
                new(ResourceId.Food, 0, 700, 0.5),
                new(ResourceId.Folklore, 30, 100, 0.7)
            }},
            new TimeSpan(0, 3, 0),
            new() { LockId.Atlatl, LockId.FireStarting })
    };

    public static readonly Dictionary<IdeaId, IdeaTemplate> Ideas = new()
    {
        { IdeaId.Atlatl, new(IdeaId.Atlatl, "Atlatl", "Description of atlatl", new(ResourceId.Folklore, 10), LockId.Atlatl, Array.Empty<Lock>()) },
        { IdeaId.FireStarting, new(IdeaId.FireStarting, "Fire starting", "Description of fire starting", new(ResourceId.Folklore, 100), LockId.FireStarting, Array.Empty<Lock>()) }
    };
}
