namespace IncrementalSheep;

public class Building : ICanStore
{
    public BuildingId Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public SimplePrice BasePrice { get; init; }
    public SimplePrice ProductionPerSecond { get; init; }
    public int NumberBuilt { get; set; }
    public SimplePrice Price => BasePrice * Math.Pow(1.15, NumberBuilt);
    public bool IsBuildable { get; }

    public SimplePrice? AdditionalStorage { get; }

    public Building(
        BuildingId id,
        string name,
        string description,
        SimplePrice basePrice,
        SimplePrice baseProduction,
        SimplePrice? additionalStorage = null,
        int numberBuilt = 0,
        bool isBuildable = false)
    {
        Id = id;
        Name = name;
        Description = description;
        BasePrice = basePrice;
        ProductionPerSecond = baseProduction;
        AdditionalStorage = additionalStorage;
        NumberBuilt = numberBuilt;
        IsBuildable = isBuildable;
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
        AdditionalStorage = template.AdditionalStorage;
        IsBuildable = template.IsBuildable;
    }

    public BuildingState SaveState()
        => new(Id, NumberBuilt);
}
