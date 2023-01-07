namespace IncrementalSheep;

public class Hunt
{
    public HuntId Id { get; }
    public string Name { get; }
    public string Description { get; }

    public ResourceValue Price { get; }
    public ResourceValue Reward { get; }

    public Hunt(
        HuntId id,
        string name,
        string description,
        ResourceValue price,
        ResourceValue reward
    )
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Reward = reward;
    }

    public Hunt(HuntTemplate template) : this(
        template.Id,
        template.Name,
        template.Description,
        template.Price,
        template.Reward
    ) {}
}
