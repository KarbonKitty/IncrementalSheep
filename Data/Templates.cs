namespace IncrementalSheep;

public static class Templates
{
    public static readonly Dictionary<GameObjectId, StructureTemplate> Buildings = new()
    {
        { GameObjectId.GreenPastures, new() {
            Id = GameObjectId.GreenPastures,
            Name = "Green Pastures",
            Description = "A land that the sheep grazed on for generations, providing them with free food.",
            ProductionPerSecond = new(ResourceId.Food, 4),
            ConsumptionPerSecond = null,
            AdditionalStorage = new((ResourceId.Food, 100), (ResourceId.StoneTools, 10), (ResourceId.BuildingMaterials, 10)),
            Locks = Array.Empty<Lock>()
        } },
        {
            GameObjectId.Tent, new BuildingTemplate {
                Id = GameObjectId.Tent,
                Name = "Hide tent",
                Description = "Shelter for sheep and a place to keep food from spoiling, made out of raw animal hides. It's a very simple construction, that can be easily packed and moved, so it can travel with the tribe.",
                BasePrice = new(ResourceId.BuildingMaterials, 30),
                ProductionPerSecond = new(),
                ConsumptionPerSecond = null,
                AdditionalStorage = new(ResourceId.Food, 50),
                Locks = new Lock[] { LockId.TentLock }
            }
        },
        {
            GameObjectId.LeanTo, new BuildingTemplate() {
                Id = GameObjectId.LeanTo,
                Name = "Lean-to",
                Description = "Simple construction made from sticks and leaves leaning against a tree or a rock, providing some cover for sheep and their food. It can be easily disassembled and rebuilt elsewhere.",
                BasePrice = new(ResourceId.BuildingMaterials, 25),
                ProductionPerSecond = new(),
                ConsumptionPerSecond = null,
                AdditionalStorage = new((ResourceId.Food, 20), (ResourceId.StoneTools, 5), (ResourceId.BuildingMaterials, 10)),
                Locks = new Lock[] { LockId.LeanToLock }
            }
        }
    };

    public static readonly Dictionary<GameObjectId, SheepJobTemplate> Jobs = new()
    {
        { GameObjectId.FoodGatherer, new(
            GameObjectId.FoodGatherer,
            "Food gatherer",
            "Sheep with no training, wandering around the familiar territory, gathering food for herself and others.",
            new((ResourceId.Food, 2.25), (ResourceId.Folklore, 0.03)),
            null,
            new(ResourceId.Folklore, 2),
            Array.Empty<Lock>()) },
        { GameObjectId.Hunter, new(
            GameObjectId.Hunter,
            "Hunter",
            "One would expect sheep to be rather mellow, but needs must, and they too can pick up sticks and stones to hunt.",
            new(),
            null,
            new(ResourceId.Folklore, 2),
            new Lock[] { LockId.HunterLock }) },
        { GameObjectId.Elder, new(
            GameObjectId.Elder,
            "Elder",
            "An old and wise sheep, that can teach the tribe about wisdom of the ages. Perhaps this can help with learning something new?",
            new(ResourceId.Folklore, 0.05),
            gs => new SimplePrice(ResourceId.Folklore, 100) * gs.Sheep.Count(s => s.Job.Id == GameObjectId.Elder),
            new(ResourceId.Folklore, 100),
            new Lock[] { LockId.ElderLock }) },
        { GameObjectId.Toolmaker, new(
            GameObjectId.Toolmaker,
            "Toolmaker",
            "Shaping stone into useful forms is somewhere between art and craft, and to prepare tools requires skill and patientce, which is why some sheep specialize in their production.",
            new(ResourceId.StoneTools, 0.1),
            _ => new SimplePrice((ResourceId.Food, 50), (ResourceId.Folklore, 10)),
            new(ResourceId.Folklore, 2),
            new Lock[] { LockId.ToolmakerLock }
        ) }
    };

    public static readonly List<HuntTemplate> Hunts = new()
    {
        new(
            GameObjectId.SmallGameHunt,
            "Squirrel Hunt",
            "Sheep are fast and stealthy, they can hunt small game like squirels without too much trouble... But they are no big catches to be had this way.",
            new(),
            new(NumberOfHunters: 1),
            new() { Items = new RandomRewardItem[] { new(ResourceId.Food, 25, 50, 1.0), new(ResourceId.Folklore, 1, 5, 0.5) } },
            new TimeSpan(0, 0, 15),
            new () { LockId.HunterLock }),
        new(
            GameObjectId.LargeGameHunt,
            "Deer hunt",
            "With some spears, sheep are perfectly capable of hunting antelopes and deer. This takes longer time to stalk the prey and prepare the hunt, but the bounty of meat, hides and tall tales is nothing to sneeze at.",
            new(ResourceId.StoneTools, 1.0),
            new(NumberOfHunters: 2),
            new() { Items = new RandomRewardItem[] { new(ResourceId.Food, 30, 60, 1.0), new(ResourceId.Food, 50, 100, 0.5), new(ResourceId.Folklore, 2, 10, 1.0), new(ResourceId.Folklore, 5, 20, 0.3), new(ResourceId.BuildingMaterials, 5, 7, 0.7) } },
            new TimeSpan(0, 5, 0),
            new() { LockId.ToolmakerLock }
        ),
        new(
            GameObjectId.FishingHunt,
            "Fishing expedition",
            "With nets and fishing spears, the sheep can fish close to the shore. With fish more plentiful and more evenly spread than the land animals, this is a boring, but sure way to build up the food stores for the tribe",
            new(ResourceId.StoneTools, 0.5),
            new(NumberOfHunters: 2),
            new() { Items = new RandomRewardItem[] { new(ResourceId.Food, 40, 50, 1.0), new(ResourceId.Folklore, 1, 2, 1.0)}},
            new TimeSpan(0, 5, 0),
            new() { LockId.FishingLock }
        )
    };

    public static readonly Dictionary<GameObjectId, IdeaTemplate> Ideas = new()
    {
        {
            GameObjectId.CaveStorage, new(
                GameObjectId.CaveStorage,
                "Cave storage",
                "With the number of sheep in the tribe growing, newcomers will want to see more food before they can be convinced that joining the tribe is a good idea. Maybe we could store some extra food somewhere to keep it from spoiling...",
                new(ResourceId.Folklore, 1),
                null,
                new(GameObjectId.GreenPastures, UpgradeProperty.Storage, additiveEffect: new(ResourceId.Food, 100)),
                Array.Empty<Lock>()
            )
        },
        {
            GameObjectId.TryNewFoods, new(
                GameObjectId.TryNewFoods,
                "Try new foods",
                "Your sheep are growing discontent from just eating grass, and they want to try something else. Maybe that's actually a good idea?",
                new(ResourceId.Folklore, 1),
                LockId.ExperimentsLock,
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
                LockId.HunterLock,
                null,
                new Lock[] { LockId.ExperimentsLock }
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
                new Lock[] { LockId.ExperimentsLock }
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
                new Lock[] { LockId.ExperimentsLock }
            )
        },
        {
            GameObjectId.StoneTools, new(
                GameObjectId.StoneTools,
                "Develop stone tools",
                "Hunting without any weapons or tools is very tiring and not very effective. Maybe it's time to try and use some of those stones that lie around?",
                new(ResourceId.Folklore, 50),
                LockId.ToolmakerLock,
                null,
                new Lock[] { LockId.HunterLock }
            )
        },
        {
            GameObjectId.FoodStorage, new(
                GameObjectId.FoodStorage,
                "Build food storage",
                "As the tribe gets bigger, and the hunting becomes more important source of food, storing this food becomes more and more of an issue. Coming up with new ways to keep it from spoiling might be useful.",
                new(ResourceId.Folklore, 75),
                LockId.ShelterLock,
                null,
                new Lock[] { LockId.HunterLock }
            )
        },
        {
            GameObjectId.TentIdea, new(
                GameObjectId.TentIdea,
                "Make a tent",
                "Maybe the tribe could use all those hides that the hunters are bringing back from the hunts for something else than just decoration?",
                new((ResourceId.Folklore, 75), (ResourceId.BuildingMaterials, 10)),
                LockId.TentLock,
                null,
                new Lock[] { LockId.ShelterLock }
            )
        },
        {
            GameObjectId.LeanToIdea, new(
                GameObjectId.LeanToIdea,
                "Make a lean-to",
                "This fallen tree that rests against the rock is a good place to hide from the rain. Maybe the sheep could construct something similar with a bunch of sticks and dry grass?",
                new((ResourceId.Folklore, 75), (ResourceId.Food, 50)),
                LockId.LeanToLock,
                null,
                new Lock[] { LockId.ShelterLock }
            )
        },
        {
            GameObjectId.TribeElders, new(
                GameObjectId.TribeElders,
                "Tribe elders",
                "Now that the tribes knows more and more about the world, this knowledge is wider than the sheep who works can keep in her head. The tribe needs some sheep who will keep the knowledge, allowing the learn more of the world.",
                new(ResourceId.Folklore, 10),
                LockId.ElderLock,
                null,
                new Lock[] { LockId.HunterLock }
            )
        },
        {
            GameObjectId.WoodGathering, new(
                GameObjectId.WoodGathering,
                "Wood gathering",
                "When the sheep are out and about gathering food, they can also gather other stuff, like wood to build the shelters from.",
                new(ResourceId.Folklore, 30),
                null,
                new(GameObjectId.FoodGatherer, UpgradeProperty.Production, new(ResourceId.BuildingMaterials, 0.05)),
                new Lock[] { LockId.ToolmakerLock, LockId.ShelterLock }
            )
        },
        {
            GameObjectId.FishingIdea, new(
                GameObjectId.FishingIdea,
                "Fishing",
                "While meat is tasty and the tales of the hunts are heroic, the fish can be easier and more consistent source of protein for the sheep - but they won't be easy to catch without tools.",
                new((ResourceId.Folklore, 70), (ResourceId.StoneTools, 10)),
                LockId.FishingLock,
                null,
                new Lock[] { LockId.ToolmakerLock }
            )
        },
        {
            GameObjectId.Agriculture, new(
                GameObjectId.Agriculture,
                "Agriculture",
                "As the sheep gather more types of food, hunt, and fish, build shelters, and make tools, a new radical idea starts to form - what if instead of just using whatever the nature provides, the tribe would try and reshape the nature in a way that would be helpful? For example, leaving seeds in the ground and caring for them, so they would grow up to be edible plants, without having to travel and find new grass..",
                new((ResourceId.Folklore, 200), (ResourceId.Food, 200), (ResourceId.StoneTools, 50)),
                LockId.AgricultureLock,
                null,
                new Lock[] { LockId.ShelterLock, LockId.FishingLock, LockId.ToolmakerLock }
            )
        }
    };
}
