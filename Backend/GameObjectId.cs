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

    Gatherer = GameObjectIdConstants.SheepJobIdStart + 1,
    Hunter = GameObjectIdConstants.SheepJobIdStart + 2,
    Elder = GameObjectIdConstants.SheepJobIdStart + 3,

    AtlatlLock = GameObjectIdConstants.LockIdStart + 1,
    FireStartingLock = GameObjectIdConstants.LockIdStart + 2,

    Atlatl = GameObjectIdConstants.IdeaIdStart + 1,
    FireStarting = GameObjectIdConstants.IdeaIdStart + 2,

    SquirrelHunt = GameObjectIdConstants.HundIdStart + 1,
    DeerHunt = GameObjectIdConstants.HundIdStart + 2,
    MammothHunt = GameObjectIdConstants.HundIdStart + 3,
}
