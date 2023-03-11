namespace IncrementalSheep;

public class Building : Structure, IBuyable
{
    public SimplePrice Price => innerPrice.Total() * Math.Pow(1.15, NumberBuilt);
    public Requirements Requirements => new(0);

    private ComplexPrice innerPrice;

    public Building(
        GameObjectId id,
        string name,
        string description,
        SimplePrice basePrice,
        SimplePrice baseProduction,
        SimplePrice? baseConsumption,
        SimplePrice? additionalStorage = null,
        int numberBuilt = 0)
        : base(id, name, description, baseProduction, baseConsumption, additionalStorage, numberBuilt)
    {
        innerPrice = new(basePrice);
    }

    public Building(BuildingTemplate template, StructureState state) : base(template, state)
    {
        innerPrice = new(template.BasePrice);
    }

    public void ModifyPrice(SimplePrice upgradeEffect)
        => innerPrice.AddBonus(upgradeEffect);
}
