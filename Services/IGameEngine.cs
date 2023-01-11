namespace IncrementalSheep;

public interface IGameEngine
{
    void ProcessTime(DateTime newTime);
    bool CanAfford(SimplePrice price);
    bool TryBuy(Building building);
}
