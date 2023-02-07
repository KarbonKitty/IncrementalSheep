namespace IncrementalSheep;

public record StructureState(
    StructureId Id,
    int NumberBuilt,
    Lock[] Locks
);
