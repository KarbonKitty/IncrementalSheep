namespace IncrementalSheep;

public static class GameObjectIdConstants
{
    public const int StructureIdStart = 1 << 24;
    public const int SheepJobIdStart = 2 << 24;
    // Locks have their own enum now
    // public const int LockIdStart = 3 << 24;
    public const int IdeaIdStart = 4 << 24;
    public const int HundIdStart = 5 << 24;
}

public enum GameObjectId
{
    GreenPastures = GameObjectIdConstants.StructureIdStart + 1,
    Tent = GameObjectIdConstants.StructureIdStart + 2,
    LeanTo = GameObjectIdConstants.StructureIdStart + 3,

    FoodGatherer = GameObjectIdConstants.SheepJobIdStart + 1,
    Hunter = GameObjectIdConstants.SheepJobIdStart + 2,
    Elder = GameObjectIdConstants.SheepJobIdStart + 3,
    Toolmaker = GameObjectIdConstants.SheepJobIdStart + 4,

    TryNewFoods = GameObjectIdConstants.IdeaIdStart + 1,
    Hunting = GameObjectIdConstants.IdeaIdStart + 2,
    FruitGathering = GameObjectIdConstants.IdeaIdStart + 3,
    RootingForTubers = GameObjectIdConstants.IdeaIdStart + 4,
    TribeElders = GameObjectIdConstants.IdeaIdStart + 5,
    StoneTools = GameObjectIdConstants.IdeaIdStart + 6,
    FoodStorage = GameObjectIdConstants.IdeaIdStart + 7,
    TentIdea = GameObjectIdConstants.IdeaIdStart + 8,
    LeanToIdea = GameObjectIdConstants.IdeaIdStart + 9,
    WoodGathering = GameObjectIdConstants.IdeaIdStart + 10,
    FishingIdea = GameObjectIdConstants.IdeaIdStart + 11,
    Agriculture = GameObjectIdConstants.IdeaIdStart + 12,

    SmallGameHunt = GameObjectIdConstants.HundIdStart + 1,
    LargeGameHunt = GameObjectIdConstants.HundIdStart + 2,
    FishingHunt = GameObjectIdConstants.HundIdStart + 3,
}
