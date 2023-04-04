namespace IncrementalSheep;

public class SheepJob : GameObject, ICanStore, ICanProduce
{
    public Func<GameState, SimplePrice>? GetPrice { get; init; }
    public SimplePrice? AdditionalStorage { get; init; }

    public ComplexPrice ProductionPerSecond { get; init; }

    public SheepJob(
        GameObjectId id,
        string name,
        string description,
        SimplePrice baseProduction,
        Func<GameState, SimplePrice>? getPrice,
        SimplePrice? additionalStorage,
        HashSet<Lock>? locks) : base(id, name, description, locks)
        {
            ProductionPerSecond = new(baseProduction);
            GetPrice = getPrice;
            AdditionalStorage = additionalStorage;
        }

    public SheepJob(SheepJobTemplate template, SheepJobState state)
        : this(
            template.Id,
            template.Name,
            template.Description,
            template.BaseProduction,
            template.PriceFunction,
            template.AdditionalStorage,
            state.Locks.ToHashSet()
        ) { }

    public SheepJob(SheepJobTemplate template)
        : this(template, new(template.Id, template.Locks)) { }

    public SheepJobState SaveState()
        => new(Id, Locks.ToArray());
}
