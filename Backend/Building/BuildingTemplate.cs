namespace IncrementalSheep;

public record BuildingTemplate(
    BuildingId Id,
    string Name,
    string Description,
    SimplePrice BasePrice,
    SimplePrice ProductionPerSecond,
    SimplePrice? AdditionalStorage,
    bool IsBuildable
);
