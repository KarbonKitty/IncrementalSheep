namespace IncrementalSheep;

public class SimplePrice
{
    private readonly Dictionary<ResourceId, double> innerResources;

    public double this[ResourceId id] => innerResources.GetValueOrDefault(id);

    public IReadOnlyDictionary<ResourceId, double> AllResources => innerResources;

    public SimplePrice()
    {
        innerResources = new Dictionary<ResourceId, double>();
    }

    public SimplePrice(ResourceId id, double value)
    {
        innerResources = new Dictionary<ResourceId, double> { { id, value } };
    }

    public SimplePrice(SimplePrice value)
    {
        innerResources = new Dictionary<ResourceId, double>(value.AllResources);
    }

    public SimplePrice(IReadOnlyDictionary<ResourceId, double> dict)
    {
        innerResources = new Dictionary<ResourceId, double>(dict);
    }

    public static SimplePrice operator *(SimplePrice resources, double scale)
        => new(resources.AllResources.Select(res => (id: res.Key, val: res.Value * scale)).ToDictionary(t => t.id, t => t.val));

    public static SimplePrice operator +(SimplePrice left, SimplePrice right)
        => new(Enum.GetValues<ResourceId>().Select(id => (id, val: left[id] + right[id])).ToDictionary(t => t.id, t => t.val));

    public static SimplePrice operator -(SimplePrice left, SimplePrice right)
        => new(Enum.GetValues<ResourceId>().Select(id => (id, val: left[id] - right[id])).ToDictionary(t => t.id, t => t.val));

    public static bool operator <=(SimplePrice left, SimplePrice right)
        => Enum.GetValues<ResourceId>().All(id => left[id] <= right[id]);

    public static bool operator >=(SimplePrice left, SimplePrice right)
        => Enum.GetValues<ResourceId>().All(id => left[id] >= right[id]);

    public static bool operator <(SimplePrice left, SimplePrice right)
        => Enum.GetValues<ResourceId>().All(id => left[id] < right[id]);

    public static bool operator >(SimplePrice left, SimplePrice right)
        => Enum.GetValues<ResourceId>().All(id => left[id] > right[id]);
}