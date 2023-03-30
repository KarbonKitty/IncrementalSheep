namespace IncrementalSheep;

public static class GameObjectIdConstants
{
    public const int StructureIdStart = 1 << 24;
    public const int SheepJobIdStart = 2 << 24;
    public const int LockIdStart = 3 << 24;
    public const int IdeaIdStart = 4 << 24;
    public const int HundIdStart = 5 << 24;
}

public enum GameObjectId
{
    WoodGatherer = GameObjectIdConstants.StructureIdStart + 1,
    GreenPastures = GameObjectIdConstants.StructureIdStart + 2,
    FoodTent = GameObjectIdConstants.StructureIdStart + 3,

    FoodGatherer = GameObjectIdConstants.SheepJobIdStart + 1,
    Hunter = GameObjectIdConstants.SheepJobIdStart + 2,
    Elder = GameObjectIdConstants.SheepJobIdStart + 3,

    ImpossibleLock = GameObjectIdConstants.LockIdStart + 1,
    HunterLock = GameObjectIdConstants.LockIdStart + 2,
    ExperimentsLock = GameObjectIdConstants.LockIdStart + 3,
    ElderLock = GameObjectIdConstants.LockIdStart + 4,

    TryNewFoods = GameObjectIdConstants.IdeaIdStart + 1,
    Hunting = GameObjectIdConstants.IdeaIdStart + 2,
    FruitGathering = GameObjectIdConstants.IdeaIdStart + 3,
    RootingForTubers = GameObjectIdConstants.IdeaIdStart + 4,

    SquirrelHunt = GameObjectIdConstants.HundIdStart + 1,
    DeerHunt = GameObjectIdConstants.HundIdStart + 2,
    MammothHunt = GameObjectIdConstants.HundIdStart + 3,
}
