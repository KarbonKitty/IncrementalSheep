namespace IncrementalSheep;

public record StructureState(
    GameObjectId Id,
    int NumberBuilt,
    Lock[] Locks
);
