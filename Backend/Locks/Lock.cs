namespace IncrementalSheep;

public readonly struct Lock
{
    public required LockId Id { get; init; }

    public static implicit operator Lock(LockId id)
        => new() { Id = id };
}
