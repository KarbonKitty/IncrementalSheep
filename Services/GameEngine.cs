namespace IncrementalSheep;

public class GameEngine : IGameEngine
{
    public GameState State { get; set; }

    public string[] Log { get; } = new string[15];
    public int LogIndex { get; private set; }

    public SimplePrice NewSheepPrice => SheepData.NewSheepBasePrice * Math.Pow(1.15, State.Sheep.Count);

    public GameEngine()
    {
        State = new GameState
        {
            LastTick = DateTime.Now,
            LastDiff = 0,
            Resources = new ResourceWarehouse(
                new Dictionary<ResourceId, ResourceWithStorage>
                {
                    { ResourceId.Food, new(100, 300) },
                    { ResourceId.HuntPoints, new(0, 100) },
                    { ResourceId.Wood, new(100, null) }
                }
            ),
            Sheep = new List<Sheep>(),
            Hunts = Templates.Hunts.ConvertAll(t => new Hunt(t)),
            Jobs = Templates.Jobs.Select(t => t.Value).ToArray(),
            Buildings = Templates.Buildings.Select(b => new Building(b.Value, new BuildingState(b.Key, 0))).ToArray()
        };

        // Green pastures starting building

        State.Buildings.Single(b => b.Id == BuildingId.GreenPastures).NumberBuilt = 1;
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
            SheepData.Names[rand.Next(State.Sheep.Count)],
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

        CheckForStarvation(tickProduction, deltaT);

        State.Resources.Add(tickProduction);
    }

    public bool CanAfford(SimplePrice price)
        => State.Resources >= price;

    public bool TryBuy(Building building)
    {
        var isBuildable = building.IsBuildable;
        var canAfford = CanAfford(building.Price);
        if (isBuildable && canAfford)
        {
            State.Resources.Remove(building.Price);
            building.NumberBuilt++;
            State.Resources.AddStorage(building.AdditionalStorage);
            PostMessage($"A new {building.Name} has been built");
            return true;
        }
        return false;
    }

    public bool TryHunt(Hunt hunt)
    {
        var canAfford = CanAfford(hunt.Price);
        if (canAfford)
        {
            State.Resources.Remove(hunt.Price);
            State.Resources.Add(hunt.Reward);
            PostMessage($"Your sheep have finished the {hunt.Name} and brought the rewards back");
            return true;
        }
        return false;
    }

    public void SwitchJobs(Sheep sheep, SheepJob job)
    {
        State.Resources.AddStorage(job.AdditionalStorage);
        State.Resources.RemoveStorage(sheep.Job.AdditionalStorage);
        sheep.SwitchJobs(job);
        PostMessage($"{sheep.Name} is now {sheep.Job.Name}");
    }

    private void CheckForStarvation(SimplePrice tickProduction, TimeSpan deltaT)
    {
        if (tickProduction[ResourceId.Food] < 0)
        {
            var timeToStarvation = State.Resources[ResourceId.Food] / (-tickProduction[ResourceId.Food] / deltaT.TotalSeconds);
            if (timeToStarvation < 60)
            {
                var firstNonFoodProducer = State.Sheep.Find(s => s.Job.ProductionPerSecond[ResourceId.Food] <= 0);
                if (firstNonFoodProducer is null)
                {
                    var lastSheep = State.Sheep[^1];
                    State.Resources.RemoveStorage(lastSheep.Job.AdditionalStorage);
                    State.Sheep.Remove(lastSheep);
                    PostMessage($"Your sheep are hungry! {lastSheep.Name} decides to leave the tribe!");
                }
                else
                {
                    PostMessage($"Your sheep are hungry! {firstNonFoodProducer.Name} decides to gather some food for themselves!");
                    firstNonFoodProducer?.SwitchJobs(State.Jobs.Single(j => j.Id == SheepJobId.Gatherer));
                }
            }
        }
    }

    private SimplePrice ProduceResources(TimeSpan deltaT)
    {
        var totalProducedResources = new SimplePrice();
        foreach (var building in State.Buildings)
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

    private SimplePrice FeedTheSheep(TimeSpan deltaT)
        => SheepData.SheepBaseConsumption * State.Sheep.Count * deltaT.TotalSeconds;
}
