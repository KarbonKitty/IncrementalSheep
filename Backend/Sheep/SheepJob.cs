namespace IncrementalSheep;

public class SheepJob : ICanStore
{
    public SheepJobId Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public SimplePrice ProductionPerSecond { get; init; }

    public SimplePrice? AdditionalStorage { get; init; }

    public SheepJob(
        SheepJobId id,
        string name,
        string description,
        SimplePrice baseProduction,
        SimplePrice? additionalStorage)
        {
            Id = id;
            Name = name;
            Description = description;
            ProductionPerSecond = baseProduction;
            AdditionalStorage = additionalStorage;
        }
}
