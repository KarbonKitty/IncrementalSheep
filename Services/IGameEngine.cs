namespace IncrementalSheep;

public interface IGameEngine
{
    void ProcessTime(DateTime newTime);
    bool TryBuy(Building building);
}
