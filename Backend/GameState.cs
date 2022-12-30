namespace IncrementalSheep;

public class GameState
{
    public DateTime LastTick { get; set; }
    public double LastDiff { get; set; }
    public ResourceValue Resources { get; set; }
    public Building[] Buildings { get; set; }
    public Building? SelectedBuilding { get; set; }
    public List<Sheep> Sheep { get; set; }
    public Sheep SelectedSheep { get; set; }
    public Branch SelectedBranch { get; set; }
    public SheepJob[] Jobs { get; set; }
}
