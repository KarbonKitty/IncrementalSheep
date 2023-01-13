namespace IncrementalSheep;

public class GameState
{
    public DateTime LastTick { get; set; }
    public double LastDiff { get; set; }
    public required ResourceWarehouse Resources { get; set; }
    public required Building[] Buildings { get; set; }
    public required List<Sheep> Sheep { get; set; }
    public required List<Hunt> Hunts { get; set; }
    public Branch SelectedBranch { get; set; }
    public Building? SelectedBuilding { get; set; }
    public Sheep? SelectedSheep { get; set; }
    public Hunt? SelectedHunt { get; set; }
    public required SheepJob[] Jobs { get; set; }
}
