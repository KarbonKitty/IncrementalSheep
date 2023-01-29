namespace IncrementalSheep;

public static class ServiceHelpers
{
    public static Structure StructureFactory(StructureTemplate template, StructureState state)
        => template switch
        {
            BuildingTemplate bt => new Building(bt, state),
            _ => new Structure(template, state)
        };
}
