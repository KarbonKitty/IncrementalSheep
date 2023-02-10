namespace IncrementalSheep;

public record HuntTemplate(
    HuntId Id,
    string Name,
    string Description,
    SimplePrice Price,
    Requirements Requirements,
    RandomReward Reward,
    TimeSpan Duration,
    HashSet<Lock> Locks
);
