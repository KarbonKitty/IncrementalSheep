namespace IncrementalSheep;

public record HuntTemplate(
    GameObjectId Id,
    string Name,
    string Description,
    SimplePrice Price,
    Requirements Requirements,
    RandomReward Reward,
    TimeSpan Duration,
    HashSet<Lock> Locks
);
