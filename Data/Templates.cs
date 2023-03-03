namespace IncrementalSheep;

public static class Templates
{
    public static readonly Dictionary<GameObjectId, StructureTemplate> Buildings = new()
    {
        { GameObjectId.GreenPastures, new() {
            Id = GameObjectId.GreenPastures,
            Name = "Green Pastures",
            Description = "A land that the sheep grazed on for generations, providing them with free food.",
            ProductionPerSecond = new(ResourceId.Food, 5),
            AdditionalStorage = null,
            Locks = Array.Empty<Lock>()
        } },
        { GameObjectId.FoodTent, new BuildingTemplate() {
            Id = GameObjectId.FoodTent,
            Name = "Food tent",
            Description = "Description of food tent",
            BasePrice = new(ResourceId.Food, 30),
            ProductionPerSecond = new(),
            AdditionalStorage = new(ResourceId.Food, 50),
            Locks = Array.Empty<Lock>()
        } },
        { GameObjectId.WoodGatherer, new BuildingTemplate() {
            Id = GameObjectId.WoodGatherer,
            Name = "Wood Gatherer",
            Description = "Description of wood gatherer",
            BasePrice = new(ResourceId.Wood, 25),
            ProductionPerSecond = new(ResourceId.Wood, 1),
            AdditionalStorage = null,
            Locks = Array.Empty<Lock>()
        } }
    };

    public static readonly Dictionary<GameObjectId, SheepJob> Jobs = new()
    {
        { GameObjectId.FoodGatherer, new(GameObjectId.FoodGatherer, "Food gatherer", "Sheep with no training, wandering around the familiar territory, gathering food for herself and others.", new(ResourceId.Food, 2.25), null, null) },
        { GameObjectId.Hunter, new(GameObjectId.Hunter, "Hunter", "One would expect sheep to be rather mellow, but needs must, and they too can pick up sticks and stones to hunt.", new(), null, null) },
        { GameObjectId.Elder, new(
            GameObjectId.Elder,
            "Elder",
            "An old and wise sheep, that can teach the tribe about wisdom of the ages. Perhaps this can help with learning something new?",
            new(ResourceId.Folklore, 0.66),
            gs => new SimplePrice(ResourceId.Folklore, 100) * gs.Sheep.Count(s => s.Job.Id == GameObjectId.Elder),
            new(ResourceId.Folklore, 100))}
    };

    public static readonly List<HuntTemplate> Hunts = new()
    {
        new(
            GameObjectId.SquirrelHunt,
            "Squirrel Hunt",
            "Description of squirrel hunt",
            new(),
            new(NumberOfHunters: 1),
            new() { Items = new RandomRewardItem[] { new(ResourceId.Food, 25, 50, 1.0) } },
            new TimeSpan(0, 0, 15),
            new () { }),
        new(
            GameObjectId.DeerHunt,
            "Deer Hunt",
            "Description of deer hunt",
            new(),
            new(NumberOfHunters: 2),
            new() { Items = new RandomRewardItem[] { new(ResourceId.Food, 100, 100, 1.0), new(ResourceId.Food, 0, 100, 0.5) } },
            new TimeSpan(0, 1, 0),
            new() { GameObjectId.AtlatlLock }),
        new(
            GameObjectId.MammothHunt,
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
            new() { GameObjectId.AtlatlLock, GameObjectId.ImpossibleLock })
    };

    public static readonly Dictionary<GameObjectId, IdeaTemplate> Ideas = new()
    {
        { GameObjectId.Atlatl, new(
            GameObjectId.Atlatl,
            "Atlatl",
            "Description of atlatl",
            new(ResourceId.Folklore, 10),
            GameObjectId.AtlatlLock,
            null,
            Array.Empty<Lock>()) },
        {
            GameObjectId.BetterGrass, new(
                GameObjectId.BetterGrass,
                "Better grass",
                "This is a test upgrade",
                new(ResourceId.Folklore, 10),
                null,
                new(GameObjectId.GreenPastures, UpgradeProperty.Production, new(ResourceId.Food, 1)),
                Array.Empty<Lock>()
            )
        },
        { GameObjectId.RootingForTubers, new(
            GameObjectId.RootingForTubers,
            "Rooting for tubers",
            "The sheep have learned that some of the plants have tubers underground, which are both tasty and nutritious.",
            new(ResourceId.Folklore, 100),
            null,
            new(GameObjectId.FoodGatherer, UpgradeProperty.Production, new(ResourceId.Food, 0.25)),
            new Lock[] { GameObjectId.ImpossibleLock }) }
    };
}
