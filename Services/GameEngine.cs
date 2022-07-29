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
            Cash = 100,
            Buildings = new [] {
                new Building(BuildingId.WoodGatherer, "Wood gatherer", "Description of wood gatherer", 10, 1),
                new Building(BuildingId.Factory, "Factory", "Factory description", 2000, 15)
            }
        };
    }

    public void ProcessTime(DateTime newTime)
    {
        var deltaT = newTime - State.LastTick;
        State.LastDiff = deltaT.TotalMilliseconds;
        State.LastTick = newTime;

        ProduceCash(deltaT);
    }

    public bool CanAfford(double price) => price <= State.Cash;

    public bool TryBuy(Building building) {
        var canAfford = CanAfford(building.Price);
        if (canAfford) {
            State.Cash -= building.Price;
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
            Cash = State.Cash,
            SelectedBuilding = State.SelectedBuilding?.Id,
            Buildings = State.Buildings.Select(b => b.SaveToDto()).ToArray()
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
            LastTick = new DateTime(gameStateDto.LastTick),
            LastDiff = gameStateDto.LastDiff,
            Cash = gameStateDto.Cash,
            Buildings = gameStateDto.Buildings.Select(b => new Building(b)).ToArray()
        };

        if (gameStateDto.SelectedBuilding is not null)
        {
            State.SelectedBuilding = State.Buildings.Single(b => b.Id == gameStateDto.SelectedBuilding);
        }
    }

    private void ProduceCash(TimeSpan deltaT)
    {
        foreach (var building in State.Buildings)
        {
            var cashProduced = building.ProductionPerSecond * building.NumberBuilt * deltaT.TotalSeconds;
            State.Cash += cashProduced;
        }
    }
}
