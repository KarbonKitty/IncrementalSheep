namespace IncrementalSheep;

public class Building
{
    public BuildingId Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public double ProductionPerSecond { get; set; }
    public int NumberBuilt { get; set; }

    public Building(BuildingId id, string name, string description, double basePrice, double baseProduction)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = basePrice;
        ProductionPerSecond = baseProduction;
        NumberBuilt = 0;
    }
}
