namespace IncrementalSheep;

public class Building : Structure, ICanBeBuyable
{
    public SimplePrice BasePrice { get; init; }
    public SimplePrice Price => BasePrice * Math.Pow(1.15, NumberBuilt);
    public Requirements Requirements => new(0);

    public Building(
        StructureId id,
        string name,
        string description,
        SimplePrice basePrice,
        SimplePrice baseProduction,
        SimplePrice? additionalStorage = null,
        int numberBuilt = 0) : base(id, name, description, baseProduction, additionalStorage, numberBuilt)
    {
        BasePrice = basePrice;
    }

    public Building(BuildingTemplate template, StructureState state) : base(template, state)
    {
        BasePrice = template.BasePrice;
    }
}
