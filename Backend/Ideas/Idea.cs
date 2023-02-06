namespace IncrementalSheep;

public class Idea : GameObject, IBuyable, ICanUnlock
{
    public IdeaId Id { get; }
    public SimplePrice Price { get; }
    public Requirements Requirements => new(0);
    public Lock? LockToRemove { get; }
    public bool IsBought { get; private set; }

    public Idea(
        IdeaId id,
        string name,
        string description,
        SimplePrice basePrice,
        Lock? lockToRemove
    ) : base(name, description)
    {
        Id = id;
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

    public void Buy()
        => IsBought = true;

    public IdeaState SaveState()
        => new(Id, IsBought);
}
