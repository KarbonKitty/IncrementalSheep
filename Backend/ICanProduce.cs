namespace IncrementalSheep;

public interface ICanProduce
{
    ComplexPrice? ProductionPerSecond { get; }
}
