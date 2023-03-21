namespace IncrementalSheep;

public class SheepJob : GameObject, ICanStore
{
    public SimplePrice ProductionPerSecond { get; init; }

    public Func<GameState, SimplePrice>? GetPrice { get; init; }
    public SimplePrice? AdditionalStorage { get; init; }

    public SheepJob(
        GameObjectId id,
        string name,
        string description,
        SimplePrice baseProduction,
        Func<GameState, SimplePrice>? getPrice,
        SimplePrice? additionalStorage,
        HashSet<Lock>? locks) : base(id, name, description, locks)
        {
            ProductionPerSecond = baseProduction;
            GetPrice = getPrice;
            AdditionalStorage = additionalStorage;
        }
}
