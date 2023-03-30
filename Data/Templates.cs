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
            ConsumptionPerSecond = null,
            AdditionalStorage = null,
            Locks = Array.Empty<Lock>()
        } },
        { GameObjectId.FoodTent, new BuildingTemplate() {
            Id = GameObjectId.FoodTent,
            Name = "Food tent",
            Description = "Description of food tent",
            BasePrice = new(ResourceId.Food, 30),
            ProductionPerSecond = new(ResourceId.Food, 0.1),
            ConsumptionPerSecond = new(ResourceId.Wood, 0.05),
            AdditionalStorage = new(ResourceId.Food, 50),
            Locks = Array.Empty<Lock>()
        } },
        { GameObjectId.WoodGatherer, new BuildingTemplate() {
            Id = GameObjectId.WoodGatherer,
            Name = "Wood Gatherer",
            Description = "Description of wood gatherer",
            BasePrice = new(ResourceId.Wood, 25),
            ProductionPerSecond = new(ResourceId.Wood, 1),
            ConsumptionPerSecond = null,
            AdditionalStorage = null,
            Locks = Array.Empty<Lock>()
        } }
    };

    public static readonly Dictionary<GameObjectId, SheepJob> Jobs = new()
    {
        { GameObjectId.FoodGatherer, new(
            GameObjectId.FoodGatherer,
            "Food gatherer",
            "Sheep with no training, wandering around the familiar territory, gathering food for herself and others.",
            new((ResourceId.Food, 2.25), (ResourceId.Folklore, 0.03)),
            null,
            null,
            new() {}) },
        { GameObjectId.Hunter, new(
            GameObjectId.Hunter,
            "Hunter",
            "One would expect sheep to be rather mellow, but needs must, and they too can pick up sticks and stones to hunt.",
            new(),
            null,
            null,
            new() { GameObjectId.HunterLock }) },
        { GameObjectId.Elder, new(
            GameObjectId.Elder,
            "Elder",
            "An old and wise sheep, that can teach the tribe about wisdom of the ages. Perhaps this can help with learning something new?",
            new(ResourceId.Folklore, 0.66),
            gs => new SimplePrice(ResourceId.Folklore, 100) * gs.Sheep.Count(s => s.Job.Id == GameObjectId.Elder),
            new(ResourceId.Folklore, 100),
            new () { GameObjectId.ElderLock })}
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
            new () { GameObjectId.HunterLock })
    };

    public static readonly Dictionary<GameObjectId, IdeaTemplate> Ideas = new()
    {
        {
            GameObjectId.TryNewFoods, new(
                GameObjectId.TryNewFoods,
                "Try new foods",
                "Your sheep are growing discontent from just eating grass, and they want to try something else. Maybe that's actually a good idea?",
                new(ResourceId.Folklore, 1),
                GameObjectId.ExperimentsLock,
                new(GameObjectId.FoodGatherer, UpgradeProperty.Production, additiveEffect: new(ResourceId.Food, 0.05)),
                Array.Empty<Lock>()
            )
        },
        {
            GameObjectId.Hunting, new(
                GameObjectId.Hunting,
                "Start hunting",
                "The new food experiments have made the sheep long for the fresh meat. It's time to go hunting.",
                new(ResourceId.Folklore, 3),
                GameObjectId.HunterLock,
                null,
                new Lock[] { GameObjectId.ExperimentsLock }
            )
        },
        {
            GameObjectId.FruitGathering, new(
                GameObjectId.FruitGathering,
                "Fruit gathering",
                "All those delicious fruits on trees and bushes are just waiting to be picked...",
                new(ResourceId.Folklore, 3),
                null,
                new(GameObjectId.FoodGatherer, UpgradeProperty.Production, additiveEffect: new(ResourceId.Food, 0.1)),
                new Lock[] { GameObjectId.ExperimentsLock }
            )
        },
        {
            GameObjectId.RootingForTubers, new(
                GameObjectId.RootingForTubers,
                "Rooting for tubers",
                "With a little bit of elbow grease and a sensitive nose, there are many treasures to be found underground. Some of them edible.",
                new(ResourceId.Folklore, 3),
                null,
                new(GameObjectId.FoodGatherer, UpgradeProperty.Production, additiveEffect: new(ResourceId.Food, 0.1)),
                new Lock[] { GameObjectId.ExperimentsLock }
            )
        }
    };
}
