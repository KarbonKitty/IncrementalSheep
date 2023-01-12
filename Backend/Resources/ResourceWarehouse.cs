namespace IncrementalSheep;

public class ResourceWarehouse
{
    private readonly Dictionary<ResourceId, ResourceWithStorage> innerResources;

    public double this[ResourceId id] => innerResources.GetValueOrDefault(id)?.Amount ?? 0;

    public IReadOnlyDictionary<ResourceId, ResourceWithStorage> AllResources => innerResources;

    public ResourceWarehouse(IReadOnlyDictionary<ResourceId, ResourceWithStorage> data)
    {
        innerResources = new Dictionary<ResourceId, ResourceWithStorage>();
        foreach (var (id, (amount, storage)) in data)
        {
            innerResources[id] = new(amount, storage);
        }
    }

    public void Add(SimplePrice addition, bool respectStorage = true)
    {
        foreach (var (id, val) in addition.AllResources)
        {
            if (innerResources[id].Storage is null)
            {
                innerResources[id] = new(innerResources[id].Amount + val, null);
            }
            else
            {
                double newValue;
                if (respectStorage)
                {
                    var oldValue = innerResources[id].Amount;
                    if (oldValue > innerResources[id].Storage!.Value)
                    {
                        newValue = oldValue;
                    }
                    else
                    {
                        newValue = Math.Min(innerResources[id].Amount + val, innerResources[id].Storage!.Value);
                    }
                }
                else
                {
                    newValue = innerResources[id].Amount + val;
                }
                innerResources[id] = new(newValue, innerResources[id].Storage);
            }
        }
    }

    public void AddStorage(SimplePrice? addition)
    {
        if (addition is null)
        {
            return;
        }

        foreach (var (resId, additionalStorage) in addition.AllResources)
        {
            innerResources[resId] = new(innerResources[resId].Amount, innerResources[resId].Storage + additionalStorage);
        }
    }

    public void RemoveStorage(SimplePrice? subtraction)
    {
        if (subtraction is null)
        {
            return;
        }

        foreach (var (resId, removeStorage) in subtraction.AllResources)
        {
            innerResources[resId] = new(innerResources[resId].Amount, innerResources[resId].Storage - removeStorage);
        }
    }

    public static ResourceWarehouse operator -(ResourceWarehouse left, SimplePrice right)
        => new(Enum.GetValues<ResourceId>().Select(id => (id, val: left[id] - right[id], str: left.AllResources[id].Storage)).ToDictionary(t => t.id, t => new ResourceWithStorage(t.val, t.str)));

    public static bool operator <=(ResourceWarehouse left, SimplePrice right)
        => Enum.GetValues<ResourceId>().All(id => left[id] <= right[id]);

    public static bool operator >=(ResourceWarehouse left, SimplePrice right)
        => Enum.GetValues<ResourceId>().All(id => left[id] >= right[id]);

    public static bool operator <(ResourceWarehouse left, SimplePrice right)
        => Enum.GetValues<ResourceId>().All(id => left[id] < right[id]);

    public static bool operator >(ResourceWarehouse left, SimplePrice right)
        => Enum.GetValues<ResourceId>().All(id => left[id] > right[id]);
}
