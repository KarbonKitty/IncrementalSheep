namespace IncrementalSheep;

public class Structure : GameObject, ICanStore
{
    public StructureId Id { get; init; }

    public SimplePrice ProductionPerSecond { get; init; }
    public int NumberBuilt { get; set; }

    public SimplePrice? AdditionalStorage { get; protected set; }

    public Structure(
        StructureId id,
        string name,
        string description,
        SimplePrice baseProduction,
        SimplePrice? additionalStorage = null,
        int numberBuilt = 0) : base(name, description)
    {
        Id = id;
        ProductionPerSecond = baseProduction;
        AdditionalStorage = additionalStorage;
        NumberBuilt = numberBuilt;
    }

    public Structure(StructureTemplate template, StructureState state) : base(template.Name, template.Description)
    {
        if (template.Id != state.Id)
        {
            throw new ArgumentException("Template and state must belong to the same structure");
        }

        Id = template.Id;
        ProductionPerSecond = template.ProductionPerSecond;
        NumberBuilt = state.NumberBuilt;
        AdditionalStorage = template.AdditionalStorage;
    }

    public StructureState SaveState()
        => new(Id, NumberBuilt, Locks.ToArray());
}
