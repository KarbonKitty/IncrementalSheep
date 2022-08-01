namespace IncrementalSheep;

public record BuildingTemplate(
    BuildingId Id,
    string Name,
    string Description,
    double BasePrice,
    double ProductionPerSecond
);
