namespace IncrementalSheep;

public interface IBuyable
{
    SimplePrice Price { get; }
    Requirements Requirements { get; }
    void ModifyPrice(Upgrade upgrade);
}
