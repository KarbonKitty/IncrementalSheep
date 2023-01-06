namespace IncrementalSheep;

public record HuntTemplate(
    HuntId Id,
    string Name,
    string Description,
    ResourceValue Price,
    ResourceValue Reward
);
