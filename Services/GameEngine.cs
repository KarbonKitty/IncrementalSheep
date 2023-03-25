namespace IncrementalSheep;

using System.Linq;

public class GameEngine : IGameEngine
{
    private readonly IToastService _toastService;

    public GameState State { get; set; }

    public CircularBuffer<string> Log { get; } = new(15);

    public SimplePrice NewSheepPrice
        => SheepData.NewSheepBasePrice * Math.Pow(1.15, State.Sheep.Count);

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
                    { ResourceId.Wood, new(0, null) }
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

        State.Structures.Single(b => b.Id == GameObjectId.GreenPastures).NumberBuilt = 1;
    }

    public string GameObjectName(GameObjectId id)
        => AllGameObjects.FirstOrDefault(go => go.Id == id)?.Name ?? "NO SUCH OBJECT";

    public void PostMessage(string message)
        => Log.Add(message);

    public void IngestLoadedState(GameState state)
    {
        State = state;
        foreach (var idea in State.Ideas.Where(i => i.IsBought && i.Upgrade is not null))
        {
            ProcessUpgrades(idea);
        }
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
            State.Jobs.Single(j => j.Id == GameObjectId.FoodGatherer)));
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
        State.Resources.StoreProductionPerSecond(tickProduction, deltaT);
    }

    public bool CanBuy(IBuyable buyable)
        => CanPay(buyable.Price) && FulfillsRequirements(buyable.Requirements);

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
        if (canBuy && !isBought)
        {
            State.Resources.Remove(idea.Price);
            idea.Buy();
            PostMessage($"Your sheep have invented {idea.Name}!");
            ProcessUnlocking(idea);
            ProcessUpgrades(idea);
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
                .Where(s => s.Job.Id == GameObjectId.Hunter && !s.JobState.Locked)
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
        if (job.GetPrice is not null)
        {
            var price = job.GetPrice(State);
            var canAfford = CanPay(price);
            if (!canAfford)
            {
                PostMessage($"You can't afford the training cost for {job.Name}!");
                return false;
            }
            State.Resources.Remove(price);
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

    private double NextNormalized()
    {
        var x = (uint)Next();
        return x / Convert.ToDouble(uint.MaxValue);
    }

    private void FinishHunt(Hunt hunt)
    {
        var lockedHunters = State
            .Sheep
            .Where(s => s.Job.Id == GameObjectId.Hunter && s.JobState.Locked)
            .TakeLast(hunt.Requirements.NumberOfHunters);
        if (lockedHunters.Count() >= hunt.Requirements.NumberOfHunters)
        {
            var actualReward = GetActualReward(hunt.Reward);
            var formattedReward = string.Join(
                ", ",
                actualReward
                    .AllResources
                    .Select(kvp => $"{kvp.Value:N2} {EnumNames.GetName(kvp.Key)}"));
            PostMessage($"Your sheep have finished the {hunt.Name} and brought back: {formattedReward}");
            State.Resources.Add(actualReward);
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

    private bool CanPay(SimplePrice price)
        => State.Resources >= price;

    private bool CanAfford(IBuyable buyable)
        => CanPay(buyable.Price);

    private SimplePrice GetActualReward(RandomReward randomReward)
    {
        var result = new SimplePrice();
        foreach (var item in randomReward.Items)
        {
            if (NextNormalized() < item.Chance)
            {
                var s = NextNormalized();
                result += new SimplePrice(
                    item.Resource,
                    item.Minimum + ((item.Maximum - item.Minimum) * s));
            }
        }
        return result;
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

            var sheepThatDontProduceFood = State.Sheep.Where(s => s.Job.ProductionPerSecond.Total()[ResourceId.Food] <= 0);
            var unlockedNonFoodProducer = sheepThatDontProduceFood.FirstOrDefault(s => !s.JobState.Locked);

            if (unlockedNonFoodProducer is not null)
            {
                SwitchToFoodGathering(unlockedNonFoodProducer);
                return;
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
            State.Resources.RemoveStorage(sheep.Job.AdditionalStorage);
            sheep.SwitchJobs(State.Jobs.Single(j => j.Id == GameObjectId.FoodGatherer));
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

    private void ProcessUpgrades(ICanUpgrade upgrader)
    {
        if (upgrader.Upgrade is null)
        {
            return;
        }

        var upgrade = upgrader.Upgrade;
        var upgradee = AllGameObjects.Single(go => go.Id == upgrader.Upgrade.Upgradee);

        var result = upgrade.Property switch
        {
            UpgradeProperty.Production => UpgradeProduction(upgrade, upgradee),
            UpgradeProperty.Price => UpgradePrice(upgrade, upgradee),
            UpgradeProperty.Consumption => UpgradeConsumption(upgrade, upgradee),
            _ => throw new Exception("Missing upgrade property handler")
        };

        bool UpgradeProduction(Upgrade upgrade, GameObject upgradee)
        {
            if (upgradee is ICanProduce producer)
            {
                if (producer.ProductionPerSecond is null)
                {
                    throw new ArgumentException($"Can't change production of {upgradee.Name}");
                }
                producer.ProductionPerSecond.ApplyUpgrade(upgrade);
                PostMessage($"{upgradee.Name} has been upgraded!");
                return true;
            }

            throw new ArgumentException("Can't upgrade a non-structure");
        }

        bool UpgradePrice(Upgrade upgrade, GameObject upgradee)
        {
            if (upgradee is IBuyable buyable)
            {
                buyable.ModifyPrice(upgrade);
                PostMessage($"{upgradee.Name} has been upgraded!");
                return true;
            }

            throw new ArgumentException("Can't change price of a non-buyable!");
        }

        bool UpgradeConsumption(Upgrade upgrade, GameObject upgradee)
        {
            if (upgradee is ICanConsume consumer)
            {
                if (consumer.ConsumptionPerSecond is null)
                {
                    throw new ArgumentException($"Can't change consumption of a {upgradee.Name}");
                }
                consumer.ConsumptionPerSecond.ApplyUpgrade(upgrade);
                PostMessage($"{upgradee.Name} has been upgraded!");
                return true;
            }

            throw new ArgumentException("Can't change consumption of non-consumer");
        }
    }

    private SimplePrice ProduceResources(TimeSpan deltaT)
    {
        var totalProducedResources = new SimplePrice();

        foreach (var building in State.Structures)
        {
            var canProduce = true;

            var isConsumer = building.ConsumptionPerSecond is not null;
            if (isConsumer)
            {
                var resourcesConsumed = building.ConsumptionPerSecond!.Total() * building.NumberBuilt * deltaT.TotalSeconds;
                if (CanPay(resourcesConsumed))
                {
                    totalProducedResources -= resourcesConsumed;
                }
                else
                {
                    canProduce = false;
                }
            }

            if (canProduce)
            {
                var resourcesProduced = building.ProductionPerSecond.Total() * building.NumberBuilt * deltaT.TotalSeconds;
                totalProducedResources += resourcesProduced;
            }
        }

        foreach (var sheep in State.Sheep)
        {
            var resourcesProduced = sheep.Job.ProductionPerSecond.Total() * deltaT.TotalSeconds;
            totalProducedResources += resourcesProduced;
        }
        return totalProducedResources;
    }

    private bool FulfillsRequirements(Requirements req)
    {
        var enoughHunters = State.Sheep.Count(s => s.Job.Id == GameObjectId.Hunter && !s.JobState.Locked) >= req.NumberOfHunters;
        return enoughHunters;
    }

    private SimplePrice FeedTheSheep(TimeSpan deltaT)
        => SheepData.SheepBaseConsumption * State.Sheep.Count * deltaT.TotalSeconds;
}
