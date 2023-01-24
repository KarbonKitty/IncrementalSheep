namespace IncrementalSheep;

public class Structure : ICanStore
{
    public StructureId Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }

    public SimplePrice ProductionPerSecond { get; init; }
    public int NumberBuilt { get; set; }

    public SimplePrice? AdditionalStorage { get; protected set; }

    public Structure(
        StructureId id,
        string name,
        string description,
        SimplePrice baseProduction,
        SimplePrice? additionalStorage = null,
        int numberBuilt = 0)
    {
        Id = id;
        Name = name;
        Description = description;
        ProductionPerSecond = baseProduction;
        AdditionalStorage = additionalStorage;
        NumberBuilt = numberBuilt;
    }

    public Structure(StructureTemplate template, StructureState state)
    {
        if (template.Id != state.Id)
        {
            throw new ArgumentException("Template and state must belong to the same structure");
        }

        Id = template.Id;
        Name = template.Name;
        Description = template.Description;
        ProductionPerSecond = template.ProductionPerSecond;
        NumberBuilt = state.NumberBuilt;
        AdditionalStorage = template.AdditionalStorage;
    }

    public StructureState SaveState()
        => new(Id, NumberBuilt);
}
