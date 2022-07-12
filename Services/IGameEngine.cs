using System;
using System.Threading.Tasks;

namespace IncrementalSheep;

public interface IGameEngine
{
    void ProcessTime(DateTime newTime);
    ValueTask SaveGame();
    ValueTask<string> GetSavedGameString();
    void LoadGame(string serializedState);
}
