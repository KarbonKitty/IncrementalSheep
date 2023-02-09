namespace IncrementalSheep;

using System.Linq;

public class GameEngine : IGameEngine
{
    private readonly IToastService _toastService;

    public GameState State { get; set; }

    public string[] Log { get; } = new string[15];
    public int LogIndex { get; private set; }

    public SimplePrice NewSheepPrice => SheepData.NewSheepBasePrice * Math.Pow(1.15, State.Sheep.Count);

    private IEnumerable<GameObject> AllGameObjects
        => State.Hunts
            .Concat<GameObject>(State.Structures)
            .Concat(State.Jobs)
            .Concat(State.Ideas);

    public GameEngine(IToastService toastService)
    {
        _toastService = toastService;

        State = new GameState
        {
            LastTick = DateTime.Now,
            LastDiff = 0,
            Resources = new ResourceWarehouse(
                new Dictionary<ResourceId, ResourceWithStorage>
                {
                    { ResourceId.Food, new(100, 300) },
                    { ResourceId.Folklore, new(0, 10) },
                    { ResourceId.Wood, new(100, null) }
                }
            ),
            Sheep = new List<Sheep>(),
            Hunts = Templates.Hunts.ConvertAll(t => new Hunt(t)),
            Jobs = Templates.Jobs.Select(t => t.Value).ToArray(),
            Structures = Templates.Buildings.Select(b => ServiceHelpers.StructureFactory(b.Value)).ToArray(),
            Ideas = Templates.Ideas.Select(i => new Idea(i.Value)).ToList(),
            XoshiroState = new ulong[4] { 1, 2, 3, (ulong)DateTime.Now.Ticks }
        };

        // Green pastures starting building

        State.Structures.Single(b => b.Id == StructureId.GreenPastures).NumberBuilt = 1;
    }

    public void PostMessage(string message)
    {
        --LogIndex;
        if (LogIndex < 0)
        {
            LogIndex = Log.Length - 1;
        }
        Log[LogIndex] = message;
    }

    public void RecruitNewSheep()
    {
        var canAfford = State.Resources >= NewSheepPrice;
        if (!canAfford)
        {
            return;
        }
        var rand = new Random();
        State.Resources.Remove(NewSheepPrice);
        var lastId = State.Sheep.Any() ? State.Sheep.Max(s => s.Id) : 1;
        State.Sheep.Add(new Sheep(
            lastId + 1,
            SheepData.Names[rand.Next(SheepData.Names.Length)],
            State.Jobs.Single(j => j.Id == SheepJobId.Gatherer)));
        PostMessage($"New sheep named {State.Sheep.Last().Name} joins the tribe!");
    }

    public void ProcessTime(DateTime newTime)
    {
        var deltaT = newTime - State.LastTick;
        if (deltaT.TotalMilliseconds > 1000)
        {
            deltaT = TimeSpan.FromMilliseconds(1000);
        }
        State.LastDiff = deltaT.TotalMilliseconds;
        State.LastTick += deltaT;

        var tickProduction = ProduceResources(deltaT);
        tickProduction -= FeedTheSheep(deltaT);

        foreach (var hunt in State.Hunts)
        {
            var isFinished = hunt.SpendTime(deltaT);
            if (isFinished)
            {
                FinishHunt(hunt);
            }
        }

        CheckForStarvation(tickProduction, deltaT);

        State.Resources.Add(tickProduction);
    }

    public bool CanBuy(IBuyable buyable)
        => CanAfford(buyable) && FulfillsRequirements(buyable.Requirements);

    public bool CanAfford(IBuyable buyable)
        => State.Resources >= buyable.Price;

    public bool TryBuy(Building building)
    {
        var canBuy = CanBuy(building);
        if (canBuy)
        {
            State.Resources.Remove(building.Price);
            building.NumberBuilt++;
            State.Resources.AddStorage(building.AdditionalStorage);
            PostMessage($"A new {building.Name} has been built");
            return true;
        }
        return false;
    }

    public bool TryBuy(Idea idea)
    {
        var canBuy = CanBuy(idea);
        var isBought = idea.IsBought;
        if(canBuy && !isBought)
        {
            State.Resources.Remove(idea.Price);
            idea.Buy();
            PostMessage($"Your sheep have invented {idea.Name}!");
            ProcessUnlocking(idea);
            return true;
        }
        return false;
    }

    public bool TryHunt(Hunt hunt)
    {
        var canAfford = CanAfford(hunt);
        var fulfillsRequirements = FulfillsRequirements(hunt.Requirements);
        var isNotRunning = hunt.TimeLeft <= TimeSpan.Zero;
        if (canAfford && fulfillsRequirements && isNotRunning)
        {
            State.Resources.Remove(hunt.Price);
            var unlockedHunters = State
                .Sheep
                .Where(s => s.Job.Id == SheepJobId.Hunter && !s.JobState.Locked)
                .Take(hunt.Requirements.NumberOfHunters);
            foreach (var hunter in unlockedHunters)
            {
                hunter.LockJob();
            }
            hunt.Start();
            PostMessage($"Your sheep have started the {hunt.Name}");
            return true;
        }
        return false;
    }

    public bool SwitchJobs(Sheep sheep, SheepJob job)
    {
        if (sheep.JobState.Locked)
        {
            PostMessage($"{sheep.Name} can't switch jobs now!");
            return false;
        }
        State.Resources.AddStorage(job.AdditionalStorage);
        State.Resources.RemoveStorage(sheep.Job.AdditionalStorage);
        sheep.SwitchJobs(job);
        PostMessage($"{sheep.Name} is now {sheep.Job.Name}");
        return true;
    }

    // This and next func based on public domain implementation
    // of xoshiro256** from https://prng.di.unimi.it/
    private static ulong RotateLeft(ulong x, int k)
        => (x << k) | (x >> (64 - k));

    private int Next()
    {
        ulong result = RotateLeft(State.XoshiroState[1] * 5, 7) * 9;

        ulong t = State.XoshiroState[1] << 17;

        State.XoshiroState[2] ^= State.XoshiroState[0];
        State.XoshiroState[3] ^= State.XoshiroState[1];
        State.XoshiroState[1] ^= State.XoshiroState[2];
        State.XoshiroState[0] ^= State.XoshiroState[3];

        State.XoshiroState[2] ^= t;

        State.XoshiroState[3] ^= RotateLeft(State.XoshiroState[3], 45);

        return (int)result;
    }

    private void FinishHunt(Hunt hunt)
    {
        var lockedHunters = State
            .Sheep
            .Where(s => s.Job.Id == SheepJobId.Hunter && s.JobState.Locked)
            .TakeLast(hunt.Requirements.NumberOfHunters);
        if (lockedHunters.Count() >= hunt.Requirements.NumberOfHunters)
        {
            PostMessage($"Your sheep have finished the {hunt.Name} and brought the rewards back");
            State.Resources.Add(hunt.Reward);
        }
        else
        {
            PostMessage($"Your sheep have returned from tht {hunt.Name}, but some have left in the meantime, so they failed to bring any rewards");
        }
        foreach (var hunter in lockedHunters)
        {
            hunter.UnlockJob();
        }
    }

    private void CheckForStarvation(SimplePrice tickProduction, TimeSpan deltaT)
    {
        if (tickProduction[ResourceId.Food] < 0)
        {
            var foodDebtPerSecond = Math.Abs(tickProduction[ResourceId.Food] / deltaT.TotalSeconds);
            var timeToStarvation = State.Resources[ResourceId.Food] / foodDebtPerSecond;

            if (timeToStarvation >= 60)
            {
                return;
            }

            var sheepThatDontProduceFood = State.Sheep.Where(s => s.Job.ProductionPerSecond[ResourceId.Food] <= 0);
            var unlockedNonFoodProducer = sheepThatDontProduceFood.FirstOrDefault(s => !s.JobState.Locked);

            if (unlockedNonFoodProducer is not null)
            {
                SwitchToFoodGathering(unlockedNonFoodProducer);
            }

            var firstNonFoodProducer = sheepThatDontProduceFood.FirstOrDefault();

            if (firstNonFoodProducer is null)
            {
                var lastSheep = State.Sheep[^1];
                State.Resources.RemoveStorage(lastSheep.Job.AdditionalStorage);
                State.Sheep.Remove(lastSheep);
                PostMessage($"Your sheep are hungry! {lastSheep.Name} decides to leave the tribe!");
            }
            else
            {
                SwitchToFoodGathering(firstNonFoodProducer);
            }
        }

        void SwitchToFoodGathering(Sheep sheep)
        {
            PostMessage($"Your sheep are hungry! {sheep.Name} decides to gather some food for themselves!");
            sheep.SwitchJobs(State.Jobs.Single(j => j.Id == SheepJobId.Gatherer));
            sheep.UnlockJob();
        }
    }

    private void ProcessUnlocking(ICanUnlock unlocker)
    {
        if (unlocker.LockToRemove is null)
        {
            return;
        }
        foreach (var go in AllGameObjects.Where(go => go.Locks.Contains(unlocker.LockToRemove.Value)))
        {
            var (removed, unlocked) = go.RemoveLock(unlocker.LockToRemove.Value);
            if (removed && unlocked)
            {
                var msg = $"{go.Name} has been unlocked!";
                _toastService.ShowToast(msg);
                PostMessage($"💡 {msg}");
            }
        }
    }

    private SimplePrice ProduceResources(TimeSpan deltaT)
    {
        var totalProducedResources = new SimplePrice();
        foreach (var building in State.Structures)
        {
            var resourcesProduced = building.ProductionPerSecond * building.NumberBuilt * deltaT.TotalSeconds;
            totalProducedResources += resourcesProduced;
        }
        foreach (var sheep in State.Sheep)
        {
            var resourcesProduced = sheep.Job.ProductionPerSecond * deltaT.TotalSeconds;
            totalProducedResources += resourcesProduced;
        }
        return totalProducedResources;
    }

    private bool FulfillsRequirements(Requirements req)
    {
        var enoughHunters = State.Sheep.Count(s => s.Job.Id == SheepJobId.Hunter && !s.JobState.Locked) >= req.NumberOfHunters;
        return enoughHunters;
    }

    private SimplePrice FeedTheSheep(TimeSpan deltaT)
        => SheepData.SheepBaseConsumption * State.Sheep.Count * deltaT.TotalSeconds;
}
