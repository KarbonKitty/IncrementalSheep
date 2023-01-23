namespace IncrementalSheep;

public interface ICanBeBuyable
{
    bool IsBuyable { get; }
    SimplePrice Price { get; }
    Requirements Requirements { get; }
}
