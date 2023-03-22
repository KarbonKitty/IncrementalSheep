namespace IncrementalSheep;

public class Structure : GameObject, ICanStore, ICanConsume, ICanProduce
{
    public ComplexPrice ProductionPerSecond { get; init; }
    public ComplexPrice? ConsumptionPerSecond { get; init; }

    public int NumberBuilt { get; set; }

    public SimplePrice? AdditionalStorage { get; protected set; }

    public Structure(
        GameObjectId id,
        string name,
        string description,
        SimplePrice baseProduction,
        SimplePrice? baseConsumption,
        SimplePrice? additionalStorage = null,
        int numberBuilt = 0)
    : base(id, name, description)
    {
        ProductionPerSecond = new(baseProduction);
        ConsumptionPerSecond = baseConsumption is null ? null : new(baseConsumption);
        AdditionalStorage = additionalStorage;
        NumberBuilt = numberBuilt;
    }

    public Structure(StructureTemplate template, StructureState state)
    : base(template.Id, template.Name, template.Description)
    {
        if (template.Id != state.Id)
        {
            throw new ArgumentException("Template and state must belong to the same structure");
        }

        ProductionPerSecond = new(template.ProductionPerSecond);
        ConsumptionPerSecond = template.ConsumptionPerSecond is null ? null : new(template.ConsumptionPerSecond);
        NumberBuilt = state.NumberBuilt;
        AdditionalStorage = template.AdditionalStorage;
    }

    public StructureState SaveState()
        => new(Id, NumberBuilt, Locks.ToArray());
}
