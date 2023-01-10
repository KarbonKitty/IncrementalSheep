namespace IncrementalSheep;

public class Hunt
{
    public HuntId Id { get; }
    public string Name { get; }
    public string Description { get; }

    public SimplePrice Price { get; }
    public SimplePrice Reward { get; }

    public Hunt(
        HuntId id,
        string name,
        string description,
        SimplePrice price,
        SimplePrice reward
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
