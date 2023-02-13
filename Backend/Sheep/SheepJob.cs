namespace IncrementalSheep;

public class SheepJob : GameObject, ICanStore
{
    public SheepJobId Id { get; init; }
    public SimplePrice ProductionPerSecond { get; init; }

    public Func<GameState, SimplePrice>? GetPrice { get; init; }
    public SimplePrice? AdditionalStorage { get; init; }

    public SheepJob(
        SheepJobId id,
        string name,
        string description,
        SimplePrice baseProduction,
        Func<GameState, SimplePrice>? getPrice,
        SimplePrice? additionalStorage) : base(name, description)
        {
            Id = id;
            ProductionPerSecond = baseProduction;
            GetPrice = getPrice;
            AdditionalStorage = additionalStorage;
        }
}
