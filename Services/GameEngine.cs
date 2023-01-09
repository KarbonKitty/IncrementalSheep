using Microsoft.JSInterop;
using System.Text.Json;

namespace IncrementalSheep;

public class GameEngine : IGameEngine
{
    public GameState State { get; set; }

    public ResourceValue NewSheepPrice => NewSheepBasePrice * Math.Pow(1.15, State.Sheep.Count);

    private readonly IJSRuntime JS;
    private readonly ResourceValue NewSheepBasePrice = new(ResourceId.Food, 100);

    public GameEngine(IJSRuntime js)
    {
        JS = js;

        State = new GameState
        {
            LastTick = DateTime.Now,
            LastDiff = 0,
            Resources = new ResourceWarehouse(
                new Dictionary<ResourceId, (double, double?)>
                {
                    { ResourceId.Food, (100, null) },
                    { ResourceId.HuntPoints, (0, 100) },
                    { ResourceId.Wood, (100, 200) }
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

    public bool CanAfford(ResourceValue price)
        => State.Resources >= price;

    public bool TryBuy(Building building)
    {
        var isBuildable = building.IsBuildable;
        var canAfford = CanAfford(building.Price);
        if (isBuildable && canAfford) {
            State.Resources -= building.Price;
            building.NumberBuilt++;
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

    public async ValueTask SaveGame()
    {
        var gameStateDto = new GameStateDto
        {
            LastTick = State.LastTick.Ticks,
            LastDiff = State.LastDiff,
            Resources = State.Resources.AllResources,
            Sheep = State.Sheep.Select(s => s.SaveState()).ToArray(),
            SelectedBuilding = State.SelectedBuilding?.Id,
            Buildings = State.Buildings.Select(b => b.SaveState()).ToArray()
        };
        await JS.InvokeVoidAsync("localStorage.setItem", "data", JsonSerializer.Serialize(gameStateDto));
    }

    public async ValueTask ClearSave()
        => await JS.InvokeVoidAsync("localStorage.removeItem", "data");

    public async ValueTask<string> GetSavedGameString()
        => await JS.InvokeAsync<string>("localStorage.getItem", "data");

    public void LoadGame(string serializedState)
    {
        var gameStateDto = JsonSerializer.Deserialize<GameStateDto>(serializedState);
        State = new GameState
        {
            LastTick = new DateTime(gameStateDto!.LastTick),
            LastDiff = gameStateDto.LastDiff,
            Resources = new ResourceWarehouse(gameStateDto.Resources),
            Jobs = Templates.Jobs.Select(t => t.Value).ToArray(),
            Hunts = Templates.Hunts.ConvertAll(t => new Hunt(t)),
            Buildings = gameStateDto.Buildings.Select(b => new Building(Templates.Buildings[b.Id], b)).ToArray()
        };

        State.Sheep = gameStateDto.Sheep.Select(s => new Sheep(s.Id, s.Name, State.Jobs.Single(j => j.Id == s.JobId))).ToList();

        if (gameStateDto.SelectedBuilding is not null)
        {
            State.SelectedBuilding = State.Buildings.Single(b => b.Id == gameStateDto.SelectedBuilding);
        }
    }

    private void ProduceResources(TimeSpan deltaT)
    {
        var totalProducedResources = new ResourceValue();
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
