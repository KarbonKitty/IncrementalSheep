namespace IncrementalSheep;

public class Hunt : GameObject, IBuyable, ITakeTime
{
    public SimplePrice Price => innerPrice.Total();
    public Requirements Requirements { get; }
    public RandomReward Reward { get; }

    public TimeSpan Duration { get; }

    public TimeSpan TimeLeft { get; private set; }

    private ComplexPrice innerPrice;

    public Hunt(
        GameObjectId id,
        string name,
        string description,
        SimplePrice price,
        Requirements requirements,
        RandomReward reward,
        TimeSpan duration,
        HashSet<Lock> locks
    ) : base(id, name, description, locks)
    {
        innerPrice = new(price);
        Requirements = requirements;
        Reward = reward;
        Duration = duration;
    }

    public Hunt(HuntTemplate template, HuntState state) : this(
        template.Id,
        template.Name,
        template.Description,
        template.Price,
        template.Requirements,
        template.Reward,
        template.Duration,
        state.Locks.ToHashSet()
    )
    {
        TimeLeft = state.TimeLeft;
    }

    public Hunt(HuntTemplate template)
    : this(template, new(template.Id, TimeSpan.Zero, template.Locks.ToArray()))
    { }

    public void ModifyPrice(Upgrade upgrade)
        => innerPrice.ApplyUpgrade(upgrade);

    public bool Start()
    {
        if (TimeLeft != TimeSpan.Zero)
        {
            return false;
        }
        TimeLeft = Duration;
        return true;
    }

    public bool SpendTime(TimeSpan deltaT)
    {
        if (TimeLeft > TimeSpan.Zero)
        {
            TimeLeft -= deltaT;
            if (TimeLeft <= TimeSpan.Zero)
            {
                TimeLeft = TimeSpan.Zero;
                return true;
            }
        }
        return false;
    }

    public HuntState SaveState()
        => new(Id, TimeLeft, Locks.ToArray());
}
