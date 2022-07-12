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
            LastDiff = 0
        };
    }

    public void ProcessTime(DateTime newTime)
    {
        var deltaT = newTime - State.LastTick;
        State.LastDiff = deltaT.TotalMilliseconds;
        State.LastTick = newTime;
    }

    public async ValueTask SaveGame()
    {
        var gameStateDto = new GameStateDto
        {
            LastTick = State.LastTick.Ticks,
            LastDiff = State.LastDiff
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
            LastDiff = gameStateDto.LastDiff
        };
    }
}
