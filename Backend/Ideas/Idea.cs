namespace IncrementalSheep;

public class Idea : GameObject, IBuyable, ICanUnlock
{
    public SimplePrice Price { get; }
    public Requirements Requirements => new(0);
    public Lock? LockToRemove { get; }
    public bool IsBought { get; private set; }

    public Idea(
        GameObjectId id,
        string name,
        string description,
        SimplePrice basePrice,
        Lock? lockToRemove
    ) : base(id, name, description)
    {
        Price = basePrice;
        LockToRemove = lockToRemove;
        IsBought = false;
    }

    public Idea(IdeaTemplate template, IdeaState state)
    : this(
        template.Id,
        template.Name,
        template.Description,
        template.Price,
        template.LockToRemove)
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
