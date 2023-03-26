namespace IncrementalSheep;

public class PriceMultiplier
{
    private readonly Dictionary<ResourceId, double> multipliers = new();

    public double this[ResourceId id] => multipliers.GetValueOrDefault(id, 1.0);

    public IReadOnlyDictionary<ResourceId, double> AllMultipliers => multipliers;

    public PriceMultiplier()
    {
        multipliers = new Dictionary<ResourceId, double>();
    }

    public PriceMultiplier(ResourceId id, double value)
    {
        multipliers = new Dictionary<ResourceId, double> { { id, value } };
    }

    public PriceMultiplier(params (ResourceId id, double value)[] items)
    {
        multipliers = items.ToDictionary(i => i.id, i => i.value);
    }

    public PriceMultiplier(PriceMultiplier value)
    {
        multipliers = new Dictionary<ResourceId, double>(value.AllMultipliers);
    }

    public PriceMultiplier(IReadOnlyDictionary<ResourceId, double> dict)
    {
        multipliers = new Dictionary<ResourceId, double>(dict);
    }

    public void AddMultiplier(ResourceId id, double scale)
        => multipliers[id] = this[id] + (scale - 1.0);

    public void AddMultiplier(PriceMultiplier multiplier)
    {
        foreach (var (id, scale) in multiplier.AllMultipliers)
        {
            AddMultiplier(id, scale);
        }
    }
}
