namespace IncrementalSheep;

public class SheepJob : GameObject, ICanStore
{
    public SheepJobId Id { get; init; }
    public SimplePrice ProductionPerSecond { get; init; }

    public SimplePrice? AdditionalStorage { get; init; }

    public SheepJob(
        SheepJobId id,
        string name,
        string description,
        SimplePrice baseProduction,
        SimplePrice? additionalStorage) : base(name, description)
        {
            Id = id;
            ProductionPerSecond = baseProduction;
            AdditionalStorage = additionalStorage;
        }
}
