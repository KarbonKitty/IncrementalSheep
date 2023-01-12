namespace IncrementalSheep;

public class GameEngine : IGameEngine
{
    public GameState State { get; set; }

    public SimplePrice NewSheepPrice => NewSheepBasePrice * Math.Pow(1.15, State.Sheep.Count);

    private readonly SimplePrice NewSheepBasePrice = new(ResourceId.Food, 100);

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
                    { ResourceId.Wood, new(100, 200) }
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

    public void RecruitNewSheep()
    {
        var canAfford = State.Resources >= NewSheepPrice;
        if (!canAfford)
        {
            return;
        }
        var rand = new Random();
        State.Resources -= NewSheepPrice;
        var lastId = State.Sheep.Any() ? State.Sheep.Max(s => s.Id) : 1;
        State.Sheep.Add(new Sheep(
            lastId + 1,
            SheepNames.Names[rand.Next(State.Sheep.Count)],
            State.Jobs.Single(j => j.Id == SheepJobId.Gatherer)));
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

        ProduceResources(deltaT);
    }

    public bool CanAfford(SimplePrice price)
        => State.Resources >= price;

    public bool TryBuy(Building building)
    {
        var isBuildable = building.IsBuildable;
        var canAfford = CanAfford(building.Price);
        if (isBuildable && canAfford) {
            State.Resources -= building.Price;
            building.NumberBuilt++;
            State.Resources.AddStorage(building.AdditionalStorage);
            return true;
        }
        return false;
    }

    public bool TryHunt(Hunt hunt)
    {
        var canAfford = CanAfford(hunt.Price);
        if (canAfford)
        {
            State.Resources -= hunt.Price;
            State.Resources.Add(hunt.Reward);
            return true;
        }
        return false;
    }

    public void SwitchJobs(Sheep sheep, SheepJob job)
    {
        State.Resources.AddStorage(job.AdditionalStorage);
        State.Resources.RemoveStorage(sheep.Job.AdditionalStorage);
        sheep.SwitchJobs(job);
    }

    private void ProduceResources(TimeSpan deltaT)
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
        State.Resources.Add(totalProducedResources);
    }
}
