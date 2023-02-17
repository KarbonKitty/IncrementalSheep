namespace IncrementalSheep;

public readonly struct Lock
{
    public required GameObjectId Id { get; init; }

    public static implicit operator Lock(GameObjectId id)
        => new() { Id = id };
}
