namespace IncrementalSheep;

public interface IGameEngine
{
    void ProcessTime(DateTime newTime);
    bool CanAfford(SimplePrice price);
    bool TryBuy(Building building);
    ValueTask SaveGame();
    ValueTask<string> GetSavedGameString();
    void LoadGame(string serializedState);
}
