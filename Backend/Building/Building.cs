namespace IncrementalSheep;

public class Building
{
    public BuildingId Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double BasePrice { get; set; }
    public double ProductionPerSecond { get; set; }
    public int NumberBuilt { get; set; }
    public double Price => BasePrice * Math.Pow(1.15, NumberBuilt);

    public Building(BuildingId id, string name, string description, double basePrice, double baseProduction)
    {
        Id = id;
        Name = name;
        Description = description;
        BasePrice = basePrice;
        ProductionPerSecond = baseProduction;
        NumberBuilt = 0;
    }

    public Building(BuildingDto dto)
    {
        Id = dto.Id;
        Name = dto.Name;
        Description = dto.Description;
        BasePrice = dto.Price;
        ProductionPerSecond = dto.ProductionPerSecond;
        NumberBuilt = dto.NumberBuilt;
    }

    public BuildingDto SaveToDto()
        => new(Id, Name, Description, BasePrice, ProductionPerSecond, NumberBuilt);
}
