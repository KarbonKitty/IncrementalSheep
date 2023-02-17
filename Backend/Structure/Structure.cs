namespace IncrementalSheep;

public class Structure : GameObject, ICanStore
{
    public SimplePrice ProductionPerSecond { get; init; }
    public int NumberBuilt { get; set; }

    public SimplePrice? AdditionalStorage { get; protected set; }

    public Structure(
        GameObjectId id,
        string name,
        string description,
        SimplePrice baseProduction,
        SimplePrice? additionalStorage = null,
        int numberBuilt = 0) : base(id, name, description)
    {
        ProductionPerSecond = baseProduction;
        AdditionalStorage = additionalStorage;
        NumberBuilt = numberBuilt;
    }

    public Structure(StructureTemplate template, StructureState state) : base(template.Id, template.Name, template.Description)
    {
        if (template.Id != state.Id)
        {
            throw new ArgumentException("Template and state must belong to the same structure");
        }

        ProductionPerSecond = template.ProductionPerSecond;
        NumberBuilt = state.NumberBuilt;
        AdditionalStorage = template.AdditionalStorage;
    }

    public StructureState SaveState()
        => new(Id, NumberBuilt, Locks.ToArray());
}
