namespace IncrementalSheep;

public interface ICanBeBuyable
{
    SimplePrice Price { get; }
    Requirements Requirements { get; }
}
