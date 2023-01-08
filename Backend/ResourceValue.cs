namespace IncrementalSheep;

public class ResourceValue
{
    private readonly Dictionary<ResourceId, double> innerResources;

    public double this[ResourceId id] => innerResources.GetValueOrDefault(id);

    public IReadOnlyDictionary<ResourceId, double> AllResources => innerResources;

    public ResourceValue()
    {
        innerResources = new Dictionary<ResourceId, double>();
    }

    public ResourceValue(ResourceId id, double value)
    {
        innerResources = new Dictionary<ResourceId, double> { { id, value } };
    }

    public ResourceValue(ResourceValue value)
    {
        innerResources = new Dictionary<ResourceId, double>(value.AllResources);
    }

    public ResourceValue(IReadOnlyDictionary<ResourceId, double> dict)
    {
        innerResources = new Dictionary<ResourceId, double>(dict);
    }

    public static ResourceValue operator *(ResourceValue resources, double scale)
        => new(resources.AllResources.Select(res => (id: res.Key, val: res.Value * scale)).ToDictionary(t => t.id, t => t.val));

    public static ResourceValue operator +(ResourceValue left, ResourceValue right)
        => new(Enum.GetValues<ResourceId>().Select(id => (id, val: left[id] + right[id])).ToDictionary(t => t.id, t => t.val));

    public static ResourceValue operator -(ResourceValue left, ResourceValue right)
        => new(Enum.GetValues<ResourceId>().Select(id => (id, val: left[id] - right[id])).ToDictionary(t => t.id, t => t.val));

    public static bool operator <=(ResourceValue left, ResourceValue right)
        => Enum.GetValues<ResourceId>().All(id => left[id] <= right[id]);

    public static bool operator >=(ResourceValue left, ResourceValue right)
        => Enum.GetValues<ResourceId>().All(id => left[id] >= right[id]);

    public static bool operator <(ResourceValue left, ResourceValue right)
        => Enum.GetValues<ResourceId>().All(id => left[id] < right[id]);

    public static bool operator >(ResourceValue left, ResourceValue right)
        => Enum.GetValues<ResourceId>().All(id => left[id] > right[id]);
}
