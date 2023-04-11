namespace IncrementalSheep;

public class ResourceWarehouse
{
    private readonly Dictionary<ResourceId, ResourceWithStorage> innerResources;
    private readonly Dictionary<ResourceId, CircularBuffer<double>> productionMemory;

    public double this[ResourceId id] => innerResources.GetValueOrDefault(id)?.Amount ?? 0;

    public IReadOnlyDictionary<ResourceId, ResourceWithStorage> AllResources => innerResources;

    public ResourceWarehouse(IReadOnlyDictionary<ResourceId, ResourceWithStorage> data)
    {
        innerResources = new Dictionary<ResourceId, ResourceWithStorage>();
        productionMemory = new Dictionary<ResourceId, CircularBuffer<double>>();
        foreach (var (id, (amount, storage)) in data)
        {
            innerResources[id] = new(amount, storage);
            productionMemory[id] = new(10);
        }
    }

    public void Add(SimplePrice addition, bool respectStorage = true)
    {
        foreach (var (id, val) in addition.AllResources)
        {
            if (val < 0)
            {
                var newValue = Math.Max(innerResources[id].Amount + val, 0);
                innerResources[id] = new(newValue, innerResources[id].Storage);
                continue;
            }

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

    public CircularBuffer<double> Memory(ResourceId id)
        => productionMemory[id];

    public void Remove(SimplePrice subtraction)
    {
        foreach (var (id, val) in subtraction.AllResources)
        {
            var newValue = Math.Max(innerResources[id].Amount - val, 0);
            innerResources[id] = new(newValue, innerResources[id].Storage);
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

    public void SetStorage(ResourceId resource, double newValue)
        => innerResources[resource] = new(innerResources[resource].Amount, newValue);

    public void StoreProductionPerSecond(SimplePrice production, TimeSpan deltaT)
    {
        foreach (var (id, mem) in productionMemory)
        {
            mem.Add(production[id] / deltaT.TotalSeconds);
        }
    }

    public static bool operator <=(ResourceWarehouse left, SimplePrice right)
        => Enum.GetValues<ResourceId>().All(id => left[id] <= right[id]);

    public static bool operator >=(ResourceWarehouse left, SimplePrice right)
        => Enum.GetValues<ResourceId>().All(id => left[id] >= right[id]);

    public static bool operator <(ResourceWarehouse left, SimplePrice right)
        => Enum.GetValues<ResourceId>().All(id => left[id] < right[id]);

    public static bool operator >(ResourceWarehouse left, SimplePrice right)
        => Enum.GetValues<ResourceId>().All(id => left[id] > right[id]);
}
