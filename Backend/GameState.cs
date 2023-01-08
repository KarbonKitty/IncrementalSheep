namespace IncrementalSheep;

public class GameState
{
    public DateTime LastTick { get; set; }
    public double LastDiff { get; set; }
    public ResourceWarehouse Resources { get; set; }
    public Building[] Buildings { get; set; }
    public List<Sheep> Sheep { get; set; }
    public List<Hunt> Hunts { get; set; }
    public Branch SelectedBranch { get; set; }
    public Building? SelectedBuilding { get; set; }
    public Sheep? SelectedSheep { get; set; }
    public Hunt? SelectedHunt { get; set; }
    public SheepJob[] Jobs { get; set; }
}
