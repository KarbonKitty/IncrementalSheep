namespace IncrementalSheep;

public record HuntTemplate(
    HuntId Id,
    string Name,
    string Description,
    SimplePrice Price,
    SimplePrice Reward
);
