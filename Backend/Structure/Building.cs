namespace IncrementalSheep;

public class Building : Structure, IBuyable, ICanUnlock
{
    public SimplePrice BasePrice { get; init; }
    public SimplePrice Price => BasePrice * Math.Pow(1.15, NumberBuilt);
    public Requirements Requirements => new(0);
    public Lock? LockToRemove { get; init; }

    public Building(
        StructureId id,
        string name,
        string description,
        SimplePrice basePrice,
        SimplePrice baseProduction,
        SimplePrice? additionalStorage = null,
        Lock? lockToRemove = null,
        int numberBuilt = 0) : base(id, name, description, baseProduction, additionalStorage, numberBuilt)
    {
        BasePrice = basePrice;
        LockToRemove = lockToRemove;
    }

    public Building(BuildingTemplate template, StructureState state) : base(template, state)
    {
        BasePrice = template.BasePrice;
        LockToRemove = template.LockToRemove;
    }
}
