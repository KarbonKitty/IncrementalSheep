namespace IncrementalSheep;

public class Building
{
    public BuildingId Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public double BasePrice { get; init; }
    public double ProductionPerSecond { get; init; }
    public int NumberBuilt { get; set; }
    public double Price => BasePrice * Math.Pow(1.15, NumberBuilt);

    public Building(
        BuildingId id,
        string name,
        string description,
        double basePrice,
        double baseProduction)
    {
        Id = id;
        Name = name;
        Description = description;
        BasePrice = basePrice;
        ProductionPerSecond = baseProduction;
        NumberBuilt = 0;
    }

    public Building(BuildingTemplate template, BuildingState state)
    {
        if (template.Id != state.Id)
        {
            throw new ArgumentException("Template and state must belong to the same building");
        }

        Id = template.Id;
        Name = template.Name;
        Description = template.Description;
        BasePrice = template.BasePrice;
        ProductionPerSecond = template.ProductionPerSecond;
        NumberBuilt = state.NumberBuilt;
    }

    public BuildingState SaveState()
        => new(Id, NumberBuilt);
}
