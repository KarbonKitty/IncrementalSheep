namespace IncrementalSheep;

public class Idea : GameObject, IBuyable, ICanUnlock, ICanUpgrade
{
    public SimplePrice Price { get; }
    public Requirements Requirements => new(0);
    public Lock? LockToRemove { get; }
    public Upgrade? Upgrade { get; }
    public bool IsBought { get; private set; }

    public Idea(
        GameObjectId id,
        string name,
        string description,
        SimplePrice basePrice,
        Lock? lockToRemove,
        Upgrade? upgrade
    ) : base(id, name, description)
    {
        Price = basePrice;
        LockToRemove = lockToRemove;
        Upgrade = upgrade;
        IsBought = false;
    }

    public Idea(IdeaTemplate template, IdeaState state)
    : this(
        template.Id,
        template.Name,
        template.Description,
        template.Price,
        template.LockToRemove,
        template.Upgrade)
    {
        IsBought = state.IsBought;
    }

    public Idea(IdeaTemplate template)
    : this(template, new(template.Id, false, template.Locks))
    { }

    public void Buy()
        => IsBought = true;

    public IdeaState SaveState()
        => new(Id, IsBought, Locks.ToArray());
}
