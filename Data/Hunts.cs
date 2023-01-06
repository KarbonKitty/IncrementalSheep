namespace IncrementalSheep;

public static class HuntData
{
    public static List<HuntTemplate> Data = new()
    {
        new(HuntId.SquirrelHunt, "Squirrel Hunt", "Description of squirrel hunt", new(ResourceId.HuntPoints, 100), new(ResourceId.Food, 50))
    };
}
