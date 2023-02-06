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
            SelectedStructure = state.SelectedStructure?.Id,
            Structures = state.Structures.Select(b => b.SaveState()).ToArray(),
            Ideas = state.Ideas.Select(i => i.SaveState()).ToArray()
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
        var jobs = Templates.Jobs.Select(t => t.Value).ToArray();
        var state = new GameState
        {
            LastTick = new DateTime(gameStateDto!.LastTick),
            LastDiff = gameStateDto.LastDiff,
            Resources = new ResourceWarehouse(gameStateDto.Resources),
            Jobs = jobs,
            Sheep = gameStateDto
                .Sheep
                .Select(s => new Sheep(
                    s.Id,
                    s.Name,
                    jobs.Single(j => j.Id == s.JobId)))
                .ToList(),
            Hunts = Templates.Hunts.ConvertAll(t => new Hunt(t)),
            Structures = gameStateDto
                .Structures
                .Select(b => ServiceHelpers.StructureFactory(Templates.Buildings[b.Id], b))
                .ToArray(),
            Ideas = gameStateDto
                .Ideas
                .Select(i => new Idea(Templates.Ideas[i.Id], i))
                .ToList()
        };

        if (gameStateDto.SelectedStructure is not null)
        {
            state.SelectedStructure = state
                .Structures
                .Single(b => b.Id == gameStateDto.SelectedStructure);
        }

        return state;
    }
}
