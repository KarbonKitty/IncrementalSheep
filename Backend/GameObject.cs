namespace IncrementalSheep;

public abstract class GameObject
{
    public string Name { get; }
    public string Description { get; }

    protected GameObject(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
