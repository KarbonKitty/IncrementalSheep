namespace IncrementalSheep;

public static class EnumNames
{
    public static Dictionary<ResourceId, string> NameDict { get; } = new()
    {
        { ResourceId.Food, "Food" },
        { ResourceId.Wood, "Wood" },
        { ResourceId.Folklore, "Folklore" }
    };

    public static string GetName(ResourceId id)
        => NameDict[id];

    public static Dictionary<UpgradeProperty, string> PropertyNameDict { get; } = new()
    {
        { UpgradeProperty.Production, "production" }
    };

    public static string GetName(UpgradeProperty prop)
        => PropertyNameDict[prop];
}
