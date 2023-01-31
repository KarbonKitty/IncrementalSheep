namespace IncrementalSheep;

public interface ICanUnlock
{
    Lock? LockToRemove { get; }
}
