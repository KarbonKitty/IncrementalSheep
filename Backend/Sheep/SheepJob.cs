namespace IncrementalSheep;

public class SheepJob
{
    public SheepJobId Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public SimplePrice ProductionPerSecond { get; init; }

    public SheepJob(
        SheepJobId id,
        string name,
        string description,
        SimplePrice baseProduction)
        {
            Id = id;
            Name = name;
            Description = description;
            ProductionPerSecond = baseProduction;
        }
}
