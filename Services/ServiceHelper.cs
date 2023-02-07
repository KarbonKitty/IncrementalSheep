namespace IncrementalSheep;

public static class ServiceHelpers
{
    public static Structure StructureFactory(StructureTemplate template)
        => template switch
        {
            BuildingTemplate bt => new Building(bt, new(bt.Id, 0, bt.Locks)),
            _ => new Structure(template, new(template.Id, 0, template.Locks))
        };

    public static Structure StructureFactory(StructureTemplate template, StructureState state)
        => template switch
        {
            BuildingTemplate bt => new Building(bt, state),
            _ => new Structure(template, state)
        };
}
