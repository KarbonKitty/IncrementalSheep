namespace IncrementalSheep;

public record SheepJobTemplate(
    GameObjectId Id,
    string Name,
    string Description,
    SimplePrice BaseProduction,
    Func<GameState, SimplePrice>? PriceFunction,
    SimplePrice? AdditionalStorage,
    Lock[] Locks
);
