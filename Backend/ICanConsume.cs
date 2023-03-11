namespace IncrementalSheep;

public interface ICanConsume
{
    ComplexPrice? ConsumptionPerSecond { get; }
}
