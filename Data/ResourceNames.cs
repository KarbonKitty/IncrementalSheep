namespace IncrementalSheep;

public static class ResourceNames
{
    public static Dictionary<ResourceId, string> NameDict { get; } = new()
    {
        { ResourceId.Food, "Food" },
        { ResourceId.Wood, "Wood" },
        { ResourceId.Folklore, "Folklore" }
    };

    public static string GetName(ResourceId id)
        => NameDict[id];
}
