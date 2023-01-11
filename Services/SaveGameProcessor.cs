using Microsoft.JSInterop;
using System.Text.Json;

namespace IncrementalSheep;

public class SaveGameProcessor
{
    private readonly IJSRuntime JS;

    public SaveGameProcessor(IJSRuntime js)
    {
        JS = js;
    }

    public async ValueTask SaveGame(GameState state)
    {
        var gameStateDto = new GameStateDto
        {
            LastTick = state.LastTick.Ticks,
            LastDiff = state.LastDiff,
            Resources = state.Resources.AllResources,
            Sheep = state.Sheep.Select(s => s.SaveState()).ToArray(),
            SelectedBuilding = state.SelectedBuilding?.Id,
            Buildings = state.Buildings.Select(b => b.SaveState()).ToArray()
        };
        await JS.InvokeVoidAsync("localStorage.setItem", "data", JsonSerializer.Serialize(gameStateDto));
    }

    public async ValueTask ClearSave()
        => await JS.InvokeVoidAsync("localStorage.removeItem", "data");

    public async ValueTask<string> GetSavedGameString()
        => await JS.InvokeAsync<string>("localStorage.getItem", "data");

    public static GameState LoadGame(string serializedState)
    {
        var gameStateDto = JsonSerializer.Deserialize<GameStateDto>(serializedState);
        var state = new GameState
        {
            LastTick = new DateTime(gameStateDto!.LastTick),
            LastDiff = gameStateDto.LastDiff,
            Resources = new ResourceWarehouse(gameStateDto.Resources),
            Jobs = Templates.Jobs.Select(t => t.Value).ToArray(),
            Hunts = Templates.Hunts.ConvertAll(t => new Hunt(t)),
            Buildings = gameStateDto.Buildings.Select(b => new Building(Templates.Buildings[b.Id], b)).ToArray()
        };

        state.Sheep = gameStateDto.Sheep.Select(s => new Sheep(s.Id, s.Name, state.Jobs.Single(j => j.Id == s.JobId))).ToList();

        if (gameStateDto.SelectedBuilding is not null)
        {
            state.SelectedBuilding = state.Buildings.Single(b => b.Id == gameStateDto.SelectedBuilding);
        }

        return state;
    }
}
