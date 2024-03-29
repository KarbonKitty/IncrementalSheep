namespace IncrementalSheep;

public static class EnumNames
{
    public static Dictionary<ResourceId, string> NameDict { get; } = new()
    {
        { ResourceId.Food, "Food" },
        { ResourceId.StoneTools, "Stole tools" },
        { ResourceId.Folklore, "Folklore" },
        { ResourceId.BuildingMaterials, "Building materials" }
    };

    public static string GetName(ResourceId id)
        => NameDict[id];

    public static Dictionary<UpgradeProperty, string> PropertyNameDict { get; } = new()
    {
        { UpgradeProperty.Production, "production" },
        { UpgradeProperty.Price, "price" },
        { UpgradeProperty.Consumption, "consumption" },
        { UpgradeProperty.Storage, "storage" }
    };

    public static string GetName(UpgradeProperty prop)
        => PropertyNameDict[prop];
}
