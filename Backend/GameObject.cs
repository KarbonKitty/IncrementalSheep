namespace IncrementalSheep;

public abstract class GameObject
{
    public GameObjectId Id { get; }
    private readonly HashSet<Lock> originalLocks;
    public string Name { get; }
    public string Description { get; }
    public IReadOnlySet<Lock> OriginalLocks => originalLocks;
    public HashSet<Lock> Locks { get; }
    public bool IsLocked => Locks.Count > 0;

    protected GameObject(GameObjectId id, string name, string description, IEnumerable<Lock>? locks)
    {
        Id = id;
        Name = name;
        Description = description;
        originalLocks = locks?.ToHashSet() ?? new HashSet<Lock>();
        Locks = originalLocks.ToHashSet();
    }

    public (bool removed, bool isUnlocked) RemoveLock(Lock lockToRemove)
    {
        return (Locks.Remove(lockToRemove), Locks.Count == 0);
    }
}
