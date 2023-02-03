namespace IncrementalSheep;

public class SheepState
{
    public int Id { get; init; }
    public string Name { get; init; }
    public SheepJobId JobId { get; init; }
    public JobState JobState { get; init; }

    public SheepState(int id, string name, SheepJobId jobId, JobState jobState)
    {
        Id = id;
        Name = name;
        JobId = jobId;
        JobState = jobState;
    }
}
