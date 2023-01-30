namespace IncrementalSheep;

public class Hunt : GameObject, IBuyable, ITakeTime
{
    public HuntId Id { get; }

    public SimplePrice Price { get; }
    public Requirements Requirements { get; }
    public SimplePrice Reward { get; }

    public TimeSpan Duration { get; }

    public TimeSpan TimeLeft { get; private set; }

    public Hunt(
        HuntId id,
        string name,
        string description,
        SimplePrice price,
        Requirements requirements,
        SimplePrice reward,
        TimeSpan duration
    ) : base(name, description)
    {
        Id = id;
        Price = price;
        Requirements = requirements;
        Reward = reward;
        Duration = duration;
    }

    public Hunt(HuntTemplate template) : this(
        template.Id,
        template.Name,
        template.Description,
        template.Price,
        template.Requirements,
        template.Reward,
        template.Duration
    ) {}

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
}
