using Microsoft.JSInterop;
using System.Text.Json;

namespace IncrementalSheep;

public class GameEngine : IGameEngine
{
    public GameState State { get; set; }

    private readonly IJSRuntime JS;

    public GameEngine(IJSRuntime js)
    {
        JS = js;

        State = new GameState
        {
            LastTick = DateTime.Now,
            LastDiff = 0,
            Resources = new ResourceValue(ResourceId.Wood, 100),
            Sheep = new List<Sheep>(),
            Jobs = Templates.Jobs.Select(t => t.Value).ToArray(),
            Buildings = Templates.Buildings.Select(b => new Building(b.Value, new BuildingState(b.Key, 0))).ToArray()
        };

        /* A bunch of test sheep */

        State.Sheep.Add(new Sheep(1, "Sheep 1", State.Jobs.Single(j => j.Id == SheepJobId.Gatherer)));
        State.Sheep.Add(new Sheep(2, "Sheep 2", State.Jobs.Single(j => j.Id == SheepJobId.Gatherer)));
        State.Sheep.Add(new Sheep(3, "Sheep 3", State.Jobs.Single(j => j.Id == SheepJobId.Hunter)));
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
        => price <= State.Resources;

    public bool TryBuy(Building building) {
        var canAfford = CanAfford(building.Price);
        if (canAfford) {
            State.Resources -= building.Price;
            building.NumberBuilt++;
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

    public async ValueTask<string> GetSavedGameString()
        => await JS.InvokeAsync<string>("localStorage.getItem", "data");

    public void LoadGame(string serializedState)
    {
        var gameStateDto = JsonSerializer.Deserialize<GameStateDto>(serializedState);
        State = new GameState
        {
            LastTick = new DateTime(gameStateDto!.LastTick),
            LastDiff = gameStateDto.LastDiff,
            Resources = new ResourceValue(gameStateDto.Resources),
            Jobs = Templates.Jobs.Select(t => t.Value).ToArray(),
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
        foreach (var building in State.Buildings)
        {
            var resourcesProduced = building.ProductionPerSecond * building.NumberBuilt * deltaT.TotalSeconds;
            State.Resources += resourcesProduced;
        }
        foreach (var sheep in State.Sheep)
        {
            var resourcesProduced = sheep.Job.ProductionPerSecond * deltaT.TotalSeconds;
            State.Resources += resourcesProduced;
        }
    }
}
