namespace IncrementalSheep;

public static class ResourceNames
{
    public static Dictionary<ResourceId, string> NameDict { get; } = new()
    {
        { ResourceId.Food, "Food" },
        { ResourceId.Wood, "Wood" },
        { ResourceId.HuntPoints, "Hunt points" }
    };

    public static string GetName(ResourceId id) => NameDict[id];
}
