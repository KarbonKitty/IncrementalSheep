namespace IncrementalSheep;

public class ResourceWarehouse
{
    private readonly Dictionary<ResourceId, (double amount, double? storage)> innerResources;

    public double this[ResourceId id] => innerResources.GetValueOrDefault(id).amount;

    public IReadOnlyDictionary<ResourceId, (double amount, double? storage)> AllResources => innerResources;

    public ResourceWarehouse(IReadOnlyDictionary<ResourceId, (double, double?)> data)
    {
        innerResources = new Dictionary<ResourceId, (double amount, double? storage)>();
        foreach (var kvp in data)
        {
            innerResources[kvp.Key] = kvp.Value;
        }
    }

    public static ResourceWarehouse operator +(ResourceWarehouse left, ResourceValue right)
        => new(Enum.GetValues<ResourceId>().Select(id => (id, val: left[id] + right[id], str: left.AllResources[id].storage)).ToDictionary(t => t.id, t => (t.val, t.str)));

    public static ResourceWarehouse operator -(ResourceWarehouse left, ResourceValue right)
        => new(Enum.GetValues<ResourceId>().Select(id => (id, val: left[id] - right[id], str: left.AllResources[id].storage)).ToDictionary(t => t.id, t => (t.val, t.str)));

    public static bool operator <=(ResourceWarehouse left, ResourceValue right)
        => Enum.GetValues<ResourceId>().All(id => left[id] <= right[id]);

    public static bool operator >=(ResourceWarehouse left, ResourceValue right)
        => Enum.GetValues<ResourceId>().All(id => left[id] >= right[id]);

    public static bool operator <(ResourceWarehouse left, ResourceValue right)
        => Enum.GetValues<ResourceId>().All(id => left[id] < right[id]);

    public static bool operator >(ResourceWarehouse left, ResourceValue right)
        => Enum.GetValues<ResourceId>().All(id => left[id] > right[id]);
}
