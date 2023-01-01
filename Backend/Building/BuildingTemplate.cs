namespace IncrementalSheep;

public record BuildingTemplate(
    BuildingId Id,
    string Name,
    string Description,
    ResourceValue BasePrice,
    ResourceValue ProductionPerSecond,
    bool IsBuildable
);
