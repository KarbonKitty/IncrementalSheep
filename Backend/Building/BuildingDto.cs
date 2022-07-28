namespace IncrementalSheep;

public record BuildingDto(
    BuildingId Id,
    string Name,
    string Description,
    double Price,
    double ProductionPerSecond,
    int NumberBuilt
);
