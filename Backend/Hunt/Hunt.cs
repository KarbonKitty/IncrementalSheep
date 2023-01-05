namespace IncrementalSheep;

public class Hunt
{
    public HuntId Id { get; }
    public string Name { get; }

    public Hunt(
        HuntId id,
        string name
    )
    {
        Id = id;
        Name = name;
    }
}
